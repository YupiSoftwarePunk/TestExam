using Client.Models;
using Client.Services;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private readonly HttpCLientService httpService;
        private readonly MainWindow mainWindow;
        private readonly Parthners partner;

        public EditWindow()
        {
            InitializeComponent();
            this.httpService = httpService;
            this.mainWindow = mainWindow;
            this.partner = partner;

            LoadPartnerTypes();
            LoadData();
        }


        private async void LoadPartnerTypes()
        {
            var types = await httpService.GetPartnerTypes();
            PartnerTypeInput.ItemsSource = types;
        }

        private void LoadData()
        {
            CompanyNameInput.Text = partner.ComapnyName;
            PhoneInput.Text = partner.PhoneNumber;
            AddressInput.Text = partner.Address;
            DirectorInput.Text = partner.DirectorFullName;
            EmailInput.Text = partner.Email;
            RatingInput.Text = partner.Rating.ToString();
            PartnerTypeInput.SelectedValue = partner.PartnerTypeId;
        }


        public async void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            partner.ComapnyName = CompanyNameInput.Text;
            partner.PhoneNumber = PhoneInput.Text;
            partner.Address = AddressInput.Text;
            partner.DirectorFullName = DirectorInput.Text;
            partner.Email = EmailInput.Text;
            partner.Rating = int.Parse(RatingInput.Text);
            partner.PartnerTypeId = (int)PartnerTypeInput.SelectedValue;

            bool ok = await httpService.UpdatePartner(partner.Id, partner);

            if (!ok)
            {
                MessageBox.Show("Ошибка при обновлении партнёра", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(CompanyNameInput.Text))
            {
                MessageBox.Show("Введите название компании", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(PhoneInput.Text, @"^\+?\d{10,15}$"))
            {
                MessageBox.Show("Некорректный номер телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(EmailInput.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Некорректный email", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(RatingInput.Text, out int rating) || rating < 0)
            {
                MessageBox.Show("Рейтинг должен быть целым неотрицательным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (PartnerTypeInput.SelectedValue == null)
            {
                MessageBox.Show("Выберите тип партнёра", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

    }
}