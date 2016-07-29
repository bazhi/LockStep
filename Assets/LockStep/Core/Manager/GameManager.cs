using UnityEngine;
using System.Collections;
using Lockstep.Mono;
using System.Diagnostics;

namespace Lockstep
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }

		public GameObject m_SaverObject;
		public GameObject m_MapObject;

		[HideInInspector]
		public EnvironmentSaver[] m_Savers;
		[HideInInspector]
		public BoundingBox[] m_Boundings;
		[HideInInspector]
		public Blocker[] m_Blockers;

		[HideInInspector]
		public DynamicBlocker[] m_DynamicBlockers;

		protected void Start()
		{
			foreach (var save in m_Savers) {
				save.Save();
			}

			foreach (var save in m_Savers) {
				save.Apply();
			}

			Instance = this;
			LockstepManager.Initialize(this);

			foreach (var box in m_Boundings) {
				box.Initialize();
			}

			foreach (var obj in m_Blockers) {
				obj.Initialize();
			}

			foreach (var obj in m_DynamicBlockers) {
				obj.Initialize();
			}

			foreach (var obj in m_Blockers) {
				obj.LateInitialize();
			}

			foreach (var obj in m_DynamicBlockers) {
				obj.LateInitialize();
			}
		}

		public void LoadSavers()
		{
			if (m_SaverObject.IsNotNull()) {
				m_Savers = m_SaverObject.GetComponents<EnvironmentSaver>();
			}
			if (m_MapObject.IsNotNull()) {
				m_Boundings = m_MapObject.GetComponentsInChildren<BoundingBox>();
				m_Blockers = m_MapObject.GetComponentsInChildren<Blocker>();
				m_DynamicBlockers = m_MapObject.GetComponentsInChildren<DynamicBlocker>();
			}
		}

		public void FixedUpdate()
		{
			foreach (var obj in m_DynamicBlockers) {
				obj.Simulate();
			}
		}
	}
}