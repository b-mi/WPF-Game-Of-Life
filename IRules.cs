namespace GameOfLife
{
    public interface IRules
    {
        void SetCells(GOLCell[,] cells);
        CellState GetNewState(GOLCell cell);
        void GetInfo(GOLCell cell, out int liveCount);
    }
}