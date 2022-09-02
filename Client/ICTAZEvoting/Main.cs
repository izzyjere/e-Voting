using ICTAZEVoting.Extensions;
using ICTAZEVoting.Services.Domain;
using ICTAZEVoting.Services.Identity;
using ICTAZEVoting.Services.Utility;
using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.WebUI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System.Globalization;

namespace ICTAZEVoting
{
    public partial class Main : Form
    {
        
        public Main()
        {
            InitializeComponent();                        
            ApplicationService.OnCloseClicked += OnClose;
            Task.Run(async()=> await SessionStorage.RemoveItemAsync("UserToken"));         
            var services = new ServiceCollection();
            services.AddMudServices(configuration =>
            {
                configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                configuration.SnackbarConfiguration.HideTransitionDuration = 1000;
                configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
                configuration.SnackbarConfiguration.VisibleStateDuration = 5000;
                configuration.SnackbarConfiguration.NewestOnTop = true;
                configuration.SnackbarConfiguration.MaximumOpacity = 100;
                configuration.SnackbarConfiguration.ShowCloseIcon = true;
                configuration.SnackbarConfiguration.ClearAfterNavigation = true;
            });
            services.AddTransient<AuthenticationHeaderHandler>()
                 .AddScoped(sp => sp
                   .GetRequiredService<IHttpClientFactory>()
                   .CreateClient("ICTAZEVotingSystem"))
               .AddHttpClient("ICTAZEVotingSystem", client =>
               {
                   client.DefaultRequestHeaders.AcceptLanguage.Clear();
                   client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
                   client.BaseAddress = new Uri("https://localhost:7119");
               })
               .AddHttpMessageHandler<AuthenticationHeaderHandler>();
            services.AddScoped<IVoterService, VoterService>();
            services.AddScoped<IVotingService, VotingService>();
            services.AddScoped<IElectionService, ElectionService>();
            services.AddScoped<ISystemAdminService, SystemAdminService>();
            services.AddScoped<IFacialRecognitionService, FacialRecognitionService>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IFileService, FileUploadService>();
            services.AddSingleton<NodeConnectionInstance>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();               
            services.AddAuthorizationCore()
            .AddScoped<IAuthenticationService, AuthenticationService>();       
            services.AddWindowsFormsBlazorWebView();
            services.AddBlazorWebViewDeveloperTools();           
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
        private void OnClose(object sender, EventArgs args)
        {
            Task.Run(async () => await SessionStorage.RemoveItemAsync("UserToken"));
            Application.Restart();
            Environment.Exit(0);
        }
       
    }

}