using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ex09_CounterButton
{
    /// <summary>
    /// Lógica de interacción para CounterButton.xaml
    /// </summary>
    public partial class CounterButton : UserControl
    {
        public CounterButton()
        {
            InitializeComponent();
        }

        private int _counter = 0;
        private string _label = "Clicks: ";

        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                updateButtonText();
            }
        }
        public int Counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
                updateButtonText();
            }
        }

        private void updateButtonText() => counterButton.Content = _label + _counter.ToString();
        private void counterButton_Click(object sender, RoutedEventArgs e) => Counter++;
    }
}
