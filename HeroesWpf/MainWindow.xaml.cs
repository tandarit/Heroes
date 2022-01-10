using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

namespace HeroesWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _client = new HttpClient();
        

        public MainWindow()
        {
            InitializeComponent();
            _client.Timeout = TimeSpan.FromSeconds(2.0);
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            try
            {
                HttpResponseMessage response = await _client.GetAsync("https://localhost:7019/api/Heroes/Health");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                ServerHealth serverStatus = JsonSerializer.Deserialize<ServerHealth>(responseBody, options);

                if (serverStatus!=null && response.StatusCode == System.Net.HttpStatusCode.OK)
                    MessageBox.Show($"HeroAPI Server {serverStatus.Version} is working!");
                else
                    MessageBox.Show($"HeroAPI Server is NOT working!");

                tbHeroesList.Text = responseBody;               
            }
            catch (HttpRequestException ex)
            {
                tbHeroesList.Text = ex.Message;
            }
            catch (TimeoutException timeoutEx)
            {
                tbHeroesList.Text = timeoutEx.Message;
            }
            catch (Exception exception)
            {
                tbHeroesList.Text = exception.Message;
            }

        }
    }
}
