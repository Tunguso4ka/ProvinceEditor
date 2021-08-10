using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("Started!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button ClickedButton = (Button)sender;
            switch ((string)ClickedButton.Tag)
            {
                case "NewProvince":
                    new NewProvinceWindow().Show();
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
            Rectangle rectangle = new Rectangle();
            rectangle.Width = 0.5;
            rectangle.Height = 0.5;
            rectangle.Fill = new SolidColorBrush(_SquarePicker.SelectedColor);
            rectangle.Margin = new Thickness(posNow.X, posNow.Y, 0, 0);
            rectangle.Tag = Convert.ToString(RectangleNum);
            MapCanvas.Children.Add(rectangle);
            Debug.WriteLine("Rectangle created in pos " + posNow.X + " " + posNow.Y + " with Tag " + RectangleNum);
            RectangleNum++;
        }
    }
}
