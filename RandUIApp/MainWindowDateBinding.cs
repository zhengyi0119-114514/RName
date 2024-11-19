using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RandUIApp
{
    public partial class MainWindowDateBinding : INotifyPropertyChanged
    {
        public MainWindowDateBinding() { }
        public void LoadFromXmlConfigFile(string xmlConfigFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlConfigFile);
            XmlElement root = doc.DocumentElement!;
            if (root == null) return;
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.Name == "NameList")
                {
                    foreach (XmlElement n2 in n.ChildNodes)
                    {
                        m_nameEnableDictionary.Add(n2.GetAttribute("value"), Boolean.Parse(n2.GetAttribute("enable")));
                    }
                }
                else if (n.Name == "settings")
                {
                    foreach (XmlElement n2 in n.ChildNodes)
                    {
                        switch (n2.GetAttribute("name"))
                        {
                            case nameof(IsRepeat):
                                IsRepeat = Boolean.Parse(n2.GetAttribute("value"));
                                break;
                            case nameof(IsMultipleChoices):
                                IsMultipleChoices = Boolean.Parse(n2.GetAttribute("value"));
                                break;
                            case nameof(IsCelebrate):
                                IsCelebrate = Boolean.Parse(n2.GetAttribute("value"));
                                break;
                            case nameof(MultipleChoicesTimes):
                                MultipleChoicesTimes = Int32.Parse(n2.GetAttribute("value"));
                                break;
                            case nameof(DefaultLanguage):
                                DefaultLanguage = n2.GetAttribute("value");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public void SaveToXMLFile(String path)
        {
            if(File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream f = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (XmlWriter xw = XmlWriter.Create(f))
                {
                    xw.WriteStartDocument();
                    xw.WriteStartElement("AppConfig");
                    xw.WriteStartElement("NameList");
                    foreach(var i in this.NameEnableDictionary)
                    {
                        xw.WriteStartElement("Name");
                        xw.WriteAttributeString("value", i.Key);
                        xw.WriteAttributeString("enable", i.Value.ToString().ToLowerInvariant());
                        xw.WriteEndElement();
                    }
                    xw.WriteEndElement();
                    xw.WriteStartElement("settings");
                    foreach(var i in new[] 
                        {
                        (nameof(IsRepeat),IsRepeat.ToString().ToLowerInvariant()),
                        (nameof(IsMultipleChoices),IsMultipleChoices.ToString().ToLowerInvariant()),
                        (nameof(IsCelebrate),IsCelebrate.ToString().ToLowerInvariant()),
                        (nameof(MultipleChoicesTimes),MultipleChoicesTimes.ToString()),
                        (nameof(DefaultLanguage),DefaultLanguage.ToLowerInvariant()),
                        })
                    {
                        xw.WriteStartElement("Setting");
                        xw.WriteAttributeString("name",i.Item1);
                        xw.WriteAttributeString("value", i.Item2);
                        xw.WriteEndElement();
                    }
                    xw.WriteEndElement();
                    xw.WriteEndElement();
                    xw.WriteEndDocument();
                }
            }
        }
        public void UpdateNumberList()
        {
            NumberList.Clear();
            Int32 n = 0;
            foreach (var i in this.NameEnableDictionary)
            {
                if (i.Value)
                {
                    n++;
                    this.NumberList.Add(n.ToString());
                }
            }
        }
        public void InitLanguageList()
        {
            foreach(var file in m_languageFileDir.EnumerateFiles("*.xml"))
            {
                LanguageList.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }
        }
        private void _onChange(String proppertyName)
        {
            var func = this.PropertyChanged;
            func?.Invoke(this, new PropertyChangedEventArgs(proppertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        ObservableCollection<String> m_nameMgrBindingList = new ObservableCollection<String>();
        ObservableCollection<String> m_numberList = new ObservableCollection<String>();
        ObservableCollection<String> m_resultList = new ObservableCollection<String>();
        ObservableCollection<String> m_languageList = new ObservableCollection<String>();
        Dictionary<String, Boolean> m_nameEnableDictionary = new Dictionary<String, Boolean>();
        String m_defaultLanguage = "zh-cn";
        String m_clelbrateFormat = String.Empty;
        String m_clelbrateNothing = String.Empty;
        DirectoryInfo m_languageFileDir = new DirectoryInfo("./Date/Lang");
        Boolean m_IsRepeat = false;
        Boolean m_IsMultipleChoices = false;
        Boolean m_IsCelebrate = false;
        Int32 m_MultipleChoicesTimes = 1;
        public ObservableCollection<String> LanguageList
        {
            get { return m_languageList; }
            set
            {
                m_languageList = value;
                _onChange(nameof(LanguageList));
            }
        }
        public ObservableCollection<String> NumberList
        {
            get { return m_numberList; }
            set
            {
                m_numberList = value;
                _onChange(nameof(NumberList));
            }
        }
        public ObservableCollection<String> NameMgrBindingList
        {
            get { return m_nameMgrBindingList; }
            set
            {
                m_nameMgrBindingList = value;
                _onChange(nameof(NameMgrBindingList));
            }
        }
        public ObservableCollection<String> ResultList
        {
            get { return m_resultList; }
            set
            {
                m_resultList = value;
                _onChange(nameof(ResultList));
            }
        }
        public Boolean IsRepeat
        {
            get
            {
                return m_IsRepeat;
            }
            set
            {
                m_IsRepeat = value;
                _onChange(nameof(IsRepeat));
            }
        }
        public Boolean IsMultipleChoices
        {
            get
            {
                return m_IsMultipleChoices;
            }
            set
            {
                m_IsMultipleChoices = value;
                _onChange(nameof(IsMultipleChoices));
            }
        }
        public Boolean IsCelebrate
        {
            get
            {
                return m_IsCelebrate;
            }
            set
            {
                m_IsCelebrate = value;
                _onChange(nameof(IsCelebrate));
            }
        }
        public String DefaultLanguage
        {
            get
            {
                return m_defaultLanguage;
            }
            set
            {
                m_defaultLanguage = value;
                _onChange(nameof(DefaultLanguage));
            }
        }
        public String LanguageFile
        {
            get 
            {
                return $"{m_languageFileDir}/{m_defaultLanguage}.xml";
            }
        }
        public String ClelbrateFormat
        {
            get
            {
                return m_clelbrateFormat;
            }
            set
            {
                m_clelbrateFormat = value;
            }
        }
        public String ClelbrateNothing
        {
            get
            {
                return m_clelbrateNothing;
            }
            set
            {
                m_clelbrateNothing = value;
                _onChange(nameof(ClelbrateNothing));
            }
        }
        public Int32 MultipleChoicesTimes
        {
            get
            {
                return m_MultipleChoicesTimes;
            }
            set
            {
                m_MultipleChoicesTimes = value;
                _onChange(nameof(MultipleChoicesTimes));
            }
        }
        public Dictionary<String, Boolean> NameEnableDictionary
        {
            get
            {
                return this.m_nameEnableDictionary;
            }
            set
            {
                this.m_nameEnableDictionary = value;
            }
        }
    }
}
