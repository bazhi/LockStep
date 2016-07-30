using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Lockstep.Mono
{
	[CustomEditor(typeof(BoundingBox))]
	public class EditorBoundingBox : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			BoundingBox saver = this.target as BoundingBox;
			EditorGUI.BeginChangeCheck();
			if (GUILayout.Button("Auto Set")) {

				saver.AutoSet();
				EditorUtility.SetDirty(target);
				serializedObject.Update();
			}

		}
	}
}
