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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private readonly HttpCLientService httpService;
        private readonly MainWindow mainWindow;

        public AddWindow(Services.HttpCLientService httpService, MainWindow mainWindow)
        {
            InitializeComponent();
            this.httpService = httpService;
            this.mainWindow = mainWindow;

            LoadPartnerTypes();

        }

        private async void LoadPartnerTypes()
        {
            var types = await httpService.GetPartnerTypes();
            PartnerTypeInput.ItemsSource = types;
        }



        public async void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            var partner = new Parthners
            {
                ComapnyName = CompanyNameInput.Text,
                PhoneNumber = PhoneInput.Text,
                Address = AddressInput.Text,
                DirectorFullName = DirectorInput.Text,
                Email = EmailInput.Text,
                Rating = int.Parse(RatingInput.Text),
                PartnerTypeId = (int)PartnerTypeInput.SelectedValue
            };

            bool ok = await httpService.AddPartner(partner);

            if (!ok)
            {
                MessageBox.Show("Ошибка при добавлении партнёра", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Партнёр успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
