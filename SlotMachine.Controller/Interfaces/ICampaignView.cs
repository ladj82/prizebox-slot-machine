using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlotMachine.Controller.Interfaces
{
    public interface ICampaignView : IView
    {
        void SetController(CampaignController oController);

        string InitInstruction { set; }
    }
}
