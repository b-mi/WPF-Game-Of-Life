using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{

    public enum CellState { Dead, Live };
    public class GOL
    {
        Canvas canvas;
        int width, height;
        double rctWidth, rctHeight;
        Brush liveBrush = Brushes.Orange;
        Brush deadBrush = Brushes.WhiteSmoke;
        DispatcherTimer timer;
        Func<bool> isStopped;
        Action wasStopped;

        public IRules Rules { get; private set; }

        public GOLCell[,] Cells { get; private set; }
        List<GOLCell> lstCells;

        public GOL(Canvas canvas, int width, int height,IRules rules, Func<bool> isStopped, Action wasStopped)
        {
            this.isStopped = isStopped;
            this.wasStopped = wasStopped;
            this.Rules = rules;
            this.canvas = canvas;
            this.width = width;
            this.height = height;
            canvas.Children.Clear();
            rctWidth = canvas.ActualWidth / width;
            rctHeight = canvas.ActualHeight / height;

            rctWidth = rctHeight = Math.Min(rctWidth, rctHeight);

            Cells = new GOLCell[width, height];
            lstCells = new List<GOLCell>();
            rules.SetCells(Cells);
            // create cells
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var cell = new GOLCell(x, y);
                    lstCells.Add(cell);
                    Cells[x, y] = cell;
                    cell.Left = x * rctWidth;
                    cell.Top = y * rctHeight;
                    cell.Rect = new Rectangle
                    {
                        Width = rctWidth,
                        Height = rctHeight,
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 0.6
                    };
                    //cell.Rect.ToolTip = "0";
                    cell.Rect.Tag = cell;
                    Canvas.SetLeft(cell.Rect, cell.Left);
                    Canvas.SetTop(cell.Rect, cell.Top);
                    canvas.Children.Add(cell.Rect);
                    SetState(cell, CellState.Dead);
                }
            }

            // fill around cells
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var cell = Cells[x, y];

                    for (int ay = -1; ay <= 1; ay++)
                    {
                        for (int ax = -1; ax <= 1; ax++)
                        {
                            if (ay == 0 && ax == 0) continue; // itself

                            var ary = y + ay;
                            var arx = x + ax;
                            if (!isValid(arx, ary)) continue;
                            cell.AroundCells.Add(Cells[arx, ary]);
                        }
                    }

                }
            }

        }



        internal void Start()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            timer.Tick += (o, e) =>
            {
                timer.Stop();
                UpdateLife();
                if (!isStopped())
                {
                    timer.Start();
                } else
                {
                    wasStopped();
                }
            };
            timer.Start();
        }

        internal void UpdateLife()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var cell = Cells[x, y];
                    var newState = Rules.GetNewState(cell);
                    if (cell.State != newState)
                    {
                        SetState(cell, newState);
                    }
                }
            }

            RefreshCellsInfo();
        }

        internal void RefreshCellsInfo()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var cell = Cells[x, y];
                    Rules.GetInfo(cell, out var liveCount);
                    cell.LiveCount = liveCount;
                    // cell.Rect.ToolTip = $"x: {x}, y: {y}, live: {liveCount}";
                }
            }
        }

        public void AddPattern0()
        {
            int x = (int)(width / 2);
            int y = (int)(height / 2);

            for (int xx = 0; xx < 11; xx++)
            {
                SetState(Cells[x + xx, y + xx], CellState.Live);
                SetState(Cells[x + xx - 1, y + xx], CellState.Live);
                SetState(Cells[x + xx + 2, y + xx], CellState.Live);


                SetState(Cells[x + xx, y - xx], CellState.Live);
                SetState(Cells[x + xx - 1, y - xx], CellState.Live);
                SetState(Cells[x + xx + 2, y - xx], CellState.Live);
            }

            RefreshCellsInfo();
        }


        public void AddPattern1()
        {
            int x = (int)(width / 2);
            int y = (int)(height / 2);
            SetState(Cells[x + 2, y], CellState.Live);

            y++;
            SetState(Cells[x, y], CellState.Live);
            SetState(Cells[x + 3, y], CellState.Live);

            y++;
            SetState(Cells[x, y], CellState.Live);
            SetState(Cells[x + 3, y], CellState.Live);

            y++;
            SetState(Cells[x + 1, y], CellState.Live);

            RefreshCellsInfo();
        }

        public void AddPattern2()
        {
            int x = (int)(width / 2);
            int y = (int)(height / 2);
            SetState(Cells[x, y], CellState.Live);
            SetState(Cells[x, y + 1], CellState.Live);
            SetState(Cells[x, y + 2], CellState.Live);
            SetState(Cells[x - 1, y + 2], CellState.Live);
            SetState(Cells[x - 2, y + 1], CellState.Live);
            RefreshCellsInfo();
        }

        public void AddPattern3()
        {
            int x = (int)(width / 2);
            int y = (int)(height / 2);
            SetState(Cells[x, y], CellState.Live);
            SetState(Cells[x, y + 1], CellState.Live);
            SetState(Cells[x, y + 2], CellState.Live);
            SetState(Cells[x - 1, y + 1], CellState.Live);
            SetState(Cells[x + 1, y], CellState.Live);
            RefreshCellsInfo();
        }




        internal void SetState(GOLCell cell, CellState state)
        {
            cell.State = state;
            switch (state)
            {
                case CellState.Live:
                    cell.Rect.Fill = liveBrush;
                    break;
                case CellState.Dead:
                    cell.Rect.Fill = deadBrush;
                    break;
                default:
                    break;
            }
        }

        private bool isValid(int arx, int ary)
        {
            if (arx < 0 || ary < 0) return false;
            if (arx >= width || ary >= height) return false;
            return true;
        }
    }


    public class GOLCell
    {
        public List<GOLCell> AroundCells { get; private set; }
        public double Left { get; internal set; }
        public double Top { get; internal set; }
        public Rectangle Rect { get; internal set; }
        public CellState State { get; internal set; }
        public int LiveCount { get; internal set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public GOLCell(int x, int y)
        {
            X = x;
            Y = y;
            AroundCells = new List<GOLCell>();
        }
    }
}
