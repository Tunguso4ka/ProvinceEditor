using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace MangoStrategyProvinceEditor
{
    /// <summary>
    /// Логика взаимодействия для NewProvinceWindow.xaml
    /// </summary>
    public partial class NewProvinceWindow : Window
    {
        MainWindow _MainWindow;

        public NewProvinceWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _MainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button ClickedButton = (Button)sender;
            switch ((string)ClickedButton.Tag)
            {
                case "OpenFolder":
                    Forms.FolderBrowserDialog folderBrowserDialog1 = new Forms.FolderBrowserDialog();

                    if (folderBrowserDialog1.ShowDialog() == Forms.DialogResult.OK)
                    {
                        LocationTextBox.Text = folderBrowserDialog1.SelectedPath;
                    }
                    break;
                case "OpenFile":
                    Forms.OpenFileDialog _OpenFileDialog = new Forms.OpenFileDialog();
                    _OpenFileDialog.InitialDirectory = LocationTextBox.Text;
                    _OpenFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

                    if (_OpenFileDialog.ShowDialog() == Forms.DialogResult.OK)
                    {
                        FileTextBox.Text = _OpenFileDialog.FileName;
                    }
                    break;
                case "CreateNew":
                    _MainWindow.CreateNew(FileTextBox.Text);
                    this.Close();
                    break;
                case "Close":
                    this.Close();
                    break;
            }
        }
    }
}
