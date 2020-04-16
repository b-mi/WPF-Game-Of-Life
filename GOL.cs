using System;
using System.Collections.Generic;
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
        Dictionary<string, GOLCell> dctCellsToRecalc;

        public bool ToroidalMove { get; private set; } = true;

        public GOL(Canvas canvas, int width, int height, string pattern, IRules rules, Func<bool> isStopped, Action wasStopped)
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
                            if (!isValidNeighbour(arx, ary, out var oarx, out var oary)) continue;
                            cell.AroundCells.Add(Cells[oarx, oary]);
                        }
                    }

                }
            }
            switch (pattern)
            {
                case "Test1":
                    AddPatternTest1();
                    break;
                case "Test2":
                    AddPatternTest2();
                    break;
                case "Test3":
                    AddPatternTest3();
                    break;
                case "Test4":
                    AddPatternTest4();
                    break;
                default:
                    break;
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
                }
                else
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
                    SetState(cell, newState);
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
                }
            }
        }

        public void AddPatternTest1()
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


        public void AddPatternTest2()
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

        public void AddPatternTest3()
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

        public void AddPatternTest4()
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
            if (cell.State != state)
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
        }

        private bool isValidNeighbour(int inputX, int inputY, out int outX, out int outY)
        {
            if (ToroidalMove)
            {
                outX = inputX;
                outY = inputY;
                if (inputX == -1) outX = width - 1;
                if (inputY == -1) outY = height - 1;
                if (inputX == width) outX = 0;
                if (inputY == width) outY = 0;
                return true;
            }
            else
            {
                outX = inputX;
                outY = inputY;
                if (inputX < 0 || inputY < 0) return false;
                if (inputX >= width || inputY >= height) return false;
                return true;
            }
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
        public string Key { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public GOLCell(int x, int y)
        {
            this.Key = $"{x}|{y}";
            X = x;
            Y = y;
            AroundCells = new List<GOLCell>();
        }
    }
}
