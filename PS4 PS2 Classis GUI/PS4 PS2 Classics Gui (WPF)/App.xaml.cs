using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

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
