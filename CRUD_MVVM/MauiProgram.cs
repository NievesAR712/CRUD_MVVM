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

            // Configuración del contexto de la base de datos (UsuarioDBContext)
            var dbContext = new UsuarioDBContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

            builder.Services.AddDbContext<UsuarioDBContext>();

            builder.Services.AddSingleton<IRepository<Usuario>, Repository<Usuario>>();
            builder.Services.AddSingleton<UsuarioRepository>();

            // Registro de los servicios para la inyección de dependencias
            builder.Services.AddScoped<UsuarioRepository, UsuarioRepository>(); // Registrar el repositorio

            // Registro de las páginas y ViewModels
            builder.Services.AddTransient<UsuarioPage>();
            builder.Services.AddTransient<UsuarioViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            Routing.RegisterRoute(nameof(UsuarioPage), typeof(UsuarioPage));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
