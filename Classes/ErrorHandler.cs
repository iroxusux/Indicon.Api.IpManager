namespace Indicon.Api.IpManager.Classes
{
    internal static class NotifyHandler
    {
        internal static void GeneralNotification(ErrorCode oCode)
        {
            MessageBox.Show($"Code: {oCode.Value}: {oCode.Message}", oCode.DisplayName);
        }
        internal static void FatalError(ErrorCode oCode, bool bEnvExit = true)
        {
            GeneralNotification(oCode);
            if(bEnvExit) { Environment.Exit(oCode.Value); }
        }
    }
    internal class ErrorCode : Enumeration
    {
        private readonly string _Message;
        public string Message { get { return _Message; } }
        internal ErrorCode(int sValue, string sDisplayName, string sMessage) : base(sValue, sDisplayName) { this._Message = sMessage; }
    }
}
