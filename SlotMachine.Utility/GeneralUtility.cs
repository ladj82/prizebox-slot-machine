using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SlotMachine.Utility
{
    public class GeneralUtility
    {
        public static DateTime? DateTime;

        public static string GetRandomAlphaNumericString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int GetRandomNumericString(int length)
        {
            const string chars = "123456789";
            var random = new Random();

            return Convert.ToInt32(new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray()));
        }

        public static string GenerateRandomKey(int length)
        {
            var cryptRNG = new RNGCryptoServiceProvider();
            var tokenBuffer = new byte[length];

            cryptRNG.GetBytes(tokenBuffer);

            return Convert.ToBase64String(tokenBuffer).ToUpper();
        }

        public static string GetMachineFingerPrint()
        {
            var biosVersion = GetMachineAttribute("Win32_BIOS", "Version");

            var biosSerialNumber = GetMachineAttribute("Win32_BIOS", "SerialNumber");

            var baseBoardSerialNumber = GetMachineAttribute("Win32_BaseBoard", "SerialNumber");

            var processorId = GetMachineAttribute("Win32_Processor", "ProcessorId");

            var processorUniqueId = GetMachineAttribute("Win32_Processor", "UniqueId");

            //var macAddress = GetMachineAttribute("Win32_NetworkAdapterConfiguration", "MACAddress");

            //var macAddress = NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet).Where(nic => nic.OperationalStatus == OperationalStatus.Up).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();

            var machineIdentifier = (biosVersion + biosSerialNumber + baseBoardSerialNumber + processorId + processorUniqueId).Replace(" ", string.Empty);
            //LogUtility.Log(LogUtility.LogType.Information, "Machine Identifier  - " + machineIdentifier);

            return machineIdentifier;
        }

        private static string GetMachineAttribute(string wmiClass, string wmiProperty)
        {
            var mc = new ManagementClass(wmiClass);
            var moc = mc.GetInstances();

            foreach (var item in moc)
            {
                try
                {
                    var mo = (ManagementObject)item;
                    var mop = mo[wmiProperty];

                    if (mop == null) continue;

                    var result = mop.ToString();

                    if (!string.IsNullOrEmpty(result))
                        return result;
                }
                catch (Exception ex)
                {
                    LogUtility.Log(LogUtility.LogType.Error, "GetMachineAttribute - " + ex.Message);
                    throw;
                }
            }

            return string.Empty;
        }

        public static string AssemblyDirectory
        {
            get
            {
                var sCodeBase = Assembly.GetExecutingAssembly().CodeBase;
                var oUri = new UriBuilder(sCodeBase);
                var sPath = Uri.UnescapeDataString(oUri.Path);

                return Path.GetDirectoryName(sPath);
            }
        }

        public static void Wait(int iMilliseconds)
        {
            var oStartTime = System.DateTime.Now;

            while ((System.DateTime.Now - oStartTime).TotalMilliseconds < iMilliseconds)
                Application.DoEvents();
        }

        public static Dictionary<string, string> ParseUrlParameters(string sUrl)
        {
            try
            {
                return sUrl.Split('?')[1].Split('&').Where(item => !string.IsNullOrEmpty(item)).ToDictionary(item => item.Split('=')[0], item => item.Split('=')[1]);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Shuffle<T>(IList<T> oArrayList)
        {
            var oRandom = new Random();
            var iArrayCount = oArrayList.Count;

            for (var iCount = 0; iCount < iArrayCount; iCount++)
            {
                var iArrayRandomIndex = iCount + (int)(oRandom.NextDouble() * (iArrayCount - iCount));
                var oArrayItem = oArrayList[iArrayRandomIndex];

                oArrayList[iArrayRandomIndex] = oArrayList[iCount];
                oArrayList[iCount] = oArrayItem;
            }
        }

        public static IEnumerable<TValue> GetDictionaryRandomItems<TKey, TValue>(IDictionary<TKey, TValue> dicObject)
        {
            var oRandom = new Random();
            var lstObject = dicObject.Values.ToList();
            var iSize = dicObject.Count;
            
            while (true)
            {
                yield return lstObject[oRandom.Next(iSize)];
            }
        }
    }
}
