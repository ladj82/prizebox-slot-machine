using System.Runtime.InteropServices;

namespace SlotMachine.Controller
{
    public class InternetController
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable
        {
            get
            {
                int description;
                return InternetGetConnectedState(out description, 0);
            }
        }
    }
}
