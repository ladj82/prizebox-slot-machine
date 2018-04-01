using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SlotMachine.Controller;
using SlotMachine.Controller.Interfaces;

namespace SlotMachine.UI
{
    /// <summary>
    /// Interaction logic for UserGenderView.xaml
    /// </summary>
    public partial class UserGenderView : UserControl, IUserGenderView
    {
        private UserGenderController _oController;

        public UserGenderView()
        {
            InitializeComponent();
        }

        private void wndUserGenderView_Loaded(object sender, RoutedEventArgs e)
        {
            _oController.InitView();
        }

        public UserControl UserControl { get { return this; } }
        
        public void SetFocus()
        {
            Gender = string.Empty;
        }

        public void SetEnabled(bool enabled)
        {
            
        }

        public void SetController(UserGenderController oController)
        {
            _oController = oController;
        }

        public string InitInstruction { set { ucAlertLabel.SetMessage(value, true); } }

        public void ShowBackButton(bool bVisible)
        {
            btnBack.Visibility = bVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private void btnGenderM_Click(object sender, RoutedEventArgs e)
        {
            Gender = "M";
        }

        private void btnGenderF_Click(object sender, RoutedEventArgs e)
        {
            Gender = "F";
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _oController.HandleGenderInput(Gender);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _oController.BackStep();
        }

        private string _sGender;

        public string Gender
        {
            get
            {
                return _sGender;
            }
            set
            {
                _sGender = value;

                if (_sGender.Equals("M"))
                {
                    btnGenderM.Style = (Style)grdCheckBox.Resources["CheckedButtonStyle"];
                    btnGenderF.Style = (Style)grdCheckBox.Resources["UnCheckedButtonStyle"];
                }
                else if (_sGender.Equals("F"))
                {
                    btnGenderM.Style = (Style)grdCheckBox.Resources["UnCheckedButtonStyle"];
                    btnGenderF.Style = (Style)grdCheckBox.Resources["CheckedButtonStyle"];
                }
                else
                {
                    btnGenderM.Style = (Style)grdCheckBox.Resources["UnCheckedButtonStyle"];
                    btnGenderF.Style = (Style)grdCheckBox.Resources["UnCheckedButtonStyle"];
                }
            }
        }
    }
}
