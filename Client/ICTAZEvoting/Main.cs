using ICTAZEvoting.WebUI;

using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

using MudBlazor.Services;

namespace ICTAZEVoting
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            var services = new ServiceCollection();
            services.AddMudServices();
            services.AddWindowsFormsBlazorWebView();
            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = services.BuildServiceProvider();
            blazorWebView1.RootComponents.Add<App>("#app");
        }

        private void blazorWebView1_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}