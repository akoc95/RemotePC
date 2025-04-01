using System.Net.Sockets;
using System.Text;

namespace Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadSettings();
        }
        private void LoadSettings()
        {
            IpEntry.Text = Preferences.Get("ServerIp", "192.168.1.100");
            PortEntry.Text = Preferences.Get("ServerPort", "5000");
        }

        private void SaveSettings_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("ServerIp", IpEntry.Text);
            Preferences.Set("ServerPort", PortEntry.Text);
            LoadSettings();
            DisplayAlert("Başarılı", "Ayarlar Kaydedildi!", "Tamam");
        }

        private async void VolumeDown_Clicked(object sender, EventArgs e) => await SendCommand("volume_down");
        private async void VolumeUp_Clicked(object sender, EventArgs e) => await SendCommand("volume_up");
        private async void Shutdown_Clicked(object sender, EventArgs e) => await SendCommand("shutdown");

        private async void Url_Clicked(object sender, EventArgs e)
        {
            string url = UrlEntry.Text;
            if (string.IsNullOrEmpty(url))
            {
                await DisplayAlert("Hata", "Lütfen geçerli bir URL girin.", "Tamam");
                return;
            }

            await SendCommand($"url:{url}");
        }

        private async Task SendCommand(string command)
        {
            string serverIp = Preferences.Get("ServerIp", "192.168.1.100");
            int serverPort = int.Parse(Preferences.Get("ServerPort", "5000"));

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(serverIp, serverPort);
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(command);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bağlantı hatası: {ex.Message}", "Tamam");
            }
        }
    }
}
