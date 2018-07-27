using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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

namespace PS4_PS2_Classics_Gui__WPF_.HowToUse
{
    /// <summary>
    /// Interaction logic for HowToUse.xaml
    /// </summary>
    public partial class HowToUse : Window
    {
        public HowToUse()
        {
            InitializeComponent();
        }

        int CurrentIdx = 0;

        public class TutorialImages
        {
            public string TutorialText { get; set; }
            public Bitmap TutorialImage { get; set; }
        }

        public List<TutorialImages> TutWithImages = new List<TutorialImages>();

        public static BitmapSource ConvertToImageSource(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private void imagforward_MouseEnter(object sender, MouseEventArgs e)
        {
            if(imagforward.IsMouseOver == true)
            {
                imagforward.Source = ConvertToImageSource(Properties.Resources.icons8_go_back_24__1_);
            }
            if(imgback.IsMouseOver == true)
            {
                imgback.Source = ConvertToImageSource(Properties.Resources.icons8_go_back_24__1_);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //programically add the steps and description
            TutWithImages.Add(new TutorialImages{ TutorialText = "Welcome to the Tutorial \n\nThis is the main UI(User interface) for PS2 Classics \n\nThe Main Controls for this screen are as follows\n\n * File \n\n * Mute Button\n\n * PS2 ISO Path\n\n * ISO Button \n\n * NP Title ID\n\n * Title Text Box\n\n * Create PS2 Classics Button ", TutorialImage = Properties.Resources.MainUI });
            TutWithImages.Add(new TutorialImages { TutorialText = "File Menu \n\n There are 5 Options on this menu \n\n\t*Settings\n\n\tThis Opens The Settings View\n\n\t*Log an issue\n\n Allows You To Log an Issue on my Github\n\n\t*Add Custom PS2 Config\n\n\t This allows you to use a custom config for the PS2 Classic(This Overrides the default one)\n\n\t*Credits\n\n\tThis Opens Up The Credit View Thanks Everyone\n\n\t*How To Use\n\n\tOpens This View", TutorialImage = Properties.Resources.MainUIFile });
            TutWithImages.Add(new TutorialImages { TutorialText = "Settings \n\nThis view will have all the settings for PS2 Classics Gui you can turn any setting on and off from here", TutorialImage = Properties.Resources.SettingsView });
            //TutWithImages.Add(new TutorialImages { TutorialText = "Log an Issue\n\nThis will open up a browser to github so you can log an issue", TutorialImage = Properties.Resources.MainUIFile });
            //TutWithImages.Add(new TutorialImages { TutorialText = "Add Custom Config\n\nThis will allow you to replace the default "})
            TutWithImages.Add(new TutorialImages { TutorialText = "Mute/Unmute XMB Music\n\nThis will mute and unmute PS2 Classics Music (XMB Music)\n\nThis option can be found at the top right of the screen",TutorialImage = Properties.Resources.Mute_Inmute });

            TutWithImages.Add(new TutorialImages { TutorialText = "Single ISO\n\nTo Add a single iso for PS2 classics Click on the ISO Button and select you single iso \nthis will auto load the PS2 Title as Np Title ID into the GUI\n\n(Attached is 2 image to show the process) \n Current 1/2 " , TutorialImage = Properties.Resources.PS2_Single_ISO});

            TutWithImages.Add(new TutorialImages { TutorialText = "Single ISO\n\nTo Add a single iso for PS2 classics Click on the ISO Button and select you single iso \nthis will auto load the PS2 Title as Np Title ID into the GUI\n\n(Attached is 2 image to show the process) \n Current 2/2 ", TutorialImage = Properties.Resources.PS2_NP_Title_ID });



            TutWithImages.Add(new TutorialImages { TutorialText = "Multi ISO\n\nTo Add Multiple iso(s) for PS2 classics Click on the ISO Button and select all your desired iso (s) Maximum of 7 allowed\nYou will need to add a Np Title ID into the GUI\n\n(Attached is 3 image to show the process) \n Current 1/3 ", TutorialImage = Properties.Resources.MultiISO1});

            TutWithImages.Add(new TutorialImages { TutorialText = "Multi ISO\n\nTo Add Multiple iso(s) for PS2 classics Click on the ISO Button and select  all your desired iso (s) Maximum of 7 allowed \nYou will need to add a Np Title ID into the GUI\n\n(Attached is 3 image to show the process) \n Current 2/3 ", TutorialImage = Properties.Resources.MultiISO2 });

            TutWithImages.Add(new TutorialImages { TutorialText = "Multi ISO\n\nTo Add Multiple iso(s) for PS2 classics Click on the ISO Button and select  all your desired iso (s) Maximum of 7 allowed \nYou will need to add a Np Title ID into the GUI\n\n(Attached is 3 image to show the process) \n Current 3/3 ", TutorialImage = Properties.Resources.MultiISO3 });

            TutWithImages.Add(new TutorialImages { TutorialText = "Title \n\nThis is the Title you will see on the PS4 XMB", TutorialImage = Properties.Resources.PS2_Title });

            TutWithImages.Add(new TutorialImages { TutorialText = "Click Create PS2 Classics\n\nThis will start the creation process select a location to save and wait for it to complete", TutorialImage = Properties.Resources.Custom_Create });
            //load starting items
            SetImageIndInfo();
        }

        private void imgback_MouseLeave(object sender, MouseEventArgs e)
        {

            imagforward.Source = ConvertToImageSource(Properties.Resources.icons8_go_back_24);

            imgback.Source = ConvertToImageSource(Properties.Resources.icons8_go_back_24);

        }

        private void imgback_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(CurrentIdx > 0)
            {
                CurrentIdx--;
                SetImageIndInfo();
                
            }
        }

        private void SetImageIndInfo()
        {
            if(TutWithImages.Count <= CurrentIdx)
            {
                return;
            }
            textBlock.Text = TutWithImages[CurrentIdx].TutorialText;
            image.Source = ConvertToImageSource(TutWithImages[CurrentIdx].TutorialImage);
            SoundClass.PlayPS4Sound(SoundClass.Sound.Navigation);
        }

        private void imagforward_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(CurrentIdx < (TutWithImages.Count -1))
            {
                CurrentIdx++;
                SetImageIndInfo();

            }
        }
    }
}
