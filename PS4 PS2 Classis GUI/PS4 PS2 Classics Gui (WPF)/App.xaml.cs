using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;


namespace PS4_PS2_Classics_Gui__WPF_
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                if (e.Args.Length == 2)
                {
                    if (e.Args[1].ToString() == "Debug")
                    {
                        AppCenter.Start("186556f8-0b63-46ee-b823-e7fd9e697be7",
                           typeof(Analytics), typeof(Crashes));
                    }
                }
                else
                {              
                        AppCenter.Start("186556f8-0b63-46ee-b823-e7fd9e697be7",
                           typeof(Analytics));   
                }
                //MessageBox mess = new MessageBox("S")
                string versionnum = e.Args[0].ToString();
                PS4_PS2_Classics_Gui__WPF_.MainWindow.VersionNum = versionnum;
            }
            catch(Exception ex)
            {

            }

        }
    }
}
