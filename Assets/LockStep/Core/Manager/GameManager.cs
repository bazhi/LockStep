using UnityEngine;
using System.Collections;
using Lockstep.Mono;
using System.Diagnostics;
using System.Collections.Generic;

namespace Lockstep
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }

		[HideInInspector]
		public EnvironmentSaver[] m_Savers;
		[HideInInspector]
		public Blocker[] m_Blockers;

		private Dictionary<DynamicBlocker, DynamicBlocker> m_DynamicBlockers = new Dictionary<DynamicBlocker, DynamicBlocker>();

		void Awake()
		{
			Instance = this;

			foreach (var save in m_Savers) {
				save.Save();
			}

			foreach (var save in m_Savers) {
				save.Apply();
			}
			LockstepManager.Initialize(this);
		}

		protected void Start()
		{
			foreach (var obj in m_Blockers) {
				obj.Initialize();
			}

			foreach (var obj in m_Blockers) {
				obj.LateInitialize();
			}
		}

		public void AddDynamicBlocker(DynamicBlocker blocker)
		{
			m_DynamicBlockers.Add(blocker, blocker);
			blocker.Initialize();
			blocker.LateInitialize();
		}

		public void RemoveDynamicBlocker(DynamicBlocker blocker)
		{
			m_DynamicBlockers.Remove(blocker);
		}

		public void LoadSavers()
		{
			m_Savers = Object.FindObjectsOfType<EnvironmentSaver>();
			m_Blockers = Object.FindObjectsOfType<Blocker>();
			UnityEngine.Debug.LogFormat("LoadSavers Success Savers({0:D}), Blockers({1:D})",
				m_Savers.Length, m_Blockers.Length);
		}

		public void FixedUpdate()
		{
			foreach (var item in m_DynamicBlockers) {
				item.Value.Simulate();
			}
		}
	}
}