using UnityEngine;
using System;

namespace Lockstep
{
	[SerializableAttribute]
	public class BBBuilder
	{
		public float degreeY = 0.0f;
		public Vector3 center;
		public Vector3 size;
		private Vector3 min;
		private Vector3 max;
		private readonly Vector3[] points = new Vector3[8];

		#if UNITY_EDITOR
		[HideInInspector]
		public Vector3[] pointsDraw = new Vector3[8];
		#endif

		public void getAABB2D(ref FixedAABB2D aabb)
		{
			aabb.update(center.x, center.z, size.x / 2, size.z / 2);
		}

		public void getOBB2D(ref FixedOBB2D obb)
		{
			obb.update(center.x, center.z, size.x / 2, size.z / 2);
			obb.setRotation(Mathf.Deg2Rad * degreeY);
		}

		public void Rotate(Quaternion quat)
		{
			updatePoints();
			for (int i = 0; i < points.Length; ++i) {
				points[i] = quat * points[i];
			}
			updateSize();
		}

		void updateBounds()
		{
			min = -size / 2;
			max = size / 2;
		}

		void updatePoints()
		{
			updateBounds();
			points[0].Set(min.x, min.y, min.z);
			points[1].Set(min.x, min.y, max.z);
			points[2].Set(min.x, max.y, max.z);
			points[3].Set(min.x, max.y, min.z);


			points[4].Set(max.x, min.y, min.z);
			points[5].Set(max.x, min.y, max.z);
			points[6].Set(max.x, max.y, max.z);
			points[7].Set(max.x, max.y, min.z);
		}

		#if UNITY_EDITOR
		public void prepareDraw()
		{
			pointsDraw[0].Set(min.x, min.y, min.z);
			pointsDraw[1].Set(min.x, min.y, max.z);
			pointsDraw[2].Set(min.x, max.y, max.z);
			pointsDraw[3].Set(min.x, max.y, min.z);


			pointsDraw[4].Set(max.x, min.y, min.z);
			pointsDraw[5].Set(max.x, min.y, max.z);
			pointsDraw[6].Set(max.x, max.y, max.z);
			pointsDraw[7].Set(max.x, max.y, min.z);
			var quat = Quaternion.Euler(0, degreeY, 0);
			for (int i = 0; i < points.Length; ++i) {
				pointsDraw[i] = quat * pointsDraw[i] + center;
			}
		}
		#endif

		private void updateSize()
		{
			min = points[0];
			max = points[0];
			for (int i = 1; i < points.Length; ++i) {
				var p = points[i];
				if (p.x < min.x)
					min.x = p.x;
				if (p.y < min.y)
					min.y = p.y;
				if (p.z < min.z)
					min.z = p.z;

				if (p.x > max.x)
					max.x = p.x;
				if (p.y > max.y)
					max.y = p.y;
				if (p.z > max.z)
					max.z = p.z;
			}
			size.x = (-min.x + max.x);
			size.y = (-min.y + max.y);
			size.z = (-min.z + max.z);
		}

		public override string ToString()
		{
			return string.Format("Center({0:F}, {1:F}, {2:F}), Extends({3:F}, {4:F}, {5:F}), degreey:{6:F}",
				center.x, center.y, center.z, size.x / 2, size.y / 2, size.z / 2, degreeY);
		}

		#if UNITY_EDITOR
		public void Draw()
		{
			Gizmos.DrawLine(pointsDraw[0], pointsDraw[1]);
			Gizmos.DrawLine(pointsDraw[1], pointsDraw[2]);
			Gizmos.DrawLine(pointsDraw[2], pointsDraw[3]);
			Gizmos.DrawLine(pointsDraw[3], pointsDraw[0]);

			Gizmos.DrawLine(pointsDraw[0], pointsDraw[4]);
			Gizmos.DrawLine(pointsDraw[1], pointsDraw[5]);
			Gizmos.DrawLine(pointsDraw[2], pointsDraw[6]);
			Gizmos.DrawLine(pointsDraw[3], pointsDraw[7]);

			Gizmos.DrawLine(pointsDraw[4], pointsDraw[5]);
			Gizmos.DrawLine(pointsDraw[5], pointsDraw[6]);
			Gizmos.DrawLine(pointsDraw[6], pointsDraw[7]);
			Gizmos.DrawLine(pointsDraw[7], pointsDraw[4]);
		}
		#endif
	}
}



