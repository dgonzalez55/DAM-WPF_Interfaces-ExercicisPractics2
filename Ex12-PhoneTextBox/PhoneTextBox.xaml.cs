using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;

namespace Ex12_PhoneTextBox
{
    /// <summary>
    /// Lógica de interacción para PhoneTextBox.xaml
    /// </summary>
    public partial class PhoneTextBox : UserControl
    {
        // Propietat pública per la màscara       
        public string? Mask { get; set; }

        // Cadena que guarda els valors numèrics
        private string _currentNumericValue = "";

        public string CurrentNumericValue => _currentNumericValue;

        public PhoneTextBox()
        {
            InitializeComponent();
        }

        // Quan el control es carrega, aplica la màscara del XAML si està definida
        private void PhoneTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Mask))
            {
                Mask = "+34 ### ### ###"; // Definir màscara predeterminada només si no està definida
            }
            InitializePhoneTextBox();
        }

        // Inicialització del TextBox amb la màscara
        private void InitializePhoneTextBox()
        {
            TextBoxPhone.Text = Mask;
            SetCaretToNextHash();
        }

        // Validació d'entrada: només permet números
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text) || _currentNumericValue.Length >= Mask?.Count(c => c == '#'))
            {
                if(_currentNumericValue.Length < Mask?.Count(c => c == '#')) SetInputValid(false);
                e.Handled = true;
                return;
            }

            _currentNumericValue += e.Text;
            SetInputValid(true); // Marca l'entrada com a vàlida
            UpdateTextWithMask();
            SetCaretToNextHash();            
            e.Handled = true;
        }

        // Gestió de l'esborrat de números
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Back || e.Key == Key.Delete) && _currentNumericValue.Length > 0)
            {
                _currentNumericValue = _currentNumericValue.Substring(0, _currentNumericValue.Length - 1);
                UpdateTextWithMask();
                SetCaretToNextHash();
                e.Handled = true;
            }else if(e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = true;
            }
        }

        // Mètode per aplicar la màscara al valor actual
        private void UpdateTextWithMask()
        {
            int numericIndex = 0;
            TextBoxPhone.Text = new string(Mask?.Select(c =>
                c == '#' && numericIndex < _currentNumericValue.Length ? _currentNumericValue[numericIndex++] : c).ToArray());
        }

        // Mou el cursor a la següent posició amb '#'
        private void SetCaretToNextHash()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                int nextHashIndex = TextBoxPhone.Text.IndexOf('#');
                TextBoxPhone.CaretIndex = nextHashIndex != -1 ? nextHashIndex : TextBoxPhone.Text.Length;
            }), DispatcherPriority.Input);
        }

        private void SetInputValid(bool isValid)
        {
            if (isValid)
            {
                TextBoxPhone.Background = Brushes.White;
            }
            else
            {
                TextBoxPhone.Background = Brushes.LightCoral;
            }
            
        }

        // Valida si el caràcter és numèric
        private bool IsNumeric(string input) => input.All(char.IsDigit);

        // Col·loca el cursor sobre el primer hashtag quan el TextBox rep el focus
        private void OnGotFocus(object sender, RoutedEventArgs e) => SetCaretToNextHash();
    }
}