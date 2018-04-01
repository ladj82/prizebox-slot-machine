using System.Windows;
using System.Windows.Controls;
using SlotMachine.Controller;
using SlotMachine.Controller.Interfaces;

namespace SlotMachine.UI
{
    /// <summary>
    /// Interaction logic for CampaignView.xaml
    /// </summary>
    public partial class CampaignView : UserControl, ICampaignView
    {
        private CampaignController _oController;

        public CampaignView()
        {
            InitializeComponent();
        }

        private void wndCampaignView_Loaded(object sender, RoutedEventArgs e)
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

        public void SetController(CampaignController oController)
        {
            _oController = oController;
        }

        public string InitInstruction { set { ucAlertLabel.SetMessage(value, true); } }
    }
}
