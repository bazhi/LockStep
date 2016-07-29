using UnityEngine;
using System.Collections;

namespace Lockstep
{
	public class FixedAABB2D
	{
		private long XMin;

		private long XMax;

		private long YMin;

		private long YMax;

		private void init (Vector2d center, long halfx, long halfy)
		{
			XMin = center.x - halfx;
			XMax = center.x + halfx;
			YMin = center.y - halfy;
			YMax = center.y + halfy;
		}

		public FixedAABB2D (Vector2d center, long halfx, long halfy)
		{
			init (center, halfx, halfy);
		}

		public FixedAABB2D (Vector2 center, float halfx, float halfy)
		{
			init (Vector2d (center), FixedMath.Create (halfx), FixedMath.Create (halfy));
		}

		public void update (Vector2 center, float halfx, float halfy)
		{
			init (Vector2d (center), FixedMath.Create (halfx), FixedMath.Create (halfy));
		}

		public bool contains (Vector2d p)
		{
			if (XMin <= p.x && p.x <= XMax && YMin <= p.y && p.y <= YMax) {
				return true;
			} else {
				return false;
			}
		}

		public bool intersect (Vector2d center, long halfx, long halfy)
		{
			if (center.x + halfx >= XMin && center.x - halfx <= XMax && center.y + halfy >= YMin && center.y - halfy <= YMax) {
				return true;
			} else {
				return false;
			}
		}

		public bool intersect (FixedAABB2D aabb)
		{
			if (aabb.XMax >= aabb.XMin && aabb.XMin <= XMax && aabb.YMax >= YMin && aabb.YMin <= YMax) {
				return true;
			} else {
				return false;
			}
		}
	}
}


