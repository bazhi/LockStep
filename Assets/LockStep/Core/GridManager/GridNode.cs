using UnityEngine;
using System.Collections;
using System;

namespace Lockstep
{
	public class GridNode
	{
		public uint ClosedSetVersion;
		public uint HeapVersion;
		public uint HeapIndex;

		public int gridX;
		public int gridY;
		public int gridIndex;

		public int gCost;
		public int hCost;
		public int fCost;

		public GridNode parent;

		public bool Unwalkable { get; private set; }

		public int ObstacleCount { get; private set; }

		public Vector2d WorldPos;
		public GridNode[] NeighborNodes = new GridNode[8];

		public GridNode()
		{

		}

		public void Initialize()
		{

			this.FastInitialize();

			GenerateNeighbors();
		}

		public void FastInitialize()
		{
			this.ClosedSetVersion = 0;
			this.HeapIndex = 0;
			this.HeapVersion = 0;
		}

		public void Setup(int _x, int _y)
		{
			gridX = _x;
			gridY = _y;
			gridIndex = GridManager.GetGridIndex(gridX, gridY);
			WorldPos.x = (long)(gridX * FixedMath.One + GridManager.OffsetX);
			WorldPos.y = (long)(gridY * FixedMath.One + GridManager.OffsetY);
		}

		public void UpdateUnwalkable()
		{
			Unwalkable = ObstacleCount > 0;
		}

		public void AddObstacle()
		{
			++ObstacleCount;
			UpdateUnwalkable();
		}

		public void RemoveObstacle()
		{
			--ObstacleCount;
			UpdateUnwalkable();
		}

		static int CachedSize;

		public static void PrepareUnpassableCheck(int size)
		{
			CachedSize = size;
		}

		public bool Unpassable()
		{
			if (Unwalkable)
				return true;
			if (CachedSize <= 2) {
				return false;
			}
			if (CachedSize <= 3) {
				return UnpassableMedium();
			} else {
				return UnpassableLarge();
			}
		}

		GridNode _node;
		static int _i;

		public bool UnpassableMedium()
		{
			for (_i = 0; _i < 8; _i++) {
				_node = NeighborNodes[_i];
				if (_node != null)
				if (_node.Unwalkable)
					return true;
			}
			return false;
		}

		public bool UnpassableLarge()
		{
			int half = (CachedSize + 1) / 2;
			for (int x = -half; x <= half; x++) {
				for (int y = -half; y <= half; y++) {
					int index = GridManager.GetGridIndex(gridX + x, gridY + y);
					if (GridManager.ValidateIndex(index)) {
						if (GridManager.Grid[index].Unwalkable) {
							return true;
						}
					}
				}
			}
			return false;
		}

		private void GenerateNeighbors()
		{
			//0-3 = sides, 4-7 = diagonals

			int sideIndex = 0;
			int diagonalIndex = 4; //I learned how to spell [s]diagnal[/s] diagonal!!!
			int i = 0, j = 0, checkX = 0, checkY = 0, neighborIndex = 0;

			for (i = -1; i <= 1; i++) {
				checkX = gridX + i;

				for (j = -1; j <= 1; j++) {
					if (i == 0 && j == 0) //Don't do anything for the same node
                        continue;
					checkY = gridY + j;
					if (GridManager.ValidateCoordinates(checkX, checkY)) {
						if ((i != 0 && j != 0)) {
							//Diagonal
							if (GridManager.UseDiagonalConnections) {
								neighborIndex = diagonalIndex++;
							} else
								continue;
						} else {
							neighborIndex = sideIndex++;
						}
						var checkNode = GridManager.Grid[GridManager.GetGridIndex(checkX, checkY)];
						NeighborNodes[neighborIndex] = checkNode;
					}
				}
			}
		}

		static int dstX;
		static int dstY;
		public static int HeuristicTargetX;
		public static int HeuristicTargetY;

		public void CalculateHeuristic()
		{
            
			//Euclidian
			dstX = HeuristicTargetX - gridX;
			dstY = HeuristicTargetY - gridY;
			hCost = (dstX * dstX + dstY * dstY);

			fCost = gCost + hCost;
		}

		public override int GetHashCode()
		{
			return gridIndex;
		}

		public bool DoesEqual(GridNode obj)
		{
			return obj.gridIndex == this.gridIndex;
		}

		public override string ToString()
		{
			return "(" + gridX.ToString() + ", " + gridY.ToString() + ")";
		}
	}
}