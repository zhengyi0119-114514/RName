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
        Random m_random = new();   
        MainWindowDateBinding binding;
        public MainWindow()
        {
            
            InitializeComponent();
            binding = new MainWindowDateBinding();
            binding.LoadFromXmlConfigFile("./Date/app.cfg");
            binding.InitLanguageList();
            _initLanguage($"./Date/Lang/{binding.DefaultLanguage}.xml");
            this.DataContext = binding;
            _SetTextCN();
        }
        private void _initLanguage(String cfgFilePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(cfgFilePath);
            var root = xmlDoc.DocumentElement!;
            XmlElement info = (XmlElement)root.FirstChild!;
            XmlElement sub = (XmlElement)root.LastChild!;
            foreach (XmlElement subElement in sub.ChildNodes)
            {
                switch (subElement.Name)
                {
                    case nameof(BallotButton):
                        BallotButton.Content = subElement.InnerText;
                        break;
                    case nameof(ClearButton):
                        ClearButton.Content = subElement.InnerText;
                        break;
                    case nameof(binding.ClelbrateFormat):
                        binding.ClelbrateFormat = subElement.InnerText;
                        break;
                    case nameof(binding.ClelbrateNothing):
                        binding.ClelbrateNothing = subElement.InnerText;
                        break;
                }
            }
        }
        private void SettingButtom_Click(object sender, RoutedEventArgs e)
        {
            new Settingwindow(this.binding).ShowDialog();
            _initLanguage(binding.LanguageFile);
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            binding.SaveToXMLFile("./Date/app.cfg");
        }
        private void _SetTextCN()
        {
            ClearButton.Content = "清除";
            BallotButton.Content = "抽签";
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Int32 times = 0;
            StringBuilder sb = new StringBuilder();
            if(binding.IsMultipleChoices)
            {
                times = binding.MultipleChoicesTimes;
            }
            else
            {
                times++;
            }
            string[] strings = binding.NameEnableDictionary.Where((p)=>p.Value).Select((p)=> p.Key).ToArray();
            for (int i = 0; i < times; i++)
            {
                if(!binding.IsRepeat)
                {
                    strings = binding.NameEnableDictionary.Where((p) => p.Value)
                        .Where((p) => !binding.ResultList.Contains(p.Key))
                        .Select((p) => p.Key).ToArray();
                }
                if(strings.Length == 0)
                {
                    if (binding.IsCelebrate)
                    {
                        MessageBox.Show(binding.ClelbrateNothing);
                    }
                    return;
                }
                String r = strings[Math.Abs(m_random.Next()%strings.Length)];
                sb.AppendLine(String.Format(binding.ClelbrateFormat, r));
                binding.ResultList.Add(r);
            }
            ShowBox.Text = sb.ToString();
            if(binding.IsCelebrate)
            {
                new CelebrateBox().ShowDialog(sb.ToString());
            }
        }

        private void ClearButtom_Click(object sender, RoutedEventArgs e)
        {
            binding.ResultList.Clear();
            ShowBox.Text = string.Empty;
        }
    }

}