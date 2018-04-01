using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SlotMachine.Controller;
using SlotMachine.Controller.Interfaces;

namespace SlotMachine.UI
{
    /// <summary>
    /// Interaction logic for UserDocumentView.xaml
    /// </summary>
    public partial class UserDocumentView : UserControl, IUserDocumentView
    {
        private readonly KeyConverter _oKeyConverter;
        private UserDocumentController _oController;

        public UserDocumentView()
        {
            _oKeyConverter = new KeyConverter();

            InitializeComponent();
        }

        private void wndUserDocumentView_Loaded(object sender, RoutedEventArgs e)
        {
            _oController.InitView();
        }

        public UserControl UserControl { get { return this; } }
        
        public void SetFocus()
        {
            txtCpf.Text = string.Empty;
            txtCpf.Focus();
        }

        public void SetEnabled(bool enabled)
        {
            
        }

        public void SetController(UserDocumentController oController)
        {
            _oController = oController;
        }

        public string InitInstruction { set { ucAlertLabel.SetMessage(value, true); } }

        private void NumPad_OnVirtualNumberKeyBoardPressEvent(Key oPressedKey)
        {
            if (oPressedKey.Equals(Key.Escape))
            {
                _oController.CancelStep();
            }
            else
            {
                if (_oController.TimeoutHandler != null)
                {
                    _oController.TimeoutHandler.Stop();
                    _oController.TimeoutHandler.Start();
                }

                //var sInput = txtCpf.Text.Replace(" ", "").Replace(".", "").Replace("-", "");

                var sInput = txtCpf.Text;

                if (oPressedKey.Equals(Key.Back))
                {
                    if (sInput.Length > 0)
                        sInput = sInput.Substring(0, sInput.Length - 1);
                }
                else
                    sInput += _oKeyConverter.ConvertToString(oPressedKey);

                txtCpf.Text = sInput;

                _oController.HandleCpfInput(sInput);
            }
        }
    }
}
