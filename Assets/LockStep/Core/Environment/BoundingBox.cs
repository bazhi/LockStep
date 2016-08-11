using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using System.ComponentModel;
using System;

namespace Lockstep.Mono
{
	public enum BoundingType : byte
	{
		None,
		Circle,
		AABox,
		OBB
	}

	[RequireComponent(typeof(Collider))]
	public sealed partial class BoundingBox : MonoBehaviour
	{
		//[SerializeField, FormerlySerializedAs("Shape")]
		public BoundingType Shape = BoundingType.AABox;

		//public BoundingType Shape { get { return _shape; } }
		public BBBuilder m_BBBuilder = new BBBuilder();

		public FixedAABB2D m_AABB = new FixedAABB2D(Vector2d.zero, 0, 0);
		public FixedOBB2D m_OBB = new FixedOBB2D();

		public bool m_bMove = false;

		public void Initialize()
		{
			UpdateValues();
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
				Type type = col.GetType();
				if (type == typeof(SphereCollider)) {
					Shape = BoundingType.Circle;
					GetComponent<SphereCollider>().getOBBB(ref m_BBBuilder);
				} else if (type == typeof(CapsuleCollider)) {
					Shape = BoundingType.Circle;
					GetComponent<CapsuleCollider>().getOBBB(ref m_BBBuilder);
				} else if (type == typeof(BoxCollider)) {
					GetComponent<BoxCollider>().getOBBB(ref m_BBBuilder);
					if (Math.Abs(m_BBBuilder.degreeY) < 0.1f) {
						Shape = BoundingType.AABox;
					} else {
						Shape = BoundingType.OBB;
					}
				}
			}
			UpdateValues();
		}

		public void UpdateValues()
		{
			if (Shape == BoundingType.OBB) {
				m_BBBuilder.getOBB2D(ref m_OBB);
			} else {
				m_BBBuilder.getAABB2D(ref m_AABB);
			}
			if (m_bMove) {
				m_AABB.update(transform.position.x, transform.position.z);
			}
		}

		void GetCoveredSnappedPositionsAABB(long snapSpacing, FastList<Vector2d> output)
		{
			long xmin = GetFlooredSnap(m_AABB.m_Min.x - FixedMath.Half, snapSpacing);
			long ymin = GetFlooredSnap(m_AABB.m_Min.y - FixedMath.Half, snapSpacing);

			long xmax = GetCeiledSnap(m_AABB.m_Max.x + FixedMath.Half - xmin, snapSpacing) + xmin;
			long ymax = GetCeiledSnap(m_AABB.m_Max.y + FixedMath.Half - ymin, snapSpacing) + ymin;
			for (long x = xmin; x < xmax; x += snapSpacing) {
				for (long y = ymin; y < ymax; y += snapSpacing) {
					var checkPos = new Vector2d(x, y);

					if (IsPositionCovered(checkPos)) {
						output.Add(checkPos);
					}
				}
			}
		}

		void GetCoveredSnappedPositionsOBB(long snapSpacing, FastList<Vector2d> output)
		{
			long xmin = GetFlooredSnap(m_OBB.m_Min.x - FixedMath.Half, snapSpacing);
			long ymin = GetFlooredSnap(m_OBB.m_Min.y - FixedMath.Half, snapSpacing);

			long xmax = GetCeiledSnap(m_OBB.m_Max.x + FixedMath.Half - xmin, snapSpacing) + xmin;
			long ymax = GetCeiledSnap(m_OBB.m_Max.y + FixedMath.Half - ymin, snapSpacing) + ymin;
			for (long x = xmin; x < xmax; x += snapSpacing) {
				for (long y = ymin; y < ymax; y += snapSpacing) {
					var checkPos = new Vector2d(x, y);

					if (IsPositionCovered(checkPos)) {
						output.Add(checkPos);
					}
				}
			}
		}

		public void GetCoveredSnappedPositions(long snapSpacing, FastList<Vector2d> output)
		{
			switch (this.Shape) {
				case BoundingType.OBB:
					GetCoveredSnappedPositionsOBB(snapSpacing, output);
					break;
				default:
					GetCoveredSnappedPositionsAABB(snapSpacing, output);
					break;
			}
		}

		public bool IsPositionCovered(Vector2d position)
		{
			switch (this.Shape) {
				case BoundingType.Circle:
					var radis = Math.Max(m_AABB.m_Half.x, m_AABB.m_Half.y);
					long maxDistance = radis + FixedMath.Half;
					maxDistance *= maxDistance;
					if ((m_AABB.m_Center - position).FastMagnitude() > maxDistance)
						return false;
					goto case BoundingType.AABox;
				case BoundingType.AABox:
					return m_AABB.intersect(position, FixedMath.Half, FixedMath.Half);
				case BoundingType.OBB:
					var obb = new FixedOBB2D(position.x, position.y, FixedMath.Half, FixedMath.Half);
					return m_OBB.isCollision(obb);
			}

			return false;
		}
		#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if (Application.isPlaying) {
				return;
			}
			Gizmos.color = Color.red;
			switch (this.Shape) {
				case BoundingType.Circle:
					Gizmos.DrawWireSphere(m_BBBuilder.center, Math.Max(m_BBBuilder.size.x, m_BBBuilder.size.z) / 2);
					break;
				case BoundingType.AABox:
					Gizmos.DrawWireCube(m_BBBuilder.center, m_BBBuilder.size);
					break;
				case BoundingType.OBB:
					m_BBBuilder.Draw();
					break;
			}
		}
		#endif
	}
}

