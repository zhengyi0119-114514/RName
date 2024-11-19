using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml;
using Newtonsoft.Json.Serialization;

namespace RandUIApp
{
    /// <summary>
    /// Settingwindow.xaml 的交互逻辑
    /// </summary>
    public partial class Settingwindow : Window
    {
        MainWindowDateBinding binding = null!;
        Settingwindow()
        {
            InitializeComponent();
        }
        public Settingwindow(MainWindowDateBinding date):this()
        {
            this.binding = date;
            this.DataContext = date;
            _flushCBoxItems();
            _initLanguage(binding.LanguageFile);
            binding.UpdateNumberList();
        }
        private void _setTextCN()
        {
            IsRepeatCheckBox.Content = "重复选择";
            MultipleChoicesCheckBox.Content = "多选";
            SelectionTextLabel.Content = "一次选择人数";
            IsCelebrateCheckBox.Content = "幸灾乐祸";
            ImportFromTxtButton.Content = "从.TXT文件导入";
        }
        private void _flushCBoxItems()
        {
            var sources = new ObservableCollection<String>();
            foreach(var i in binding.NameEnableDictionary)
            {
                if(i.Value)
                {
                    sources.Add($"[✓]-{i.Key}");
                }
                else
                {
                    sources.Add($"[✕]-{i.Key}");
                }
            }
            binding.NameMgrBindingList = sources;
        }
        private void _initLanguage(string cfgFilePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(cfgFilePath);
            var root = xmlDoc.DocumentElement!;
            XmlElement info = (XmlElement) root.FirstChild!;
            XmlElement sub = (XmlElement) root.LastChild!;
            foreach(XmlElement subElement in sub.ChildNodes)
            {
                switch (subElement.Name)
                {
                    case nameof(IsRepeatCheckBox):
                        IsRepeatCheckBox.Content = subElement.InnerText;
                        break;
                    case nameof(MultipleChoicesCheckBox):
                        MultipleChoicesCheckBox.Content = $"{subElement.InnerText}";
                        break;
                    case nameof(IsCelebrateCheckBox):
                        IsCelebrateCheckBox.Content = subElement.InnerText;
                        break;
                    case nameof(SelectionTextLabel):
                        SelectionTextLabel.Content = subElement.InnerText;
                        break;
                    case nameof(AddButton):
                        AddButton.Content = subElement.InnerText;
                        break;
                    case nameof(DisableNameButton):
                        DisableNameButton.Content = subElement.InnerText;
                        break;
                    case nameof(EnableNameButton):
                        EnableNameButton.Content = subElement.InnerText;
                        break;
                    case nameof(DeleteNumberButton):
                        DeleteNumberButton.Content = subElement.InnerText;
                        break;
                    case nameof(ImportFromTxtButton):
                        ImportFromTxtButton .Content = subElement.InnerText;
                        break;
                    case nameof(LanguageLabel):
                        LanguageLabel.Content = subElement.InnerText;
                        break;
                    default:
                        break;
                };
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
        }
        private void ImportFromTxtButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "NameList (*.txt)|*.txt";
            if (ofd.ShowDialog() == true)
            {
                binding.NameEnableDictionary.Clear();
                FileStream file = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);
                string? line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    binding.NameEnableDictionary.Add(line,true);
                }
                sr.Close();
                file.Close();
                _flushCBoxItems();
                binding.UpdateNumberList();
                binding.ResultList.Clear();
            }
        }
        IEnumerable<String> _getSelectedItemsName()
        {
            foreach (String? item in NameMgrBox.SelectedItems)
            {
                var str = item?.Substring(4);
                if (str != null)
                {
                    if (binding.NameEnableDictionary.ContainsKey(str))
                    {
                        yield return str;
                    }
                }
            }
        }
        private void DisableNameButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (String item in _getSelectedItemsName())
            {
                binding.NameEnableDictionary[item] = false;
            }
            _flushCBoxItems();
            binding.UpdateNumberList();
        }
        private void EnableNameButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (String? item in _getSelectedItemsName())
            {
                binding.NameEnableDictionary[item] = true;
            }
            _flushCBoxItems();
            binding.UpdateNumberList();
        }
        private void DeleteNumberButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (String item in _getSelectedItemsName())
            {
                if (binding.NameEnableDictionary.ContainsKey((String)item))
                {
                    binding.NameEnableDictionary.Remove((String)item);
                }
            }
            _flushCBoxItems();
            binding.UpdateNumberList();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            String Name = String.Empty;
            if(AddTextBox.Text.Length > 0)
            {
                Name = AddTextBox.Text;
                if (!binding.NameEnableDictionary.ContainsKey(Name))
                {
                    binding.NameEnableDictionary.Add((String)Name, true);
                }
            }
            binding.UpdateNumberList();
            _flushCBoxItems();
        }
        private void SelectionLanguageCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _initLanguage(binding.LanguageFile);
        }

        private void IsRepeatCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            binding.ResultList.Clear();
        }

        private void SelectionTimesCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            binding.ResultList.Clear();
        }
    }
}
