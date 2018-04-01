using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SlotMachine.Controller;
using SlotMachine.Controller.Interfaces;

namespace SlotMachine.UI
{
    /// <summary>
    /// Interaction logic for InvoiceView.xaml
    /// </summary>
    public partial class InvoiceView : UserControl, IInvoiceView
    {
        private InvoiceController _oController;

        public InvoiceView()
        {
            InitializeComponent();
        }

        private void wndInvoiceReader_Loaded(object sender, RoutedEventArgs e)
        {
            _oController.InitView();
        }

        public UserControl UserControl { get { return this; } }

        public void SetFocus()
        {
            txtQrCode.Focus();
        }

        public void SetEnabled(bool enabled)
        {
            
        }

        public void SetController(InvoiceController oController)
        {
            _oController = oController;
        }

        public string InitInstruction { set { ucAlertLabel.SetMessage(value, true); } }

        private void txtQrCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQrCode.Text)) return;
            if (e.Key != Key.Return) return;

            var sQrCode = txtQrCode.Text;
            txtQrCode.Text = string.Empty;

            _oController.HandleInvoiceInput(sQrCode);
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
