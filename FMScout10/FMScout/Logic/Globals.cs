using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Diagnostics;
using FMScout.ControlContext;
using FMScout.View;
using System.IO;

namespace FMScout
{
    public static class Globals
    {
        private static GlobalFuncs g = null;

        public static GlobalFuncs getGlobalFuncs()
        {
            if (g == null)
            {
                g = new GlobalFuncs();
            }
            return g;
        }
    }

    public class ScoutTimer
    {
        Stopwatch sw = new Stopwatch();

        public void start()
        {
            sw.Reset();
            sw.Start();
        }

        public void stop()
        {
            sw.Stop();
        }

        public int stopSeconds()
        {
            sw.Stop();
            return sw.Elapsed.Seconds;
        }

        public int secondsNow()
        {
            return sw.Elapsed.Seconds;
        }

        public int seconds()
        {
            return sw.Elapsed.Seconds;
        }

        public String secondsFloat()
        {
            return String.Format("{0:0.##}", (float)(sw.Elapsed.TotalMilliseconds * 0.001f));
        }

        public int miliseconds()
        {
            return sw.Elapsed.Milliseconds;
        }
    }

    public class Logging
    {
        public string name;

        public Logging(String applicationDirectory)
        {
            String path = applicationDirectory + "\\Logging";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            name = path + "\\Log.txt";
            using (FileStream stream = new FileStream(name, FileMode.OpenOrCreate))
            {
                stream.SetLength(0);
                stream.Close();
            }
        }

