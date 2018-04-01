using System;

namespace SlotMachine.Model
{
    public class CheckExistingInvoiceRequest
    {
        public string HashQRCode { get; set; }
    }

    public class CheckExistingInvoiceResponse
    {
        public bool Result { get; set; }
    }
}
