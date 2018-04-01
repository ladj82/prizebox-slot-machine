using System;

namespace SlotMachine.Model
{
    public class Command
    {
        public Command(string sName, Func<int> oFunction)
        {
            Name = sName;
            Function = oFunction;
        }

        public string Name { get; set; }

        public Func<int> Function { get; set; }
    }
}
