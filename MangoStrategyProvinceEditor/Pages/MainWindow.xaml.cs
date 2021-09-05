using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Forms = System.Windows.Forms;
using IO = System.IO;

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

        string ProvincesPath;

        IO.StreamWriter sw;

        public MainWindow()
        {
            InitializeComponent();
            Debug.WriteLine("Started!");
            ProvincesPath = Properties.Settings.Default.ProvincePath;
            //this.Cursor = new Cursor(@"");
        }

        public void CreateNew(string ProvinceFilePath)
        {
            sw = new IO.StreamWriter(ProvinceFilePath);
            ProvinceNameTextBox.Text = ProvinceFilePath;

            LastX = 0;
            LastY = 0;

            X = 0;
            Y = 0;

            Save.IsEnabled = true;
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

        void MakePathByCoords(double LastX, double LastY, double X, double Y, Brush PathBrush)
        {
            Path _Path = new Path();
            _Path.Stroke = PathBrush;
            _Path.Fill = PathBrush;
            _Path.StrokeThickness = 2;

            Geometry _Geometry = Geometry.Parse("M " + Convert.ToString(Convert.ToInt32(LastX)) + "," + Convert.ToString(Convert.ToInt32(LastY)) + " " + Convert.ToString(Convert.ToInt32(X)) + "," + Convert.ToString(Convert.ToInt32(Y)));

            _Path.Data = _Geometry;

            MapCanvas.Children.Add(_Path);

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
                    Save.IsEnabled = false;
                    break;
                case "Minimize":
                    WindowState = WindowState.Minimized;
                    break;
                case "Close":
                    Application.Current.Shutdown();
                    break;
                case "ShowMenu":
                    if (MenuStackPanel.Visibility == Visibility.Collapsed)
                        MenuStackPanel.Visibility = Visibility.Visible;
                    else
                        MenuStackPanel.Visibility = Visibility.Collapsed;
                    break;
                case "LoadProvinces":
                    LoadMap();
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
            if(SnapCheckBox.IsChecked == false)
            {
                Point posNow = e.GetPosition(MapCanvas);

                if (X != 0)
                {
                    LastX = X;
                    LastY = Y;
                }

                X = posNow.X - 2;
                Y = posNow.Y - 2;

                Rectangle rectangle = new Rectangle();
                rectangle.Width = 4;
                rectangle.Height = 4;
                rectangle.Fill = new SolidColorBrush(_SquarePicker.SelectedColor);
                rectangle.Margin = new Thickness(X, Y, 0, 0);
                rectangle.Tag = Convert.ToString(RectangleNum);
                rectangle.MouseRightButtonDown += new MouseButtonEventHandler(Rectangle_MouseRightButtonDown);
                MapCanvas.Children.Add(rectangle);
                Debug.WriteLine("Canvas Rectangle created in pos " + X + " " + Y + " with Tag " + RectangleNum);
                RectangleNum++;

                XYSet();
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void LoadMap()
        {
            IO.StreamReader _StreamReader;

            if (IO.File.Exists(ProvincesPath + @"\Provinces.txt"))
            {
                _StreamReader = new IO.StreamReader(ProvincesPath + @"\Provinces.txt");
            }
            else
            {
                Forms.FolderBrowserDialog folderBrowserDialog = new Forms.FolderBrowserDialog();

                if (folderBrowserDialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    ProvincesPath = folderBrowserDialog.SelectedPath;
                }

                _StreamReader = new IO.StreamReader(ProvincesPath + @"\Provinces.txt");

                Properties.Settings.Default.ProvincePath = ProvincesPath;
                Properties.Settings.Default.Save();
            }

            string Numstring;
            while ((Numstring = _StreamReader.ReadLine()) != null)
            {
                LoadProvince(Convert.ToInt32(Numstring));
            }
            _StreamReader.Dispose();
        }

        public void LoadProvince(int ProvinceNum)
        {
            IO.StreamReader _StreamReader = new IO.StreamReader(ProvincesPath + @"\" + ProvinceNum + ".txt");
            string CoordsString;
            string[] Coords;
            while ((CoordsString = _StreamReader.ReadLine()) != null)
            {
                Coords = CoordsString.Split(' ');
                if (Coords.Length == 4)
                {
                    X = Convert.ToDouble(Coords[0]);
                    Y = Convert.ToDouble(Coords[1]);
                    LastX = Convert.ToDouble(Coords[2]);
                    LastY = Convert.ToDouble(Coords[3]);
                    MakePathByCoords(X, Y, LastX, LastY, Brushes.Red);
                }
                else
                {
                    X = Convert.ToDouble(Coords[0]);
                    Y = Convert.ToDouble(Coords[1]);
                    MakePathByCoords(LastX, LastY, X, Y, Brushes.Red);
                    LastX = X;
                    LastY = Y;
                }

                Rectangle rectangle = new Rectangle();
                rectangle.Width = 4;
                rectangle.Height = 4;
                rectangle.Fill = Brushes.DarkRed;
                rectangle.Margin = new Thickness(X, Y, 0, 0);
                rectangle.Tag = Convert.ToString(RectangleNum);
                rectangle.MouseRightButtonDown += new MouseButtonEventHandler(Rectangle_MouseRightButtonDown);
                MapCanvas.Children.Add(rectangle);
                Debug.WriteLine("Rectangle created in pos " + X + " " + Y + " with Tag " + RectangleNum);
                RectangleNum++;
            }
            MakePathByCoords(LastX, LastY, X, Y, Brushes.Red);
            _StreamReader.Dispose();
        }

        private void Rectangle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle ClickedRectangle = (Rectangle)sender;

            if (X != 0)
            {
                LastX = X;
                LastY = Y;
            }

            Y = ClickedRectangle.Margin.Top;
            X = ClickedRectangle.Margin.Left;

            Debug.WriteLine("Rectangle clicked" + X + " " + Y);

            XYSet();
        }

        void XYSet()
        {
            if (LastX != 0)
            {
                MakePath();
            }

            WritePoint(X, Y);
        }
    }
}
