using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
namespace GOL
{
	public class World : MonoBehaviour
	{
		Cell[] cells;
		Texture2D worldView;
		Random randGen;
		float tickTimer;

		[SerializeField]
		int worldSize;
		[SerializeField]
		string worldSeed;
		[SerializeField]
		RawImage worldViewImage;

		[SerializeField]
		float tickDelay;

		private readonly Vector2Int[] directions = new Vector2Int[8]
		{
			Vector2Int.left,
			Vector2Int.right,
			Vector2Int.up,
			Vector2Int.down,
			Vector2Int.up+Vector2Int.left,
			Vector2Int.up+Vector2Int.right,
			Vector2Int.down+Vector2Int.left,
			Vector2Int.down+Vector2Int.right,
		};
		private void Start()
		{
			randGen = new Random(worldSeed.GetHashCode());
			cells = new Cell[worldSize * worldSize];
			worldView = new Texture2D(worldSize, worldSize);
			worldView.SetPixels32(new Color32[worldSize * worldSize]);
			worldView.filterMode = FilterMode.Point;
			for (int x = 0, count = 0; x < worldSize; x++)
			{
				for (int y = 0; y < worldSize; y++)
				{
					cells[count++] = new Cell(x, y, GetRandomState(x, y));
				}
			}
			UpdateView();
			tickTimer = tickDelay;
		}

		private CellState GetRandomState(float x, float y)
		{
			return randGen.Next(1, 100) > 50 ? CellState.LIVE : CellState.DEAD;
		}


		void Update()
		{
			if (tickTimer > 0)
			{
				tickTimer -= Time.deltaTime;
			}
			else
			{
				UpdateLogic();
				UpdateView();
				tickTimer = tickDelay;
			}
		}

		private void UpdateLogic()
		{
			for (int x = 0, count = 0; x < worldSize; x++)
			{
				for (int y = 0; y < worldSize; y++)
				{
					int currentCell = count++;
					int neighbourCount = GetLivingNeighbourCount(cells[currentCell]);
					if (neighbourCount < 2)
					{
						cells[currentCell].nextState = CellState.DEAD;
					}
					else if (neighbourCount > 1 && neighbourCount < 4)
					{
						if (cells[currentCell].currentState == CellState.LIVE)
							cells[currentCell].nextState = CellState.LIVE;
						else if (neighbourCount == 3)
							cells[currentCell].nextState = CellState.LIVE;
					}
					else
					{
						cells[currentCell].nextState = CellState.DEAD;
					}
				}
			}
		}

		private int GetLivingNeighbourCount(Cell cell)
		{
			int count = 0;
			for (int i = 0; i < directions.Length; i++)
			{
				int neighbourIndex = GetNeighborIndex(cell, directions[i]);
				if (neighbourIndex != -1)
				{
					if (cells[neighbourIndex].currentState == CellState.LIVE)
					{
						count++;
					}
				}
			}
			return count;
		}

		private int GetCellIndexFromPos(int x, int y)
		{
			if (x < 0 || y < 0)
			{
				return -1;
			}
			if (x > worldSize - 1 || y > worldSize - 1)
			{
				return -1;
			}
			var index = (x * worldSize) + y;
			return index;
		}

		public int GetNeighborIndex(Cell cell, Vector2Int direction)
		{
			var index = -1;
			int newX = (int)cell.pos.x + direction.x;
			int newY = (int)cell.pos.y + direction.y;

			if (newX < 0 || newY < 0)
			{
				return index;
			}
			if (newX > worldSize - 1 || newY > worldSize - 1)
			{
				return index;
			}
			index = (int)((newX) * worldSize) + (int)(newY);
			return index;
		}

		private void UpdateView()
		{
			for (int x = 0, count = 0; x < worldSize; x++)
			{
				for (int y = 0; y < worldSize; y++)
				{
					if (cells[count].nextState == CellState.LIVE)
					{
						worldView.SetPixel(x, y, Color.white);
					}
					else
					{
						worldView.SetPixel(x, y, Color.clear);
					}
					cells[count].currentState = cells[count].nextState;
					count++;
				}
			}
			worldView.Apply();
			worldViewImage.texture = worldView;
		}
	}
}

