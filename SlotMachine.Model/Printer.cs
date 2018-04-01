using SlotMachine.Model.Interfaces;
using SlotMachine.Utility;

namespace SlotMachine.Model
{
    public class Printer : IPrinter
    {
        public string Manufacturer { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public Enumerations.ConnectionType ConnectionType { get; set; }

        public string IpAddress { get; set; }
    }
}
