using UnityEngine;
using UnityEditor;
using Lockstep.Mono;
using System.Security.Cryptography.X509Certificates;

namespace Lockstep
{
	public static class MenuItems
	{
		[MenuItem("Lockstep/BuildMap")]
		private static void BuildMap()
		{
			GameManager manager = Object.FindObjectOfType<GameManager>();
			if (manager.IsNotNull()) {
				manager.LoadSavers();
				EditorUtility.SetDirty(manager);
			}
		
			BoundingBox[] boxes = Object.FindObjectsOfType<BoundingBox>();
			foreach (var box in boxes) {
				box.AutoSet();
				EditorUtility.SetDirty(box);
			}
		}
	}
}