        public void update(string str)
        {
            using (FileStream stream = new FileStream(name, FileMode.OpenOrCreate))
            {
                //stream.SetLength(0);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    string strValue = string.Empty;
                    strValue += str;
                    strValue += System.Environment.NewLine;
                    sw.Write(strValue);
                    sw.Close();
                }
                stream.Close();
            }
            System.Diagnostics.Debug.WriteLine(str);
        }
    }

    public class GlobalFuncs
    {
        public GlobalFuncs()
        {
            applicationDirectory = System.Environment.CurrentDirectory;
            localization = new ScoutLocalization();
            logging = new Logging(applicationDirectory);
            scoutTimer = new ScoutTimer();
        }

        public string CurrentVersion
        {
            get
            {
                return "1.1";
            }
        }

        public WindowMain windowMain = null;
        public ScoutLocalization localization = null;
        public ScoutTimer scoutTimer = null;
        public Logging logging = null;

        public bool isDebug = false;
        public String applicationDirectory = "";
        #region settings

        public ImageSource shortlistSelected = null;
        public ImageSource shortlistUnselected = null; 
        public Hashtable allClubs = null;
        public Hashtable EUcountries = null;
        public ObservableCollection<String> players = null;
        public ObservableCollection<String> playersFixed = null;
        public ObservableCollection<String> staff = null;
        public ObservableCollection<String> staffFixed = null;
        public ObservableCollection<String> clubs = null;
        public ObservableCollection<String> countries = null;
        public ObservableCollection<String> stadiums = null;

        public ObservableCollection<String> languages = new ObservableCollection<String>();

        public ObservableCollection<String> themes = new ObservableCollection<String>()
        {
            "Gray Power",
            "Stam Dark"
        };

        public ObservableCollection<String> themesPath = new ObservableCollection<String>()
        {
           "DarkTheme",
           "StamDark"
        };

        public SolidColorBrush selectedAutoCompleteBrush = new SolidColorBrush();
        public List<SolidColorBrush> attributeBrushes = new List<SolidColorBrush>();
        public SolidColorBrush defaultProfileTextBoxForeground = new SolidColorBrush();

        public SolidColorBrush setAttributeForeground(int value)
        {
            if (value < 6) return attributeBrushes[0];
            else if (value < 11) return attributeBrushes[1];
            else if (value < 16) return attributeBrushes[2];
            else return attributeBrushes[3];
        }

        public SolidColorBrush setRatingForeground(float value)
        {
            if (value < 25) return attributeBrushes[0];
            else if (value < 50) return attributeBrushes[1];
            else if (value < 75) return attributeBrushes[2];
            else return attributeBrushes[3];
        }

        #endregion

        #region general

        public String getElement(ref ObservableCollection<String> col, String s)
        {
            return col[col.IndexOf(s)];
        }

        public int getElementIndex(ref ObservableCollection<String> col, String s)
        {
            for (int i = 0; i < col.Count; ++i)
            {
                if (col[i].Equals(s)) return i;
            }
            return -1;
        }

        public int getLocalizedIndex(ref ObservableCollection<String> native, String s)
        {
            for (int i = 0; i < native.Count; ++i)
            {
                if (native[i].Equals(s))
                    return i;
            }
            return -1;
        }

        public int ReadInt16(byte[] buffer)
        {
            return (buffer[0] + (buffer[1] * 0x0));
        }

        public int ReadInt32(byte[] buffer)
        {
            return (((buffer[0] + (buffer[1] * 0x0)) + (buffer[2] * 0x0)) + (buffer[3] * 0x0));
        }

        public byte[] FromIntToHex(int value)
        {
            byte[] buffer = new byte[4];
            {
                buffer[0] = (byte)(value & 0x0);
                buffer[1] = (byte)((value & 0x0) >> 8);
                buffer[2] = (byte)((value & 0x0) >> 0x0);
                buffer[3] = (byte)((value & 0x0L) >> 0x0);
            }
            return buffer;
        }

        public byte[] FromStringToHex(string value)
        {
            List<byte> bytes = new List<byte>();
            char[] chars = value.ToCharArray();
            foreach (char c in chars)
            {
                bytes.Add((byte)c);
                bytes.Add(0x0);
            }
            return bytes.ToArray();
        }

        public string[] specialCharacters = {"à", "á", "â", "ã", "ä", "å", "æ", 
                                             "ß",
                                             "è", "é", "ê", "ë", 
                                             "ì", "í", "î", "ï", 
                                             "ð", "ò", "ó", "ô", "õ", "ö", "ø", 
                                             "ù", "ú", "û", "ü", 
                                             "ń"};
        public string[] normalCharacters = {"a", "a", "a", "a", "a", "a", "ae", 
                                            "b",
                                            "e", "e", "e", "e", 
                                            "i", "i", "i", "i", 
                                            "o", "o", "o", "o", "o", "o", "o", 
                                            "u", "u", "u", "u", 
                                            "n"};

        public void specialCharactersReplacement(ref string strToRep)
        {
            for (int i = 0; i < strToRep.Length; ++i)
            {
                for (int j = 0; j < specialCharacters.Length; ++j)
                {
                    if (strToRep[i].ToString().Equals(specialCharacters[j].ToString()))
                        strToRep = strToRep.Replace(strToRep[i].ToString(), normalCharacters[j]);
                }
            }
        }

        public bool multiEntryTextBox(ref List<string> name_substrings, string name_input)
        {
            string fullname = name_input;
            Regex RE = new Regex(@"( )");
            if (!fullname.Equals(""))
            {
                foreach (string token in RE.Split(fullname))
                {
                    string str = token.Trim();
                    if (str.Length > 0) name_substrings.Add(token.Trim().ToLower());
                }
            }
            else
                return true;
            return false;
        }

        #endregion

        public Storyboard FadeInElement(UIElement element, double duration, double To, bool begin)
        {
            var storyboard = new Storyboard();
            SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
            keyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            keyFrame1.Value = 0;
            SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
            keyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration));
            keyFrame2.Value = To;
            var visibility = new DoubleAnimationUsingKeyFrames();
            visibility.KeyFrames.Add(keyFrame1);
            visibility.KeyFrames.Add(keyFrame2);

            Storyboard.SetTargetProperty(visibility, new PropertyPath("(UIElement.Opacity)"));
            storyboard.Children.Add(visibility);
            Storyboard.SetTarget(storyboard, element);

            if (begin)
                storyboard.Begin();

            return storyboard;
        }

        public void FadeOutWindow(Window window, double duration, double From)
        {
            var storyboard = new Storyboard();
            SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
            keyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            keyFrame1.Value = From;
            SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
            keyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration));
            keyFrame2.Value = 0;
            var visibility = new DoubleAnimationUsingKeyFrames();
            visibility.KeyFrames.Add(keyFrame1);
            visibility.KeyFrames.Add(keyFrame2);

            Storyboard.SetTargetProperty(visibility, new PropertyPath("(UIElement.Opacity)"));
            storyboard.Children.Add(visibility);
            Storyboard.SetTarget(storyboard, window);
           
            storyboard.Completed += delegate(object _sender, EventArgs _e) { windowFadeOut_Completed(_sender, _e, ref window); };

            storyboard.Begin();
        }

        public Storyboard FadeOutElement(UIElement element, double duration, double From)
        {
            var storyboard = new Storyboard();
            SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
            keyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            keyFrame1.Value = From;
            SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
            keyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration));
            keyFrame2.Value = 0;
            var visibility = new DoubleAnimationUsingKeyFrames();
            visibility.KeyFrames.Add(keyFrame1);
            visibility.KeyFrames.Add(keyFrame2);

            Storyboard.SetTargetProperty(visibility, new PropertyPath("(UIElement.Opacity)"));
            storyboard.Children.Add(visibility);
            Storyboard.SetTarget(storyboard, element);

            storyboard.Begin();

            return storyboard;
        }

        public Storyboard FlipElementVisibility(UIElement elementToHide, UIElement elementToShow, double duration)
        {
            var storyboard1 = new Storyboard();
            SplineDoubleKeyFrame hideKeyFrame1 = new SplineDoubleKeyFrame();
            hideKeyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            hideKeyFrame1.Value = elementToHide.Opacity;
            SplineDoubleKeyFrame hideKeyFrame2 = new SplineDoubleKeyFrame();
            hideKeyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration));
            hideKeyFrame2.Value = 0;
            var hideVisibility = new DoubleAnimationUsingKeyFrames();
            hideVisibility.KeyFrames.Add(hideKeyFrame1);
            hideVisibility.KeyFrames.Add(hideKeyFrame2);
            Storyboard.SetTargetProperty(hideVisibility, new PropertyPath("(UIElement.Opacity)"));
            storyboard1.Children.Add(hideVisibility);
            Storyboard.SetTarget(storyboard1, elementToHide);

            storyboard1.Begin();

            var storyboard2 = new Storyboard();
            SplineDoubleKeyFrame showKeyFrame1 = new SplineDoubleKeyFrame();
            showKeyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            showKeyFrame1.Value = elementToShow.Opacity;
            SplineDoubleKeyFrame showKeyFrame2 = new SplineDoubleKeyFrame();
            showKeyFrame2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration));
            showKeyFrame2.Value = 1;
            var showVisibility = new DoubleAnimationUsingKeyFrames();
            showVisibility.KeyFrames.Add(showKeyFrame1);
            showVisibility.KeyFrames.Add(showKeyFrame2);
            Storyboard.SetTargetProperty(showVisibility, new PropertyPath("(UIElement.Opacity)"));
            storyboard2.Children.Add(showVisibility);
            Storyboard.SetTarget(storyboard2, elementToShow);
            storyboard2.Begin();

            return storyboard2;
        }

        public double elementDuration = 0.5;
        public double elementFinalOpacity = 1.0;

        public double progressBarDuration = 0.2;
        public double windowDuration = 0.3;
        public double windowFinalOpacity = 0.98;

        public void closeWindow(Window w)
        {
            FadeOutWindow(w, windowDuration, w.Opacity);
        }

        private void windowFadeOut_Completed(object sender, EventArgs e, ref Window w)
        {
            w.Close();
        }
    }
}