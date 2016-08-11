using UnityEngine;

namespace Lockstep
{
	public class FixedAABB2D
	{
		public Vector2d m_Min = Vector2d.zero;
		public Vector2d m_Max = Vector2d.zero;

		public Vector2d m_Center = Vector2d.one;

		public Vector2d m_Half = Vector2d.zero;

		public FixedAABB2D()
		{

		}

		private void init(Vector2d center, long halfx, long halfy)
		{
			m_Min.x = center.x - halfx;
			m_Max.x = center.x + halfx;
			m_Min.y = center.y - halfy;
			m_Max.y = center.y + halfy;
		}

		public FixedAABB2D(Vector2d center, long halfx, long halfy)
		{
			m_Center = center;
			m_Half.x = halfx;
			m_Half.y = halfy;
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
			m_Half.x = FixedMath.Create(halfx);
			m_Half.y = FixedMath.Create(halfy);
			init(m_Center, m_Half.x, m_Half.y);
		}

		public void update(float x, float y)
		{
			m_Center.x = FixedMath.Create(x);
			m_Center.y = FixedMath.Create(y);
			init(m_Center, m_Half.x, m_Half.y);
		}

		public void update(Vector2 center)
		{
			update(center.x, center.y);
		}

		public bool contains(Vector2d p)
		{
			if (m_Min.x <= p.x && p.x <= m_Max.x && m_Min.y <= p.y && p.y <= m_Max.y) {
				return true;
			} else {
				return false;
			}
		}

		public bool intersect(Vector2d center, long halfx, long halfy)
		{
			if (center.x + halfx >= m_Min.x && center.x - halfx <= m_Max.x && center.y + halfy >= m_Min.y && center.y - halfy <= m_Max.y) {
				return true;
			} else {
				return false;
			}
		}

		public bool intersect(FixedAABB2D aabb)
		{
			if (aabb.m_Max.x >= aabb.m_Min.x && aabb.m_Min.x <= m_Max.x && aabb.m_Max.y >= m_Min.y && aabb.m_Min.y <= m_Max.y) {
				return true;
			} else {
				return false;
			}
		}

		public override string ToString()
		{
			return string.Format("min({0:F},{1:F}), max({2:F},{3:F})", m_Min.x.ToFloat(), m_Min.y.ToFloat(), m_Max.x.ToFloat(), m_Max.y.ToFloat());
		}
	}
}


