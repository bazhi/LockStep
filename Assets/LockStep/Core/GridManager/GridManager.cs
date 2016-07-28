using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lockstep
{
	public static class GridManager
	{
		public const int DefaultCapacity = 64 * 64;

		public static long Spacing = FixedMath.One;

		public static int Width { get; private set; }

		public static int Height { get; private set; }

		public static int GridSize { get; private set; }

		public static GridNode[] Grid;

		public static long OffsetX { get; private set; }

		public static long OffsetY { get; private set; }
		//4个角能否走标志
		public static bool UseDiagonalConnections = true;

		private static bool _settingsChanged = true;
		public static readonly GridSettings DefaultSettings = new GridSettings();

		private static GridSettings _settings;

		public static GridSettings Settings {
			get {
				return _settings;
			}
			set {
				_settings = value;
				Debug.Log("GridSettings Changed");
				_settingsChanged = true;
			}
		}

		private static Stack<GridNode> CachedGridNodes = new Stack<GridNode>(GridManager.DefaultCapacity);

		static void ApplySettings()
		{
			Width = Settings.Width;
			Height = Settings.Height;
			GridSize = Width * Height;
			OffsetX = Settings.XOffset;
			OffsetY = Settings.YOffset;

			UseDiagonalConnections = Settings.UseDiagonalConnections;
		}

		private static void Generate()
		{
			if (Grid != null) {
				int min = Grid.Length;
				for (int i = min - 1; i >= 0; --i) {
					CachedGridNodes.Push(Grid[i]);
				}
			}

			Grid = new GridNode[GridSize];
			for (int i = Width - 1; i >= 0; --i) {
				for (int j = Height - 1; j >= 0; --j) {
					var node = CachedGridNodes.Count > 0 ? CachedGridNodes.Pop() : new GridNode();
					node.Setup(i, j);
					Grid[GetGridIndex(i, j)] = node;
				}
			}
		}

		public static void Initialize()
		{
			if (_settingsChanged) {
				if (_settings == null)
					_settings = DefaultSettings;
				ApplySettings();
				Generate();

				for (int i = GridSize - 1; i >= 0; i--) {
					Grid[i].Initialize();
				}
			} else {
				//If we're using the same settings, no need to generate a new grid or neighbors
				for (int i = GridSize - 1; i >= 0; i--) {
					Grid[i].FastInitialize();
				}
			}
		}

		public static bool ValidateCoordinates(int xGrid, int yGrid)
		{
			return xGrid >= 0 && xGrid < Width && yGrid >= 0 && yGrid < Height;
		}

		public static bool ValidateIndex(int index)
		{
			return index >= 0 && index < GridSize;
		}

		public static int GetGridIndex(int xGrid, int yGrid)
		{
			return xGrid * Height + yGrid;
		}

		public static GridNode GetNode(int xGrid, int yGrid)
		{
			return Grid[GetGridIndex(xGrid, yGrid)];
		}

		static int indexX;
		static int indexY;

		public static GridNode GetNode(long xPos, long yPos)
		{
			GetCoordinates(xPos, yPos, out indexX, out indexY);
			return !ValidateCoordinates(indexX, indexY) ? null : (GetNode(indexX, indexY));
		}

		public static void GetCoordinates(long xPos, long yPos, out int xGrid, out int yGrid)
		{
			xGrid = (int)((xPos + FixedMath.Half - 1 - OffsetX) >> FixedMath.SHIFT_AMOUNT);
			yGrid = (int)((yPos + FixedMath.Half - 1 - OffsetY) >> FixedMath.SHIFT_AMOUNT);
		}

		public static int ToGridX(this long xPos)
		{
			return (xPos - OffsetX).RoundToInt();
		}

		public static int ToGridY(this long yPos)
		{
			return (yPos - OffsetY).RoundToInt();
		}
	}
}