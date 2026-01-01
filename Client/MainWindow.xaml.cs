using Client.Models;
using Client.Services;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Parthners> parthners;  // ParthnersProductsResponse
        HttpCLientService httpService;
        private List<Products> productsTotal;
        public MainWindow()
        {
            InitializeComponent();

            httpService = new HttpCLientService("http://localhost:5070/api/");
            _ = LoadData();
        }


        private async void AddButtonClick(object sender, RoutedEventArgs e)
        {
            HideErrorMessage();

            AddWindow addWindow = new AddWindow(httpService, this);
            addWindow.Owner = this;
            addWindow.ShowDialog();
            await LoadData();
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            HideErrorMessage();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            HideErrorMessage();
        }


        public async Task LoadData()
        {
            parthners = await httpService.GetParthners();

            Dispatcher.Invoke(() =>
            {
                mainListBox.ItemsSource = null;
                mainListBox.ItemsSource = parthners;
            });
        }


        private void ShowErrorMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                ErrorMessage.Text = message;
                ErrorBorder.Visibility = Visibility.Visible;
            });
        }


        private void HideErrorMessage()
        {
            Dispatcher.Invoke(() =>
            {
                ErrorMessage.Text = string.Empty;
                ErrorBorder.Visibility = Visibility.Collapsed;
            });
        }
    }
}