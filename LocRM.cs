using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace iSpyApplication
{
    public static class LocRm
    {
        private static Translations _translationsList;
        private static TranslationsTranslationSet _currentSet;
        private static readonly Dictionary<string, string> Res = new Dictionary<string, string>();
        private static bool _inited;

        public static List<TranslationsTranslationSet> TranslationSets => TranslationsList.TranslationSet.ToList();

        public static Translations TranslationsList
        {
            get
            {
                if (_translationsList != null)
                    return _translationsList;
                var s = new XmlSerializer(typeof (Translations));
                using (var fs = new FileStream(Program.AppDataPath + @"\XML\Translations.xml", FileMode.Open))
                {
                    fs.Position = 0;
                    using (TextReader reader = new StreamReader(fs))
                    {
                        _translationsList = (Translations)s.Deserialize(reader);
                    }
                }

                return _translationsList;
            }
        }

        public static void Reset()
        {

            _translationsList = null;
            _inited = false;
        }

        public static string CultureCode
        {
            get
            {
                if (_currentSet!=null)
                    return _currentSet.CultureCode;
                return "en";
            }
        }

        public static string GetString(string identifier)
        {
            if (!_inited)
            {
                Init();
            }
            try
            {
                return Res[identifier];
            }
            catch (KeyNotFoundException)
            {
                MainForm.LogErrorToFile("No Translation for token " + identifier);
                if (CultureCode != "en")
                {
                    var eng = TranslationSets.FirstOrDefault(p => p.CultureCode == "en");
                    var token = eng?.Translation.FirstOrDefault(p => p.Token == identifier);
                    if (token == null) return identifier;
                    Res.Add(identifier, token.Value);
                    return Res[identifier];
                }

            }
            catch
            {
                //possible threading error where language is reset
            }
            return identifier;
        }

        

        public static void SetString(Control ctrl, string identifier)
        {
            if (!_inited)
            {
                Init();
            }

            try
            {
                ctrl.Text = Res[identifier];
            }
            catch (KeyNotFoundException)
            {
                ctrl.Text = identifier;
                MainForm.LogErrorToFile("No Translation for token " + identifier);
                if (CultureCode != "en")
                {
                    var eng = TranslationSets.FirstOrDefault(p => p.CultureCode == "en");
                    var token = eng?.Translation.FirstOrDefault(p => p.Token == identifier);
                    if (token != null)
                    {
                        Res.Add(identifier, token.Value);
                        ctrl.Text = Res[identifier];
                    }
                }

            }
            catch
            {
                //possible threading error where language is reset
            }
        }

        public static void SetString(ToolStripStatusLabel ctrl, string identifier)
        {
            if (!_inited)
            {
                Init();
            }

            try
            {
                ctrl.Text = Res[identifier];
            }
            catch (KeyNotFoundException)
            {
                MainForm.LogErrorToFile("No Translation for token " + identifier);
                ctrl.Text = identifier;
                if (CultureCode!="en")
                {
                    var eng = TranslationSets.FirstOrDefault(p => p.CultureCode == "en");
                    var token = eng?.Translation.FirstOrDefault(p => p.Token == identifier);
                    if (token != null)
                    {
                        Res.Add(identifier, token.Value);
                        ctrl.Text = Res[identifier];
                    }
                }
                
            }
            catch
            {
                //possible threading error where language is reset
            }
            //dont modify design time text
        }

        public static void SetString(MenuItem ctrl, string identifier)
        {
            if (!_inited)
            {
                Init();
            }

            try
            {
                ctrl.Text = Res[identifier];
            }
            catch (KeyNotFoundException)
            {
                MainForm.LogErrorToFile("No Translation for token " + identifier);
                ctrl.Text = identifier;
                if (CultureCode != "en")
                {
                    var eng = TranslationSets.FirstOrDefault(p => p.CultureCode == "en");
                    var token = eng?.Translation.FirstOrDefault(p => p.Token == identifier);
                    if (token != null)
                    {
                        Res.Add(identifier, token.Value);
                        ctrl.Text = Res[identifier];
                    }
                }

            }
            catch
            {
                //possible threading error where language is reset
            }
        }


        private static void Init()
        {
            string lang = MainForm.Conf.Language;
            if (lang == "NotSet")
            {
                lang = CultureInfo.CurrentCulture.Name.ToLower();
                string lang1 = lang;
                if (TranslationSets.FirstOrDefault(p => p.CultureCode == lang1) != null)
                    MainForm.Conf.Language = lang;
                else
                {
                    lang = lang.Split('-')[0];
                    string lang2 = lang;
                    if (TranslationSets.FirstOrDefault(p => p.CultureCode == lang2) != null)
                        MainForm.Conf.Language = lang;
                    else
                        MainForm.Conf.Language = lang = "en";
                }
            }

            Res.Clear();
            _currentSet = TranslationSets.FirstOrDefault(p => p.CultureCode == lang);
            if (_currentSet != null)
            {
                
                foreach (TranslationsTranslationSetTranslation tran in _currentSet.Translation)
                {
                    try
                    {
                        Res.Add(tran.Token,
                        tran.Value.Replace("&amp;", "&")
                            .Replace("&lt;", "<")
                            .Replace("&gt;", ">")
                            .Replace("，", ",")
                            .Replace("#39;", "'"));
                    }
                    catch (Exception ex)
                    {
                        MainForm.LogErrorToFile("Translation: "+tran.Token+": "+ex.Message);
                    }
                }
                
            }

            _inited = true;
        }

    }
}