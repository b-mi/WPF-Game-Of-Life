using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    public class BaseRules : IRules
    {
        GOLCell[,] cells;

        public void GetInfo(GOLCell cell, out int liveCount)
        {
            liveCount = 0;
            foreach (var item in cell.AroundCells)
            {
                if (item.State == CellState.Live)
                    liveCount++;
            }

        }

        public CellState GetNewState(GOLCell cell, int x, int y)
        {
            if (cell.State == CellState.Live)
            {
                // 1. less than 2 live neighbours = Dead
                // 2. 2 or 3 liveneighbours = Live
                // 3. more than 3 live neighbours = Dead

                if (cell.LiveCount < 2 || cell.LiveCount > 3)
                {
                    return CellState.Dead;
                }
                else
                {
                    return CellState.Live;
                }
            }
            else
            {
                // 4. 3 lives around dead - live
                if (cell.LiveCount == 3)
                {
                    return CellState.Live;
                }
            }
            return cell.State;
        }

        public void SetCells(GOLCell[,] cells)
        {
            this.cells = cells;
        }
    }
}
