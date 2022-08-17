using Indicon.Api.IpManager.Forms;
using Indicon.Api.IpManager.Classes;
namespace Indicon.Api.IpManager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            IpManagerForm oForm = new();
            StaticIpManager.Init(ref oForm);
            Application.Run(oForm);
        }
    }
}