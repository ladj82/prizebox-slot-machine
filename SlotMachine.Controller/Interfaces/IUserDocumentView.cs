namespace SlotMachine.Controller.Interfaces
{
    public interface IUserDocumentView : IView
    {
        void SetController(UserDocumentController oController);

        string InitInstruction { set; }
    }
}
