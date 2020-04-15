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
        public int AreaWidth
        {
            get { return (int)GetValue(AreaWidthProperty); }
            set { SetValue(AreaWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AreaWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AreaWidthProperty =
            DependencyProperty.Register("AreaWidth", typeof(int), typeof(MainWindow), new PropertyMetadata(76));


        public int AreaHeight
        {
            get { return (int)GetValue(AreaHeightProperty); }
            set { SetValue(AreaHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AreaHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AreaHeightProperty =
            DependencyProperty.Register("AreaHeight", typeof(int), typeof(MainWindow), new PropertyMetadata(77));

        public string[] Patterns { get => patterns; set => patterns = value; }

        string[] patterns =
        {
            "Test1",
            "Test2",
            "Test3",
            "Test4",
        };

        string[] configs = new string[] {
"/2",
"/234",
"012345678/3",
"1/1",
"12345/3",
"125/36",
"1357/1357",
"1358/357",
"23/3",
"23/36",
"2345/45678",
"235678/3678",
"235678/378",
"238/357",
"245/368",
"34/34",
"34678/3678",
"4567/345",
"45678/3",
"5/345"
            };
        private bool isRun;

        public string[] Configs { get => configs; set => configs = value; }



        public string Pattern
        {
            get { return (string)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pattern.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PatternProperty =
            DependencyProperty.Register("Pattern", typeof(string), typeof(MainWindow), new PropertyMetadata());




        public string Config
        {
            get { return (string)GetValue(ConfigProperty); }
            set { SetValue(ConfigProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Config.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfigProperty =
            DependencyProperty.Register("Config", typeof(string), typeof(MainWindow), new PropertyMetadata());






        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbPat.SelectedIndex = 1;
            cbCon.SelectedIndex = 4;

        }

        private void init()
        {
            var ptrn = this.Pattern;
            var cfg = this.Config;
            var wdth = this.AreaWidth;
            var hgh = this.AreaHeight;
            this.isRun = true;
            gol = new GOL(canvas, wdth, hgh, new BaseRules(cfg), IsStopped, WasStopped);
            switch (ptrn)
            {
                case "Test1":
                    gol.AddPattern0();
                    break;
                case "Test2":
                    gol.AddPattern1();
                    break;
                case "Test3":
                    gol.AddPattern2();
                    break;
                case "Test4":
                    gol.AddPattern3();
                    break;
                default:
                    break;
            }
        }

        bool IsStopped()
        {
            return !isRun;
        }

        public void WasStopped()
        {
            bRun.Content = "Start";
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isRun)
            {
                isRun = false;
            }
            else
            {
                bRun.Content = "Stop";
                init();
                gol.Start();

            }
        }
    }
}
