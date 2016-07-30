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
		//[SerializeField, FormerlySerializedAs("Shape")]
		private BoundingType Shape = BoundingType.AABox;

		//public BoundingType Shape { get { return _shape; } }
		[HideInInspector]
		public Bounds m_Bound;

		private FixedAABB2D m_AABB = new FixedAABB2D(Vector2d.zero, 0, 0);

		public void Initialize()
		{
			m_AABB.update(m_Bound.center.x, m_Bound.center.z, m_Bound.extents.x, m_Bound.extents.z);
		}

		long GetCeiledSnap(long f, long snap)
		{
			return (f + snap - 1) / snap * snap;
		}

		long GetFlooredSnap(long f, long snap)
		{
			return (f / snap) * snap;
		}

		public void AutoSet()
		{
			Collider col = GetComponent<Collider>();
			if (col) {
				m_Bound = col.bounds;
			}
		}

		public void BuildBounds()
		{
			
		}

		public void UpdateValues()
		{
			m_AABB.update(transform.position.x, transform.position.z);
		}

		public void GetCoveredSnappedPositions(long snapSpacing, FastList<Vector2d> output)
		{
			long xmin = GetFlooredSnap(m_AABB.XMin - FixedMath.Half, snapSpacing);
			long ymin = GetFlooredSnap(m_AABB.YMin - FixedMath.Half, snapSpacing);

			long xmax = GetCeiledSnap(m_AABB.YMax + FixedMath.Half - xmin, snapSpacing) + xmin;
			long ymax = GetCeiledSnap(m_AABB.YMax + FixedMath.Half - ymin, snapSpacing) + ymin;
			//Debug.LogFormat("XMin{0:F}, YMin{1:F}, XMax{2:F}, YMax{3:F}", m_AABB.XMin.ToFloat(), m_AABB.YMin.ToFloat(), m_AABB.XMax.ToFloat(), m_AABB.YMax.ToFloat());
			//Debug.LogFormat("xmin{0:F}, ymin{1:F}, xmax{2:F}, ymax{3:F}", xmin.ToFloat(), ymin.ToFloat(), xmax.ToFloat(), ymax.ToFloat());
			//Used for getting snapped positions this body covered
			for (long x = xmin; x < xmax; x += snapSpacing) {
				for (long y = ymin; y < ymax; y += snapSpacing) {
					Vector2d checkPos = new Vector2d(x, y);

					if (IsPositionCovered(checkPos)) {
						output.Add(checkPos);
						//Debug.Log(checkPos.ToString());
					} else {
						//Debug.Log(checkPos.ToString() + "unAdd");
					}
				}
			}
		}

		public bool IsPositionCovered(Vector2d position)
		{
			switch (this.Shape) {
				case BoundingType.Circle:
//					long maxDistance = this._radius + FixedMath.Half;
//					maxDistance *= maxDistance;
//					if ((_position - position).FastMagnitude() > maxDistance)
//						return false;
					goto case BoundingType.AABox;
				case BoundingType.AABox:
					return m_AABB.intersect(position, FixedMath.Half, FixedMath.Half);
				case BoundingType.Polygon:
					break;
			}

			return false;
		}
		#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			AutoSet();
			switch (this.Shape) {
				case BoundingType.Circle:
					//Gizmos.DrawWireSphere(this.transform.position, this.Radius);
					break;
				case BoundingType.AABox:
					Gizmos.DrawWireCube(m_Bound.center, m_Bound.size);
					break;
				case BoundingType.Polygon:
					//Gizmos.DrawWireSphere(this.transform.position, this.Radius);
					break;
			}
		}
		#endif
	}
}

