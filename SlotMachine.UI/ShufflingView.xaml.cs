using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SlotMachine.Controller;
using SlotMachine.Controller.Interfaces;

namespace SlotMachine.UI
{
    /// <summary>
    /// Interaction logic for ShufflingView.xaml
    /// </summary>
    public partial class ShufflingView : UserControl, IShufflingView
    {
        private ShufflingController _oController;

        public ShufflingView()
        {
            InitializeComponent();
        }

        private void wndShufflingView_Loaded(object sender, RoutedEventArgs e)
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

        public void SetController(ShufflingController oController)
        {
            _oController = oController;
        }

        public void SetSlotImage1(string sImagePath)
        {
            Dispatcher.Invoke((Action) (() =>
            {
                slotImg1.Children.Clear();
                slotImg1.Children.Add(GetSlotImage(sImagePath));
            }));
        }

        public void SetSlotImage2(string sImagePath)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                slotImg2.Children.Clear();
                slotImg2.Children.Add(GetSlotImage(sImagePath));
            }));
        }

        public void SetSlotImage3(string sImagePath)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                slotImg3.Children.Clear();
                slotImg3.Children.Add(GetSlotImage(sImagePath));
            }));
        }

        private UIElement GetSlotImage(string sImagePath)
        {
            var oImage = new Image();
            var oBitmapImage = new BitmapImage();

            oBitmapImage.BeginInit();
            oBitmapImage.UriSource = new Uri(sImagePath);
            oBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            oBitmapImage.EndInit();
            oImage.Source = oBitmapImage;
            oImage.Stretch = Stretch.Uniform;

            return oImage;
        }

        public void SetSlotStandardContainer()
        {
            Dispatcher.Invoke((Action)(() =>
            {
                grdSlotContainer.Background = new ImageBrush(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/App/imgSlotBackground1.png")));
            }));
        }

        public void SetSlotWinnerContainer()
        {
            Dispatcher.Invoke((Action)(() =>
            {
                grdSlotContainer.Background = new ImageBrush(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/App/imgSlotBackground2.png")));
            }));
        }
    }
}
