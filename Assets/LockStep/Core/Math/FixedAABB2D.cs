using UnityEngine;
using System.Collections;

namespace Lockstep
{
	public class FixedAABB2D
	{
		private long XMin { get; private set; }

		private long XMax { get; private set; }

		private long YMin { get; private set; }

		private long YMax { get; private set; }

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


