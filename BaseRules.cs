using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    public class BaseRules : IRules
    {
        GOLCell[,] cells;
        private HashSet<int> survive;
        private HashSet<int> reborn;

        public BaseRules(int[] survive, int[] reborn)
        {
            this.survive = new HashSet<int>(survive);
            this.reborn = new HashSet<int>(reborn);

        }

        public void GetInfo(GOLCell cell, out int liveCount)
        {
            liveCount = 0;
            foreach (var item in cell.AroundCells)
            {
                if (item.State == CellState.Live)
                    liveCount++;
            }

        }

        public CellState GetNewState(GOLCell cell)
        {
            if (cell.State == CellState.Live)
            {
                // 1. less than 2 live neighbours = Dead
                // 2. 2 or 3 liveneighbours = Live
                // 3. more than 3 live neighbours = Dead

                if (survive.Contains(cell.LiveCount))
                {
                    return CellState.Live;
                }
                else
                {
                    return CellState.Dead;
                }
            }
            else
            {
                // 4. 3 lives around dead - live
                if (reborn.Contains(cell.LiveCount))
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
