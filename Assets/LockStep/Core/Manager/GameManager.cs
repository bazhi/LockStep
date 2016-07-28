using UnityEngine;
using System.Collections;
using Lockstep.Mono;

namespace Lockstep
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }

		public GameObject m_SaverObject;
		public GameObject m_MapObject;

		public EnvironmentSaver[] m_Savers;

		public BoundingBox[] m_Boundings;

		public EnvironmentObject[] m_Enviroments;

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

			foreach (var obj in m_Enviroments) {
				obj.Initialize();
			}

			foreach (var obj in m_Enviroments) {
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
				m_Enviroments = m_MapObject.GetComponentsInChildren<EnvironmentObject>();
			}
		}
	}
}