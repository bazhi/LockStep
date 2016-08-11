using UnityEngine;
using System.Collections;
using System.Net;
using Lockstep.Mono;
using UnityEngine.Networking;

namespace Lockstep
{
	[RequireComponent(typeof(DynamicBlocker))]
	public class Move : NetworkBehaviour
	{
		private Vector3 m_Destination;
		private bool m_bArrived = true;

		private bool m_bArrivedMiddle = true;
		private bool m_bFindPath = false;

		FastList<Vector2d> m_MyPath = new FastList<Vector2d>();
		int m_Index = 0;
		Vector2d StartPos;
		Vector2d EndPos;

		Vector2d m_Target;

		public float m_Speed = 1.0f;
		Vector3 m_Forward = Vector3.forward;
		Vector3 m_TargetPos;
		float m_LastDis = 0.0f;
		float m_CurDis = 0.0f;

		DynamicBlocker m_Blocker;
		private int m_SearchCount = 0;

		//UNet
		public GameObject bulletPrefab;

		// Use this for initialization
		void Start()
		{
			m_Blocker = GetComponent<DynamicBlocker>();
			Messenger.AddListener<Vector3>(EventCmd.CMD_ChangeDestination, OnChangeDestination);
		}

		void OnDestroy()
		{
			Messenger.RemoveListener<Vector3>(EventCmd.CMD_ChangeDestination, OnChangeDestination);
		}

		public override void OnStartLocalPlayer()
		{
			GetComponent<MeshRenderer>().material.color = Color.white;
		}

		void OnChangeDestination(Vector3 target)
		{
			if (!isLocalPlayer) {
				return;
			}
			m_Destination = target;
			m_bArrived = false;
			m_bFindPath = false;
			m_MyPath.FastClear();
			m_SearchCount = 0;
		}

		public void OnCollisionEnter(Collision col)
		{
			m_bFindPath = false;
			if (m_SearchCount >= 5) {
				m_bArrived = true;
			}
		}

		[Command]
		void CmdFire()
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit; 
			if (Physics.Raycast(ray, out hit, 1000.0f)) {
				transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
			}

			var bullet = (GameObject)Instantiate(
				             bulletPrefab,
				             transform.position + transform.forward,
				             Quaternion.identity
			             );
			bullet.GetComponent<Rigidbody>().velocity = transform.forward * 10;
			NetworkServer.Spawn(bullet);
			Destroy(bullet, 2.0f);
		}

		void Update()
		{
			if (!isLocalPlayer)
				return;
			if (!m_bArrived && m_bFindPath) {
				m_CurDis = Vector2.Distance(transform.position, m_TargetPos);
				if (m_CurDis >= m_LastDis) {
					m_bArrivedMiddle = true;
					if (m_bFindPath && m_Index >= m_MyPath.Count) {
						m_bArrived = true;
					}
				} else {
					transform.Translate(m_Forward * m_Speed * Time.deltaTime);
					m_LastDis = m_CurDis;
				}
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				CmdFire();
			}
		}
	
		// Update is called once per frame
		void FixedUpdate()
		{
			if (!m_bArrived) {
				if (!m_bFindPath) {
					m_MyPath.FastClear();
					StartPos = new Vector2d(FixedMath.Create(transform.position.x), FixedMath.Create(transform.position.z));
					EndPos = new Vector2d(FixedMath.Create(m_Destination.x), FixedMath.Create(m_Destination.z));
					if (m_Blocker) {
						m_Blocker.RemoveAllCoordinates();
					}
					++m_SearchCount;
					if (Pathfinder.FindPath(StartPos, EndPos, m_MyPath)) {
						m_Index = 0;
						m_bFindPath = true;
						m_bArrivedMiddle = true;
					} else {
						m_bFindPath = false;
						m_bArrived = true;
					}

					if (m_Blocker) {
						m_Blocker.ReAddAllCoordinates();
					}
				}
				if (m_bFindPath) {
					if (m_Index < m_MyPath.Count) {
						if (m_bArrivedMiddle) {
							m_Target = m_MyPath[m_Index];
							++m_Index;
							m_bArrivedMiddle = false;
							m_TargetPos = m_Target.ToVector3(transform.position.y);
							transform.LookAt(m_TargetPos);
							//GetComponent<Rigidbody>().velocity = transform.forward * 10;
							m_CurDis = Vector2.Distance(transform.position, m_TargetPos);
							m_LastDis = m_CurDis + 1.0f;
						}
					}
				}
			}
		}

		#if UNITY_EDITOR
		public bool Show;

		void OnDrawGizmos()
		{
			if (!Show)
				return;
			
			if (Application.isPlaying && m_bFindPath) {
				Gizmos.color = Color.red;
				int count = m_MyPath.Count;
				Vector3 lastPos = m_TargetPos;
				Gizmos.DrawLine(transform.position, lastPos);
				for (int i = m_Index; i < count; ++i) {
					var node = m_MyPath[i];
					var drawPos = node.ToVector3(transform.position.y);
					Gizmos.DrawLine(lastPos, drawPos);
					lastPos = drawPos;
				}
			}
		}
		#endif
	}
}



