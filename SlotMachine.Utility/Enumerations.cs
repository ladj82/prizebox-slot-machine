namespace SlotMachine.Utility
{
    public class Enumerations
    {
        public enum AlertType
        {
            Warning,
            Error,
            Wait
        }

        public enum ConnectionType
        {
            Spool = 1,
            USB = 2,
            COM1 = 3,
            COM2 = 4,
            COM3 = 5,
            COM4 = 6,
            COM5 = 7,
            COM6 = 8,
            COM7 = 9,
            LPT1 = 10,
            LPT2 = 11,
            Remote = 12
        }

        public enum CutType
        {
            None = 1,
            Partial = 2,
            Full = 3
        }

        public enum TextAlignmentType
        {
            Left = 1,
            Center = 2,
            Right = 3
        }

        public enum BarcodeType
        {
            None = 1,
            CODE128 = 2
        }

        public enum FontSize
        {
            Small = 1,
            Medium = 2,
            Large = 3
        }

        public enum PrizeType
        {
            None,
            Double,
            Triple
        }
    }
}
