using UnityEngine;
using System.Collections;

namespace Lockstep
{
	[System.Serializable]
	public class FixedAABB2D
	{
		[HideInInspector, FixedNumber]
		public long XMin;
		[HideInInspector, FixedNumber]
		public long XMax;
		[HideInInspector, FixedNumber]
		public long YMin;
		[HideInInspector, FixedNumber]
		public long YMax;

		public Vector2d m_Center = Vector2d.one;
		[FixedNumber]
		public long m_HalfX;
		[FixedNumber]
		public long m_HalfY;

		public FixedAABB2D()
		{

		}

		private void init(Vector2d center, long halfx, long halfy)
		{
			XMin = center.x - halfx;
			XMax = center.x + halfx;
			YMin = center.y - halfy;
			YMax = center.y + halfy;
		}

		public FixedAABB2D(Vector2d center, long halfx, long halfy)
		{
			m_Center = center;
			m_HalfX = halfx;
			m_HalfY = halfy;
			init(center, halfx, halfy);
		}

		public FixedAABB2D(Vector2 center, float halfx, float halfy)
		{
			update(center, halfx, halfy);
		}

		public void update(Vector2 center, float halfx, float halfy)
		{
			update(center.x, center.y, halfx, halfy);
		}

		public void update(float x, float y, float halfx, float halfy)
		{
			m_Center.x = FixedMath.Create(x);
			m_Center.y = FixedMath.Create(y);
			m_HalfX = FixedMath.Create(halfx);
			m_HalfY = FixedMath.Create(halfy);
			init(m_Center, m_HalfX, m_HalfY);
		}

		public void update(float x, float y)
		{
			m_Center.x = FixedMath.Create(x);
			m_Center.y = FixedMath.Create(y);
			init(m_Center, m_HalfX, m_HalfY);
		}

		public void update(Vector2 center)
		{
			update(center.x, center.y);
		}

		public bool contains(Vector2d p)
		{
			if (XMin <= p.x && p.x <= XMax && YMin <= p.y && p.y <= YMax) {
				return true;
			} else {
				return false;
			}
		}

		public bool intersect(Vector2d center, long halfx, long halfy)
		{
			if (center.x + halfx >= XMin && center.x - halfx <= XMax && center.y + halfy >= YMin && center.y - halfy <= YMax) {
				return true;
			} else {
				return false;
			}
		}

		public bool intersect(FixedAABB2D aabb)
		{
			if (aabb.XMax >= aabb.XMin && aabb.XMin <= XMax && aabb.YMax >= YMin && aabb.YMin <= YMax) {
				return true;
			} else {
				return false;
			}
		}
	}
}


