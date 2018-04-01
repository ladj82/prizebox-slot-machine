using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlotMachine.Controller.Interfaces
{
    public interface ITryAgainView : IView
    {
        void SetController(TryAgainController oController);

        string HeaderMessage { set; }

        string TryAgainMessage { set; }
    }
}
