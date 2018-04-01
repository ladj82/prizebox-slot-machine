using System;
using System.Collections.Generic;

namespace SlotMachine.Utility
{
    public class PrintUtility
    {
        public class PrinterInfo
        {
            public string Name { get; set; }

            public string ComPort { get; set; }

            public string Network { get; set; }

            public List<KeyValuePair<string, string>> Models { get; set; }
        }

        public static string GetCenterAlignmentText(string text, int qntCharactersPerLine, Enumerations.FontSize fontSize)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException();

            qntCharactersPerLine = AdjustCharactersPerLine(qntCharactersPerLine, fontSize);

            var qntSpaces = qntCharactersPerLine/2 - text.Length/2;
            qntSpaces = qntSpaces < 0 ? 0 : qntSpaces;

            var spaces = new string(' ', qntSpaces);

            return spaces;
        }

        public static string GetRightAlignmentText(string text, int qntCharactersPerLine, Enumerations.FontSize fontSize)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException();

            qntCharactersPerLine = AdjustCharactersPerLine(qntCharactersPerLine, fontSize);

            var qntSpaces = qntCharactersPerLine - text.Length;
            var spaces = new string(' ', qntSpaces);

            return spaces;
        }

        private static int AdjustCharactersPerLine(int qntCharactersPerLine, Enumerations.FontSize fontSize)
        {
            switch (fontSize)
            {
                case Enumerations.FontSize.Small:
                    qntCharactersPerLine += 17;
                    break;
                case Enumerations.FontSize.Large:
                    qntCharactersPerLine -= 24;
                    break;
            }

            return qntCharactersPerLine;
        }
    }
}
