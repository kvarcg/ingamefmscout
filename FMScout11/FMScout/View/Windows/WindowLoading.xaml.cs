using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;

namespace FMScout.View
{
    public partial class WindowLoading : Window
    {
        private WindowMain windowMain = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;

        public WindowLoading(WindowMain windowMain)
        {
            InitializeComponent();

            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs(); 
            this.windowMain = windowMain;
        }

        public void setLoading(string str)
        {
            this.loadingLabel.Text = Environment.NewLine + str;
        }

        public void setInfo(string str)
        {
            this.infoLabel.Text = System.Environment.NewLine + str;
        }

        public void finishLoading(bool res)
        {
            this.windowMain.IsEnabled = true;
            this.windowMain.reset(res);

            context.scoutLoaded = res;
            FadeOutWindow(this, globalFuncs.windowDuration, this.Opacity);
        }

        private void windowFadeOut_Completed(object sender, EventArgs e)
        {
            this.Close();
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

            storyboard.Completed += windowFadeOut_Completed;

            storyboard.Begin();
        }
    }
}
