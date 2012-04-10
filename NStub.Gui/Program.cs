using System;
using System.Windows.Forms;

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
			Application.Run(new MainForm());
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
		}

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            //global::NStub.Gui.Properties.Settings.Default.Save();
        }
	}
}