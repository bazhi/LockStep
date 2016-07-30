using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Lockstep
{
	[CustomEditor(typeof(GameManager))]
	public class EditorGameManager : Editor
	{

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			GameManager saver = this.target as GameManager;
			EditorGUI.BeginChangeCheck();
			if (GUILayout.Button("Load Savers")) {
				saver.LoadSavers();
				EditorUtility.SetDirty(target);
				serializedObject.Update();
			}

		}

	}
}
