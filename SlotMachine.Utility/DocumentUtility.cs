using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlotMachine.Utility
{
    public static class DocumentUtility
    {
        public static bool IsCpfValid(string sCpf)
        {
            try
            {
                sCpf = RemoveNotNumbers(sCpf);

                if (sCpf.Length > 11)
                    return false;

                while (sCpf.Length != 11)
                    sCpf = '0' + sCpf;

                var bEqual = true;
                for (var iCount = 1; iCount < 11 && bEqual; iCount++)
                    if (sCpf[iCount] != sCpf[0])
                        bEqual = false;

                if (bEqual || sCpf == "12345678909")
                    return false;

                var oNumbers = new int[11];

                for (var i = 0; i < 11; i++)
                    oNumbers[i] = int.Parse(sCpf[i].ToString());

                var iSum = 0;
                for (var i = 0; i < 9; i++)
                    iSum += (10 - i) * oNumbers[i];

                var iResult = iSum % 11;

                if (iResult == 1 || iResult == 0)
                {
                    if (oNumbers[9] != 0)
                        return false;
                }
                else if (oNumbers[9] != 11 - iResult)
                    return false;

                iSum = 0;
                for (var i = 0; i < 10; i++)
                    iSum += (11 - i) * oNumbers[i];

                iResult = iSum % 11;

                if (iResult == 1 || iResult == 0)
                {
                    if (oNumbers[10] != 0)
                        return false;
                }
                else
                    if (oNumbers[10] != 11 - iResult)
                        return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string RemoveNotNumbers(string sText)
        {
            try
            {
                var oRegex = new System.Text.RegularExpressions.Regex(@"[^0-9]");
                var sReturnString = oRegex.Replace(sText, string.Empty);

                return sReturnString;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
