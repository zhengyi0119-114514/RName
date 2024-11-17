using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Xml;

namespace RandUIApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowDateBinding binding;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowDateBinding();
            binding = (MainWindowDateBinding)this.DataContext;
            //binding.ResultList.Add("TEXT");
            _SetTextCN();
        }

        private void SettingButtom_Click(object sender, RoutedEventArgs e)
        {
            new Settingwindow(this.DataContext).ShowDialog();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MainWindowDateBinding? m = this.DataContext as MainWindowDateBinding;
            m?.SaveAsXMLConfigFile("./Date/app.cfg");
        }
        private void _SetTextCN()
        {
            ClearButtom.Content = "清除";
            ToggleButton.Content = "切换";
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            binding.ResultList.Clear();
            Random random = new Random(new Random().Next());
            Int32 times = 1;
            ObservableCollection<String> enble = new ObservableCollection<String>();
            if(binding.IsRepeat)
            {
                times = binding.RepeatTimes;
            }
            if (times <1)
            {
                times = 1;
            }
            foreach(String s in binding.NameList)
            {
                if (!binding.DisableList.Contains(s))
                {
                    enble.Add(s);
                }
            }
            if((!binding.IsRepeat)&&(times>enble.Count))
            {
                binding.ResultList = enble;
            }
            string? item = "";
            for (int i = 0; i < times; i++)
            {
                item = enble[random.Next(enble.Count)];
                if(!binding.IsRepeat)
                {
                    enble.Remove(item);
                }
                binding.ResultList.Add(item);
            }
            if (times == 1)
            {
                ShowBox.Text = $"恭喜幸运的{binding.ResultList[0]}中dai奖";
            }

        }

        private void ClearButtom_Click(object sender, RoutedEventArgs e)
        {
            binding.ResultList.Clear();
        }
    }

    public partial class MainWindowDateBinding : INotifyPropertyChanged
    {

    }
}