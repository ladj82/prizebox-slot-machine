using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlotMachine.Controller.Interfaces
{
    public interface IUserGenderView : IView
    {
        void SetController(UserGenderController oController);

        string InitInstruction { set; }

        void ShowBackButton(bool bVisible);
    }
}
