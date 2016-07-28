using UnityEngine;
using System.Collections;
using Lockstep;

namespace Lockstep.Mono
{
	public class GridMono : EnvironmentSaver
	{
		[SerializeField]
		private Vector2d _mapCenter;

		public Vector2d Offset {
			get {
				return _mapCenter - new Vector2d(_mapWidth / 2, _mapHeight / 2);
			}
		}

		[SerializeField]
		private int _mapWidth = 100;

		public int MapWidth { get { return _mapWidth; } }

		[SerializeField]
		private int _mapHeight = 100;

		public int MapHeight { get { return _mapHeight; } }

		[SerializeField]
		private bool _useDiagonalConnections = true;

		public bool UseDiagonalConnetions { get { return _useDiagonalConnections; } }

		[Range(0.1f, 10.0f)]
		public float m_GridSpacing = 1.0f;

		FastList<Vector2d> m_FindPath;
		Vector2d StartPos;

		protected override void OnSave()
		{
			this._mapCenter = new Vector2d(transform.position);
		}

		protected override void OnApply()
		{
			GridManager.Settings = new GridSettings(this.MapWidth, this.MapHeight, this.Offset.x, this.Offset.y, this.UseDiagonalConnetions);
			Invoke("showPath", 2.0f);
		}

		void showPath()
		{
			m_FindPath = new FastList<Vector2d>();
			StartPos = new Vector2d(FixedMath.Create(-49), FixedMath.Create(-49));
			if (Pathfinder.FindPath(StartPos, new Vector2d(FixedMath.Create(49), FixedMath.Create(49)), m_FindPath)) {
				Debug.Log("find Path");
			} else {
				Debug.Log("can not find path");
			}
		}

		#if UNITY_EDITOR
		public bool Show;

		void OnDrawGizmos()
		{
			if (!Show)
				return;

			Gizmos.color = Color.green;
			Vector3 offset = Offset.ToVector3(transform.position.y);
			Vector3 scale = Vector3.one * .9f * m_GridSpacing;
			scale.y = 0.01f;
			for (int x = 0; x < MapWidth; x++) {
				for (int y = 0; y < MapHeight; y++) {
					Vector3 drawPos = new Vector3(x, 0f, y);
					drawPos += offset;
					if (Application.isPlaying) {
						var Grid = GridManager.GetNode(x, y);
						if (Grid.Unpassable()) {
							Gizmos.color = Color.red;
						} else {
							Gizmos.color = Color.green;
						}
					}
					drawPos.x *= m_GridSpacing;
					drawPos.z *= m_GridSpacing;
					Gizmos.DrawCube(drawPos, scale);
				}
			}
			Vector3 lastPos = StartPos.ToVector3(transform.position.y);
			lastPos.x *= m_GridSpacing;
			lastPos.z *= m_GridSpacing;
			if (Application.isPlaying && m_FindPath.IsNotNull()) {
				Gizmos.color = Color.yellow;
				int count = m_FindPath.Count;
				for (int i = 0; i < count; ++i) {
					var node = m_FindPath[i];
					var drawPos = node.ToVector3(transform.position.y);
					drawPos.x *= m_GridSpacing;
					drawPos.z *= m_GridSpacing;
					Gizmos.DrawLine(lastPos, drawPos);
					lastPos = drawPos;
				}
			}
		}
		#endif
	}
}



