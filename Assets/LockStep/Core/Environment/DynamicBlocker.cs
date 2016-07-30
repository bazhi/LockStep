using UnityEngine;
using System.Collections;

namespace Lockstep.Mono
{
	[RequireComponent(typeof(BoundingBox))]
	public class DynamicBlocker : EnvironmentObject
	{
		static readonly FastList<Vector2d> bufferCoordinates = new FastList<Vector2d>();

		public BoundingBox CachedBody { get; private set; }

		FastList<GridNode> LastCoordinates = new FastList<GridNode>();

		public void Start()
		{
			GameManager.Instance.AddDynamicBlocker(this);
		}

		public void OnDestroy()
		{
			RemoveLastCoordinates();
			GameManager.Instance.RemoveDynamicBlocker(this);
		}

		protected override void OnInitialize()
		{
			CachedBody = GetComponent<BoundingBox>();
			CachedBody.Initialize();
		}

		public void RemoveAllCoordinates()
		{
			for (int i = 0; i < LastCoordinates.Count; i++) {
				GridNode node = LastCoordinates[i];
				node.RemoveObstacle();
			}
		}

		public void ReAddAllCoordinates()
		{
			for (int i = 0; i < LastCoordinates.Count; i++) {
				GridNode node = LastCoordinates[i];
				node.AddObstacle();
			}
		}

		private void RemoveLastCoordinates()
		{
			for (int i = 0; i < LastCoordinates.Count; i++) {
				GridNode node = LastCoordinates[i];
				node.RemoveObstacle();
			}
			LastCoordinates.FastClear();
		}

		private void UpdateCoordinates()
		{
			if (CachedBody.IsNotNull()) {
				bufferCoordinates.FastClear();
				CachedBody.UpdateValues();
				CachedBody.GetCoveredSnappedPositions(GridManager.Spacing, bufferCoordinates);
				foreach (Vector2d vec in bufferCoordinates) {
					GridNode node = GridManager.GetNode(vec.x, vec.y);
					if (node == null) {
						//Debug.Log("GridManager getNode Null");
						continue;
					}

					node.AddObstacle();
					LastCoordinates.Add(node);
				}
			}
		}

		protected override void OnLateInitialize()
		{
			base.OnLateInitialize();
			UpdateCoordinates();
		}

		public void Simulate()
		{
			RemoveLastCoordinates();
			UpdateCoordinates();
		}
	}
}


