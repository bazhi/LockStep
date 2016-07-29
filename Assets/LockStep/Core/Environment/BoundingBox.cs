using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using System.ComponentModel;

namespace Lockstep.Mono
{
	public enum BoundingType : byte
	{
		None,
		Circle,
		AABox,
		Polygon
	}

	public sealed partial class BoundingBox : MonoBehaviour
	{
		[SerializeField, FormerlySerializedAs ("Shape")]
		private BoundingType _shape = BoundingType.AABox;

		public BoundingType Shape { get { return _shape; } }

		private long _radius = FixedMath.Half;
		private Vector3d _ABSize = new Vector3d (Vector3.one);

		public float Radius = 1.0f;
		public Vector3 ABSize = Vector3.one;

		public FixedAABB2D m_AABB;

		public long XMin { get; private set; }

		public long XMax { get; private set; }

		public long YMin { get; private set; }

		public long YMax { get; private set; }

		private Vector2d _position;

		public void Initialize ()
		{
			_position = new Vector2d (transform.position.x, transform.position.z);
			_radius = FixedMath.Create (Radius);
			_ABSize.x = FixedMath.Create (ABSize.x);
			_ABSize.z = FixedMath.Create (ABSize.z);
			BuildBounds ();
		}

		long GetCeiledSnap (long f, long snap)
		{
			return (f + snap - 1) / snap * snap;
		}

		long GetFlooredSnap (long f, long snap)
		{
			return (f / snap) * snap;
		}

		public void AutoSet ()
		{
			Collider[] cols = GetComponentsInChildren <Collider> ();
			foreach (var col in cols) {
				Debug.Log (col.bounds.ToString ());
				m_AABB.update (col.bounds.center.x, col.bounds.center.y, col.bounds.extents.x, col.bounds.extents.z);
			}
		}

		public void BuildBounds ()
		{
			if (Shape == BoundingType.Circle) {
				XMin = -_radius + _position.x;
				XMax = _radius + _position.x;
				YMin = -_radius + _position.y;
				YMax = _radius + _position.y;
			} else if (Shape == BoundingType.AABox) {
				XMin = -_ABSize.x / 2 + _position.x;
				XMax = _ABSize.x / 2 + _position.x;
				YMin = -_ABSize.z / 2 + _position.y;
				YMax = _ABSize.z / 2 + _position.y;
			} else if (Shape == BoundingType.Polygon) {

			}
		}

		public void UpdateValues ()
		{
			_position = new Vector2d (transform.position.x, transform.position.z);
			BuildBounds ();
		}

		public void GetCoveredSnappedPositions (long snapSpacing, FastList<Vector2d> output)
		{
			long xmin = GetFlooredSnap (this.XMin - FixedMath.Half, snapSpacing);
			long ymin = GetFlooredSnap (this.YMin - FixedMath.Half, snapSpacing);

			long xmax = GetCeiledSnap (this.XMax + FixedMath.Half - xmin, snapSpacing) + xmin;
			long ymax = GetCeiledSnap (this.YMax + FixedMath.Half - ymin, snapSpacing) + ymin;
			//Debug.LogFormat("xmin{0:F}, ymin{1:F}, xmax{2:F}, ymax{3:F}", XMin.ToFloat(), YMin.ToFloat(), XMax.ToFloat(), YMax.ToFloat());
			//Debug.LogFormat("xmin{0:F}, ymin{1:F}, xmax{2:F}, ymax{3:F}", xmin.ToFloat(), ymin.ToFloat(), xmax.ToFloat(), ymax.ToFloat());
			//Used for getting snapped positions this body covered
			for (long x = xmin; x < xmax; x += snapSpacing) {
				for (long y = ymin; y < ymax; y += snapSpacing) {
					Vector2d checkPos = new Vector2d (x, y);

					if (IsPositionCovered (checkPos)) {
						output.Add (checkPos);
						//Debug.Log(checkPos.ToString());
					} else {
						//Debug.Log(checkPos.ToString() + "unAdd");
					}
				}
			}
		}

		public bool IsPositionCovered (Vector2d position)
		{
			switch (this.Shape) {
			case BoundingType.Circle:
				long maxDistance = this._radius + FixedMath.Half;
				maxDistance *= maxDistance;
				if ((_position - position).FastMagnitude () > maxDistance)
					return false;
				goto case BoundingType.AABox;
			case BoundingType.AABox:
				return position.x + FixedMath.Half >= this.XMin && position.x - FixedMath.Half <= this.XMax
				&& position.y + FixedMath.Half >= this.YMin && position.y - FixedMath.Half <= this.YMax;
			case BoundingType.Polygon:
				break;
			}

			return false;
		}

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.red;
			switch (this.Shape) {
			case BoundingType.Circle:
				Gizmos.DrawWireSphere (this.transform.position, this.Radius);
				break;
			case BoundingType.AABox:
				Gizmos.DrawWireCube (this.transform.position, ABSize);
				break;
			case BoundingType.Polygon:
				Gizmos.DrawWireSphere (this.transform.position, this.Radius);
				break;
			}
		}
	}
}

