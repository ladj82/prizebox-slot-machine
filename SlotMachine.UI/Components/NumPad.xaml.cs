using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SlotMachine.UI.Components
{
    /// <summary>
    /// Interaction logic for NumPad.xaml
    /// </summary>
    public partial class NumPad : UserControl
    {
        public delegate void OnVirtualNumberKeyBoardPress(Key pressedKey);
        public event OnVirtualNumberKeyBoardPress OnVirtualNumberKeyBoardPressEvent;

        public NumPad()
        {
            InitializeComponent();
        }

        private void ParseButtonClick(Button oButton)
        {
            switch (oButton.Content.ToString())
            {
                case "1":
                    OnVirtualNumberKeyBoardPressEvent(Key.D1);
                    break;
                case "2":
                    OnVirtualNumberKeyBoardPressEvent(Key.D2);
                    break;
                case "3":
                    OnVirtualNumberKeyBoardPressEvent(Key.D3);
                    break;
                case "4":
                    OnVirtualNumberKeyBoardPressEvent(Key.D4);
                    break;
                case "5":
                    OnVirtualNumberKeyBoardPressEvent(Key.D5);
                    break;
                case "6":
                    OnVirtualNumberKeyBoardPressEvent(Key.D6);
                    break;
                case "7":
                    OnVirtualNumberKeyBoardPressEvent(Key.D7);
                    break;
                case "8":
                    OnVirtualNumberKeyBoardPressEvent(Key.D8);
                    break;
                case "9":
                    OnVirtualNumberKeyBoardPressEvent(Key.D9);
                    break;
                case "0":
                    OnVirtualNumberKeyBoardPressEvent(Key.D0);
                    break;
                case "Limpar":
                    OnVirtualNumberKeyBoardPressEvent(Key.Back);
                    break;
                case "Cancelar":
                    OnVirtualNumberKeyBoardPressEvent(Key.Escape);
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParseButtonClick((Button)sender);
        }
    }
}
