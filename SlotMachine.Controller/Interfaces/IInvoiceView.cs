namespace SlotMachine.Controller.Interfaces
{
    public interface IInvoiceView : IView
    {
        void SetController(InvoiceController oController);

        string InitInstruction { set; }
    }
}
