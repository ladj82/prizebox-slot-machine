using System.Windows.Controls;

namespace SlotMachine.UI
{
    /// <summary>
    /// Interaction logic for AlertView.xaml
    /// </summary>
    public partial class AlertView : UserControl
    {
        public AlertView(string sMessage)
        {
            InitializeComponent();

            ucAlertLabel.SetMessage(sMessage, true);
        }
    }
}
