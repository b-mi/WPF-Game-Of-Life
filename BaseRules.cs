using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    public class BaseRules : IRules
    {
        GOLCell[,] cells;
        private HashSet<int> survive;
        private HashSet<int> reborn;

        public BaseRules(string cfg)
        {
            var parts = cfg.Split('/');
            var strSurvive = parts[0].ToCharArray();
            var strReborn = parts[1].ToCharArray();
            var iaSurvive = strSurvive.Select(r => int.Parse(r.ToString()));
            var iaReborn = strReborn.Select(r => int.Parse(r.ToString()));

            this.survive = new HashSet<int>(iaSurvive);
            this.reborn = new HashSet<int>(iaReborn);

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
            if (cell.State == CellState.Live) // calculate state for live cell
            {
                if (survive.Contains(cell.LiveCount))
                {
                    return CellState.Live;
                }
                else
                {
                    return CellState.Dead;
                }
            }
            else // calculate state for dead cell
            {
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
