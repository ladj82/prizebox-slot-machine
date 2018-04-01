using System.Drawing;

namespace SlotMachine.Controller.Interfaces
{
    public interface IShufflingView : IView
    {
        void SetController(ShufflingController oController);

        void SetSlotImage1(string sImagePath);

        void SetSlotImage2(string sImagePath);

        void SetSlotImage3(string sImagePath);

        void SetSlotStandardContainer();

        void SetSlotWinnerContainer();
    }
}
