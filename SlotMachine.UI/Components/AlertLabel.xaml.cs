using System;
using System.Windows.Controls;

namespace SlotMachine.UI.Components
{
    /// <summary>
    /// Interaction logic for AlertLabel.xaml
    /// </summary>
    public partial class AlertLabel : UserControl
    {
        public AlertLabel()
        {
            InitializeComponent();
        }

        public enum MessageSize
        {
            Small,
            Medium,
            Large,
            XLarge
        }

        public void SetMessage(string sMessage, bool? bWarning = null, MessageSize? oSize = null)
        {
            grdContainer.Background.Opacity = bWarning.HasValue && bWarning.Value ? 0.15 : 0;

            if (oSize != null)
            {
                switch (oSize)
                {
                    case MessageSize.Small:
                        lblText.FontSize = 25;
                        break;

                    case MessageSize.Medium:
                        lblText.FontSize = 35;
                        break;

                    case MessageSize.Large:
                        lblText.FontSize = 40;
                        break;

                    case MessageSize.XLarge:
                        lblText.FontSize = 50;
                        break;
                }
            }

            lblText.Text = sMessage.Replace("{newline}", Environment.NewLine);
        }
    }
}
