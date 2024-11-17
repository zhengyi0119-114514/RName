using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RandUIApp
{
    /// <summary>
    /// Settingwindow.xaml 的交互逻辑
    /// </summary>
    public partial class Settingwindow : Window
    {
        public Settingwindow()
        {
            InitializeComponent();
            __SetTextCN();
            
        }
        private void __SetTextCN()
        {
            IsRepeatCheckBox.Content = "重复选择";
            MultipleChoicesCheckBox.Content = "多选";
            SelectionTextLabel.Content = "一次选择人数";
            ImportFromTxtButton.Content = "从.TXT文件导入";
            IsCelebrateCheckBox.Content = "幸灾乐祸";
        }
        public Settingwindow(Object date):this()
        {
            this.DataContext = date;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //MainWindowDateBinding binding = (MainWindowDateBinding)this.DataContext;
            //Int32 t = 1;
            //Int32.TryParse(SelectionTimesTextBox.Text, out t);
            //binding.RepeatTimes = t;
            //MessageBox.Show($"{t}");
        }

        private void ImportFromTxtButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowDateBinding binding = ( MainWindowDateBinding)this.DataContext;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "NameFile (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                binding.DisableList.Clear();
                binding.ResultList.Clear();
                binding.NameList.Clear();
                FileStream file = new FileStream(openFileDialog.FileName, FileMode.Open);
                StreamReader reader = new StreamReader(file);
                String? line = "";
                while (line != null)
                {
                    line = reader.ReadLine();
                    if (line != null)
                        binding.NameList.Add(line);
                }
                reader.Close();
                file.Close();
            }
        }
    }
}
