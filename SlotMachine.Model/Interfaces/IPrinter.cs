using SlotMachine.Utility;

namespace SlotMachine.Model.Interfaces
{
    public interface IPrinter
    {
        string Manufacturer { get; set; }

        string Name { get; set; }

        string Model { get; set; }

        Enumerations.ConnectionType ConnectionType { get; set; }

        string IpAddress { get; set; }
    }
}
