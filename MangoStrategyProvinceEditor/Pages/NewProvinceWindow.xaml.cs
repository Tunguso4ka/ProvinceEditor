using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MangoStrategyProvinceEditor
{
    /// <summary>
    /// Логика взаимодействия для NewProvinceWindow.xaml
    /// </summary>
    public partial class NewProvinceWindow : Window
    {
        public NewProvinceWindow()
        {
            InitializeComponent();
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
                case "CreateNew":
                    //mainwindow.CreateNew();
                    this.Close();
                    break;
                case "Close":
                    this.Close();
                    break;
            }
        }
    }
}
