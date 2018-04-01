using System;
using System.Windows.Controls;
using SlotMachine.Controller;
using SlotMachine.Controller.Interfaces;
using SlotMachine.UI.Components;

namespace SlotMachine.UI
{
    /// <summary>
    /// Interaction logic for PrintView.xaml
    /// </summary>
    public partial class PrintView : UserControl, IPrintView
    {
        private PrintController _oController;

        public PrintView()
        {
            InitializeComponent();
        }

        private void wndPrintView_Loaded(object sender, System.Windows.RoutedEventArgs e)
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

        public void SetController(PrintController oController)
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

        public string SubHeaderMessage
        {
            set
            {
                ucAlertLabelSubHeader.SetMessage(value);
            }
        }

        public string StoreName
        {
            set
            {
                ucAlertLabelStoreName.SetMessage(value, true, AlertLabel.MessageSize.Large);
            }
        }

        public string TakeCupomMessage
        {
            set
            {
                ucAlertLabelTakeCupomMessage.SetMessage(value, false, AlertLabel.MessageSize.Small);
            }
        }
    }
}
