using Indicon.Api.IpManager.Forms;
using Indicon.Api.IpManager.Classes;
using Engine.Forms.Classes;
namespace Indicon.Api.IpManager
{
    internal static class Program
    {
        private static bool DebugMode = false;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /// Before we do anything at all, check if the same process already exists as a running process. If true, exit the environment (we don't want 2 programs running at the same time, especially for file control)
            var exists = System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Length > 1;
            if (exists)
            {
                Environment.Exit(0);
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            /// Configure and run application
            IpManagerForm oForm = new();  // The view
            NewIpManager oManager = new();
            oManager.BindToForm(ref oForm);
            oManager.Init();
            if (DebugMode)
            {
                Debugger.Load();
            }
            Application.Run(oForm);
        }
    }
}