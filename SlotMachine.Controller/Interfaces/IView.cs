using System.Windows;
using System.Windows.Controls;

namespace SlotMachine.Controller.Interfaces
{
    public interface IView
    {
        UserControl UserControl { get; }

        void SetFocus();

        void SetEnabled(bool enabled);
    }
}
