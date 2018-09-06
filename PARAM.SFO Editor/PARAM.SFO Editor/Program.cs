using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PARAM.SFO_Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Check to see if we are ClickOnce Deployed.
            //i.e. the executing code was installed via ClickOnce
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                try
                {
                    foreach (string commandLineFile in AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData)
                    {
                        //MessageBox.Show(string.Format("Command Line File: {0}", commandLineFile));
                        Application.Run(new Form1(commandLineFile.Replace("file:///","")));
                        return;
                    }
                   
                }

                catch (Exception ex)
                {
                    Application.Run(new Form1());
                    return;
                }
            }
            else if(args.Length > 0)
            {
                Application.Run(new Form1(args[0].ToString()));
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
