namespace SlotMachine.Controller.Interfaces
{
    public interface IPrintView : IView
    {
        void SetController(PrintController oController);

        string HeaderMessage { set; }

        string SubHeaderMessage { set; }

        string StoreName { set; }

        string TakeCupomMessage { set; }
    }
}
