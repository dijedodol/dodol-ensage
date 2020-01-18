using System;
using System.Reflection;
using Ensage;
using Ensage.Common;
using PlaySharp.Toolkit.Logging;

namespace DodolAutoDota2
{
    using log4net;
    
    internal class Program
    {
        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static DodolAutoItemUsage _dodolAutoItemUsage;
        
        public static void Main(string[] args)
        {
            Events.OnLoad += Events_OnLoad;
            Events.OnClose += Events_OnClose;
        }

        private static void Events_OnLoad(object sender, EventArgs e)
        {
            Log.Info("Events_OnLoad");
            _dodolAutoItemUsage = new DodolAutoItemUsage();
            _dodolAutoItemUsage.start();
        }
        
        private static void Events_OnClose(object sender, EventArgs e)
        {
            Log.Info("Events_OnClose");
            _dodolAutoItemUsage.stop();
        }
    }
}