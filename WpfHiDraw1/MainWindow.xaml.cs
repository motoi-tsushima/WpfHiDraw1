using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/// <summary>
/// WPF描画速度検証用テストプログラム
/// https://www.peliphilo.net/archives/2447
/// を参考にしました。
/// </summary>
namespace WpfHiDraw1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 結果表示用
            xView.ResultLabel = xLabel;
        }

        //public List<Tuple<Point, Point>> points = new List<Tuple<Point, Point>>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.StartButton.IsEnabled = false;

            // 適当なシードで乱数を生成する(直線の始点と終点を適当に算出するのに使用)
            var randSH = new Random(17280489);
            var randSV = new Random(399594);
            var randEH = new Random(8793);
            var randEV = new Random(4498389);
            var points = new List<Tuple<Point, Point>>();
            for (var i = 0; i < 10000; ++i)
            {
                points.Add(new Tuple<Point, Point>(new Point(randSH.Next(800), randSV.Next(450)), new Point(randEH.Next(800), randEV.Next(450))));
            }
            xView.Points = points;

            this.StartButton.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            xView.InvalidateVisual();
        }
    }
}
