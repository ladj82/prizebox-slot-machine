namespace SlotMachine.Model
{
    public class Invoice
    {
        public string QrCode { get; set; }

        public string HashQrCode { get; set; }

        public string Cpf { get; set; }

        public string Cnpj { get; set; }

        public decimal Value { get; set; }

        public decimal Icms { get; set; }
    }
}
