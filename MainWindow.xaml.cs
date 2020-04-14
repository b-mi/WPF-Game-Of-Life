using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GOL gol;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            init();

        }

        private void init()
        {
            // gol = new GOL(canvas, 100, 100, new BaseRules(new int[] { 2, 3 }, new int[] { 3 })); // Game of life
            //gol = new GOL(canvas, 100, 100, new BaseRules(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, new int[] { 3 })); // chaotic
            // gol = new GOL(canvas, 100, 100, new BaseRules(new int[] { 1 }, new int[] { 1 })); // chaotic
            gol = new GOL(canvas, 70, 70, new BaseRules(new int[] { 1, 2, 3, 4, 5 }, new int[] { 3 })); // labyrint
            // gol = new GOL(canvas, 100, 100, new BaseRules(new int[] { 2, 3 }, new int[] { 3, 6 })); // chaotic
            //gol = new GOL(canvas, 100, 100, new BaseRules(new int[] { 2, 3, 5, 6, 7, 8 }, new int[] { 3, 6, 7, 8 })); // chaotic
            //gol = new GOL(canvas, 50, 50, new BaseRules(new int[] { 2, 3, 5, 6, 7, 8 }, new int[] { 3, 6, 7, 8 })); // chaotic
//            gol = new GOL(canvas, 50, 50, new BaseRules(new int[] { 2, 3, 5, 6, 7, 8 }, new int[] { 3, 7, 8 })); // chaotic
            //gol = new GOL(canvas, 50, 50, new BaseRules(new int[] { 3, 4, 6, 7, 8 }, new int[] { 3, 6, 7, 8 })); // chaotic
            //gol = new GOL(canvas, 50, 50, new BaseRules(new int[] { 4, 5, 6, 7, 8 }, new int[] { 3 })); // chaotic
            //gol = new GOL(canvas, 50, 50, new BaseRules(new int[] { 5 }, new int[] { 3, 4, 5 })); // chaotic
            //gol = new GOL(canvas, 50, 50, new BaseRules(new int[] { 5, 3, 7, 8 }, new int[] { 3, 5, 6, 7, 8 })); // chaotic
            // gol = new GOL(canvas, 70, 70, new BaseRules(new int[] { 0, 1, 2, 3 }, new int[] { 3, 9 })); // chaotic
            gol.AddPattern0();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            init();
            gol.Start();
        }
    }
}
