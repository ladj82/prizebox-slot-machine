using System.Windows.Controls;
using SlotMachine.Controller;
using SlotMachine.Controller.Interfaces;
using SlotMachine.UI.Components;

namespace SlotMachine.UI
{
    /// <summary>
    /// Interaction logic for TryAgainView.xaml
    /// </summary>
    public partial class TryAgainView : UserControl, ITryAgainView
    {
        private TryAgainController _oController;

        public TryAgainView()
        {
            InitializeComponent();
        }

        private void wndTryAgainView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _oController.InitView();
        }

        public UserControl UserControl { get { return this; } }
        
        public void SetFocus()
        {
            
        }

        public void SetEnabled(bool enabled)
        {
            
        }

        public void SetController(TryAgainController oController)
        {
            _oController = oController;
        }

        public string HeaderMessage
        {
            set
            {
                ucAlertLabelHeader.SetMessage(value, false, AlertLabel.MessageSize.XLarge);
            }
        }

        public string TryAgainMessage
        {
            set
            {
                ucAlertLabelTryAgainMessage.SetMessage(value, false, AlertLabel.MessageSize.XLarge);
            }
        }
    }
}
