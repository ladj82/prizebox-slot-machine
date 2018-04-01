using System;

namespace SlotMachine.Model
{
    public class CheckExistingStoreRequest
    {
        public string Cnpj { get; set; }
    }

    public class CheckExistingStoreResponse
    {
        public bool Result { get; set; }
    }
}
