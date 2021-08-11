using System;
using System.Collections.Generic;
using System.Diagnostics;
using IO = System.IO;
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

namespace MangoStrategyProvinceEditor
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Color ProvinceColor;
        int RectangleNum = 0;

        Double LastX = 0;
        Double LastY = 0;

        Double X = 0;
        Double Y = 0;

        IO.StreamWriter sw;

        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("Started!");
        }

        public void CreateNew(string ProvinceFilePath)
        {
            sw = new IO.StreamWriter(ProvinceFilePath);
            ProvinceNameTextBox.Text = ProvinceFilePath;

            LastX = 0;
            LastY = 0;

            X = 0;
            Y = 0;
        }

        void WritePoint(double X, double Y)
        {
            sw.WriteLine(X + " " + Y);
        }

        public void DisposeStreamWriter()
        {
            sw.Close();
            sw.Dispose();
        }

        void MakePath()
        {
            Path _Path = new Path();
            _Path.Stroke = new SolidColorBrush(_SquarePicker.SelectedColor);
            _Path.Fill = new SolidColorBrush(_SquarePicker.SelectedColor);
            _Path.StrokeThickness = 2;

            Geometry _Geometry = Geometry.Parse("M " + Convert.ToString(Convert.ToInt32(LastX)) + "," + Convert.ToString(Convert.ToInt32(LastY)) + " " + Convert.ToString(Convert.ToInt32(X)) + "," + Convert.ToString(Convert.ToInt32(Y)));

            //EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            //myEllipseGeometry.Center = new System.Windows.Point(X, Y);
            //myEllipseGeometry.RadiusX = 25;
            //myEllipseGeometry.RadiusY = 25;
            //_Path.Data = myEllipseGeometry;

            _Path.Data = _Geometry;

            MapCanvas.Children.Add(_Path);

            //Paths.Add(_Path);

            Debug.WriteLine("AddPath " + LastX + " " + LastY + " " + X + " " + Y);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button ClickedButton = (Button)sender;
            switch ((string)ClickedButton.Tag)
            {
                case "NewProvince":
                    new NewProvinceWindow(this).Show();
                    break;
                case "Save":
                    DisposeStreamWriter();
                    break;
                case "Minimize":
                    WindowState = WindowState.Minimized;
                    break;
                case "Close":
                    Application.Current.Shutdown();
                    break;
            }
        }

        private void _SquarePicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            _HexColorTexBox.SelectedColor = _SquarePicker.SelectedColor;
            Debug.WriteLine("Color " + _SquarePicker.SelectedColor);
        }

        private void _HexColorTexBox_ColorChanged(object sender, RoutedEventArgs e)
        {
            _SquarePicker.SelectedColor = _HexColorTexBox.SelectedColor;
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point posNow = e.GetPosition(MapCanvas);

            if(X != 0)
            {
                LastX = X;
                LastY = Y;
            }

            X = posNow.X - 1;
            Y = posNow.Y - 1;

            Rectangle rectangle = new Rectangle();
            rectangle.Width = 2;
            rectangle.Height = 2;
            rectangle.Fill = new SolidColorBrush(_SquarePicker.SelectedColor);
            rectangle.Margin = new Thickness(X, Y, 0, 0);
            rectangle.Tag = Convert.ToString(RectangleNum);
            MapCanvas.Children.Add(rectangle);
            Debug.WriteLine("Rectangle created in pos " + X + " " + Y + " with Tag " + RectangleNum);
            WritePoint(X, Y);

            if(LastX != 0)
            {
                MakePath();
            }

            RectangleNum++;
        }
    }
}
