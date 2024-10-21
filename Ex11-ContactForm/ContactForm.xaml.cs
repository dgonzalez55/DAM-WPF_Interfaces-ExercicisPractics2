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

namespace Ex11_ContactForm
{
    /// <summary>
    /// Lógica de interacción para ContactForm.xaml
    /// </summary>
    public partial class ContactForm : UserControl
    {
        public ContactForm()
        {
            InitializeComponent();
        }

        // Mètode que s'executa cada vegada que es canvia el text en qualsevol camp
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateForm();
        }

        // Mètode que valida el formulari en temps real
        private void ValidateForm()
        {
            bool isValid = true;

            // Validació del Nom (almenys 3 caràcters)
            if (NomTextBox.Text.Length < 3)
            {
                NomError.Opacity = 1; // Mostra el missatge d'error
                isValid = false;
            }
            else
            {
                NomError.Opacity = 0; // Amaga el missatge d'error
            }

            // Validació del Telèfon (exactament 9 dígits i només números)
            if (!Regex.IsMatch(TelefonTextBox.Text, @"^\d{9}$"))
            {
                TelefonError.Opacity = 1; // Mostra el missatge d'error
                isValid = false;
            }
            else
            {
                TelefonError.Opacity = 0; // Amaga el missatge d'error
            }

            // Validació del Correu Electrònic (format correcte)
            if (!IsValidEmail(EmailTextBox.Text))
            {
                EmailError.Opacity = 1; // Mostra el missatge d'error
                isValid = false;
            }
            else
            {
                EmailError.Opacity = 0; // Amaga el missatge d'error
            }

            // Activar o desactivar el botó d'enviament segons la validació
            EnviarButton.IsEnabled = isValid;
        }

        // Mètode per validar el format del correu electrònic
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        // Mètode que s'executa quan es prem el botó "Enviar"
        private void EnviarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Formulari enviat correctament!", "Èxit", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}