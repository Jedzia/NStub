using System;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace NStub.Gui
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

            //AppDomain ad = AppDomain.CreateDomain("Test");
            ///AppDomain ad = AppDomain.CurrentDomain;
            //ad.AssemblyResolve += new ResolveEventHandler(ad_AssemblyResolve);
            //ad.TypeResolve += new ResolveEventHandler(ad_TypeResolve);


            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            Application.Run(new MainForm());


		}

        static Assembly ad_TypeResolve(object sender, ResolveEventArgs args)
        {
            throw new NotImplementedException();
        }

        static Assembly ad_AssemblyResolve(object sender, ResolveEventArgs args)
        {

            var ass = Assembly.Load(args.Name);
            return ass;
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            //global::NStub.Gui.Properties.Settings.Default.Save();
        }
	}
}