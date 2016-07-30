using UnityEngine;
using System.Collections;

namespace Lockstep.Mono
{
	[RequireComponent(typeof(BoundingBox))]
	public class Blocker : EnvironmentObject
	{
		static readonly FastList<Vector2d> bufferCoordinates = new FastList<Vector2d>();

		public BoundingBox CachedBody { get; private set; }

		#if DEBUG
		public bool m_bLog = false;
		#endif

		protected override void OnInitialize()
		{
			CachedBody = GetComponent<BoundingBox>();
			CachedBody.Initialize();
		}


		protected override void OnLateInitialize()
		{
			base.OnLateInitialize();

			if (CachedBody.IsNotNull()) {
				bufferCoordinates.FastClear();
				CachedBody.GetCoveredSnappedPositions(GridManager.Spacing, bufferCoordinates);
				foreach (Vector2d vec in bufferCoordinates) {
					GridNode node = GridManager.GetNode(vec.x, vec.y);
					if (node.IsNotNull()) {
						//Debug.LogFormat("GridManager GetNode Null x {0:F}, y {1:F}", vec.x.ToFloat(), vec.y.ToFloat());
						node.AddObstacle();
					}
					#if DEBUG
					if (m_bLog) {
						Debug.Log(vec.ToString());
					}
					#endif
				}
			}
		}
	}
}


