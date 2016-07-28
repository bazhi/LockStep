using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

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
		[SerializeField, FormerlySerializedAs("Shape")]
		private BoundingType _shape = BoundingType.AABox;

		public BoundingType Shape { get { return _shape; } }

		[SerializeField,FixedNumber, FormerlySerializedAs("Radius")]
		private long _radius = FixedMath.Half;

		[SerializeField,FixedNumber, FormerlySerializedAs("HalfWidth")]
		private long _halfWidth = FixedMath.Half;

		public long HalfWidth { get { return _halfWidth; } }

		[SerializeField,FixedNumber, FormerlySerializedAs("HalfHeight")]
		public long _halfHeight = FixedMath.Half;

		public long Radius { get { return _radius; } }

		[SerializeField, FormerlySerializedAs("Vertices")]
		private Vector2d[] _vertices;

		public Vector2d[] Vertices { get { return _vertices; } }

		public bool Movable;

		public long XMin { get; private set; }

		public long XMax { get; private set; }

		public long YMin { get; private set; }

		public long YMax { get; private set; }

		[SerializeField] //For inspector debugging
        internal Vector2d _position;

		public void Initialize()
		{
			_position = new Vector2d(transform.position.x, transform.position.z);
			BuildBounds();
		}

		long GetCeiledSnap(long f, long snap)
		{
			return (f + snap - 1) / snap * snap;
		}

		long GetFlooredSnap(long f, long snap)
		{
			return (f / snap) * snap;
		}

		public void BuildBounds()
		{
			if (Shape == BoundingType.Circle) {
				XMin = -Radius + _position.x;
				XMax = Radius + _position.x;
				YMin = -Radius + _position.y;
				YMax = Radius + _position.y;
			} else if (Shape == BoundingType.AABox) {

			} else if (Shape == BoundingType.Polygon) {

			}
		}

		public void GetCoveredSnappedPositions(long snapSpacing, FastList<Vector2d> output)
		{
			long xmin = GetFlooredSnap(this.XMin - FixedMath.Half, snapSpacing);
			long ymin = GetFlooredSnap(this.YMin - FixedMath.Half, snapSpacing);

			long xmax = GetCeiledSnap(this.XMax + FixedMath.Half - xmin, snapSpacing) + xmin;
			long ymax = GetCeiledSnap(this.YMax + FixedMath.Half - ymin, snapSpacing) + ymin;
			//Used for getting snapped positions this body covered
			for (long x = xmin; x < xmax; x += snapSpacing) {
				for (long y = ymin; y < ymax; y += snapSpacing) {
					Vector2d checkPos = new Vector2d(x, y);
					if (IsPositionCovered(checkPos)) {
						output.Add(checkPos);
					}
				}
			}
		}

		public bool IsPositionCovered(Vector2d position)
		{
			switch (this.Shape) {
				case BoundingType.Circle:
					long maxDistance = this.Radius + FixedMath.Half;
					maxDistance *= maxDistance;
					if ((_position - position).FastMagnitude() > maxDistance)
						return false;
					goto case BoundingType.AABox;
				case BoundingType.AABox:
					return position.x + FixedMath.Half >= this.XMin && position.x - FixedMath.Half <= this.XMax
					&& position.y + FixedMath.Half >= this.YMin && position.y - FixedMath.Half <= this.YMax;
					break;
				case BoundingType.Polygon:
					break;
			}


			return false;
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			switch (this.Shape) {
				case BoundingType.Circle:
					Gizmos.DrawWireSphere(this.transform.position, this.Radius.ToFloat());
					break;
				case BoundingType.AABox:
					Gizmos.DrawWireSphere(this.transform.position, this.Radius.ToFloat());
					break;
				case BoundingType.Polygon:
					Gizmos.DrawWireSphere(this.transform.position, this.Radius.ToFloat());
					break;
			}
		}
	}
}

