using CRUD_MVVM.DataAccess;
using CRUD_MVVM.Modelos;
using CRUD_MVVM.ViewModels;
using CRUD_MVVM.Views;
using Microsoft.Extensions.Logging;
using static CRUD_MVVM.DataAccess.UsuarioRepository;

namespace CRUD_MVVM
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            

            builder.Services.AddSingleton<IRepository<Usuario>, Repository<Usuario>>();
            builder.Services.AddSingleton<UsuarioRepository>();

            // Registro de los servicios para la inyección de dependencias
            builder.Services.AddScoped<UsuarioRepository, UsuarioRepository>(); // Registrar el repositorio

            // Registro de las páginas y ViewModels
            builder.Services.AddTransient<VerPedidosPage>();
            builder.Services.AddTransient<VerPedidosViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            builder.Services.AddSingleton<UsuarioRepository>();
            builder.Services.AddTransient<UsuarioViewModel>();
            builder.Services.AddTransient<UsuarioPage>();

            builder.Services.AddSingleton<PedidoRepository>();
            builder.Services.AddTransient<PedidoViewModel>();
            builder.Services.AddTransient<PedidoPage>();

            builder.Services.AddSingleton<UsuarioRepository>();  // Inyecta el UsuarioRepository
            builder.Services.AddSingleton<MainViewModel>();


            Routing.RegisterRoute("PedidoPage", typeof(PedidoPage));
            Routing.RegisterRoute("UsuariosPage", typeof(UsuarioPage));
            Routing.RegisterRoute("VerPedidosPage", typeof(VerPedidosPage));
            Routing.RegisterRoute(nameof(UsuarioPage), typeof(UsuarioPage));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
