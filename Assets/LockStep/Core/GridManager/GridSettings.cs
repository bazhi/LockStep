using UnityEngine;
using System.Collections;

namespace Lockstep
{
	public sealed class GridSettings
	{
		public int Width { get; private set; }

		public int Height { get; private set; }

		public long XOffset { get; private set; }

		public long YOffset { get; private set; }

		public bool UseDiagonalConnections { get; private set; }

		public GridSettings()
		{
			Init(256, 256, FixedMath.Create(-128), FixedMath.Create(-128), true);
		}

		public GridSettings(int width, int height, long xOffset, long yOffset, bool useDiagonalConnections)
		{
			Init(width, height, xOffset, yOffset, useDiagonalConnections);
		}

		private void Init(int width, int height, long xOffset, long yOffset, bool useDiagonalConnections)
		{
			this.Width = width;
			this.Height = height;
			this.XOffset = xOffset;
			this.YOffset = yOffset;
			this.UseDiagonalConnections = useDiagonalConnections;

			Debug.LogFormat("Width:{0:D}, Height:{1:D}, xOffset:{2:D}, yOffset:{3:D}", width, height, xOffset, yOffset);
			//Debug.LogFormat("xOffset normalized:{0:D}", xOffset.Normalized());
		}
	}
}