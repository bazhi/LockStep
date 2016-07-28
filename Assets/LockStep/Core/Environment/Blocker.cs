using UnityEngine;
using System.Collections;

namespace Lockstep.Mono
{
	[RequireComponent(typeof(BoundingBox))]
	public class Blocker : EnvironmentObject
	{
		static readonly FastList<Vector2d> bufferCoordinates = new FastList<Vector2d>();

		public BoundingBox CachedBody { get; private set; }

		protected override void OnInitialize()
		{

		}


		protected override void OnLateInitialize()
		{
			base.OnLateInitialize();
			CachedBody = GetComponent<BoundingBox>();
			if (CachedBody.IsNotNull()) {
				bufferCoordinates.FastClear();
				CachedBody.GetCoveredSnappedPositions(GridManager.Spacing, bufferCoordinates);
				foreach (Vector2d vec in bufferCoordinates) {
					GridNode node = GridManager.GetNode(vec.x, vec.y);
					if (node == null) {
						Debug.Log("GridManager getNode Null");
						continue;
					}

					node.AddObstacle();
				}
			}
		}
	}
}


