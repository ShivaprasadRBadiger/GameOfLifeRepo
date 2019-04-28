using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOL
{
	public struct Cell
	{
		public Vector3 pos { get; }
		public CellState currentState;
		public CellState nextState;
		public Cell(int x, int y, CellState state)
		{
			pos = new Vector3(x, y, 0);
			currentState = state;
			nextState = state;
		}
	}
}

