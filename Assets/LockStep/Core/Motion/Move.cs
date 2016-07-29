using UnityEngine;
using System.Collections;
using System.Net;

namespace Lockstep
{
	public class Move : MonoBehaviour
	{
		public Vector3 m_Destination;

		private bool m_bArrived = false;
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

		// Use this for initialization
		void Start()
		{
			
		}

		void Update()
		{
			m_CurDis = Vector2.Distance(transform.position, m_TargetPos);
			if (m_CurDis >= m_LastDis) {
				m_bArrivedMiddle = true;
			} else {
				transform.Translate(m_Forward * m_Speed * Time.deltaTime);
				m_LastDis = m_CurDis;
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
					if (Pathfinder.FindPath(StartPos, EndPos, m_MyPath)) {
						m_Index = 0;
						m_bFindPath = true;
						m_bArrivedMiddle = true;
						Debug.Log("Find Path");
					} else {
						m_bFindPath = false;
						Debug.Log("Not Find Path");
					}
				} else {
					if (m_Index < m_MyPath.Count) {
						if (m_bArrivedMiddle) {
							m_Target = m_MyPath[m_Index];
							++m_Index;
							m_bArrivedMiddle = false;
							m_TargetPos = m_Target.ToVector3(transform.position.y);
							transform.LookAt(m_TargetPos);
							m_CurDis = Vector2.Distance(transform.position, m_TargetPos);
							m_LastDis = m_CurDis + 1.0f;
						}
					} else {
						m_bArrived = true;
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



