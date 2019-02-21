using System.Windows;

namespace MessagesSender.Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Client Client;
        public MainWindow()
        {
            InitializeComponent();
            Client = new Client();
            if (Client.Start())
                lblStatus.Content = "Подключение к серверу установлено";
        }
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Client.Start())
                lblStatus.Content = "Подключение к серверу установлено";
            else
                MessageBox.Show("Не удалось подключиться к серверу");
        }
    }
}