namespace GameOfLife
{
    public interface IRules
    {
        void SetCells(GOLCell[,] cells);
        CellState GetNewState(GOLCell cell, int x, int y);
        void GetInfo(GOLCell cell, out int liveCount);
    }
}