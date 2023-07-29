using AppLibro.Models.Domain;
using AppLibro.Repositories.Abstract;
using AppLibro.Repositories.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
/**
A este archivo también se le conoce como "Contenedor de Dependencias" o 
"Contendor de Servicios"
Porque muchas clases o servicios puede ser incializadas al interior del
"Program.cs" al ser este la primera clase o componente que se ejecuta
dentro la aplicación esto se aprovecha para incializar o crear objetos
que se necesitan para algunas transacciones o funcionalidad o servicio en 
particular que se necesita en el proyecto.
*/
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Inyección para ILibroService
builder.Services.AddScoped<ILibroService, LibroService>();
#endregion

#region Conexión a la Base de Datos + Logger
builder.Services.AddDbContext<DatabaseContext>(optionsAction => {
    //Imprimirá en consola cada consulta que se haga a la base de datos
    optionsAction.LogTo(Console.WriteLine, new [] {
        DbLoggerCategory.Database.Command.Name
    }, LogLevel.Information).EnableServiceProviderCaching();

    //Se inicializa y se le pasa la cadena al DataBaseContext
    optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("SqlConexion"));
});
#endregion

#region Configuración al parecer de Identity - Clase 99 ASP.NET Core MVC and C# Bootcamp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
#endregion

//Encargado de ejecutar el archivo Program.cs
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

#region Login
//Con esto aañadido va a ser posible trabajar Login y generación de token o cookie
//que mantenga la sesión  en la pagina del cliente - clase 99
app.UseAuthentication();
#endregion

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#region Cargar data de BD inicial
using(var ambiente = app.Services.CreateScope()){
    var services = ambiente.ServiceProvider;

    try{
        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        //Inicio - Ejecuta la migración es decir solo ejecuta los archivos de migración ya creados
        //no crea ningún archivo de migración
        await context.Database.MigrateAsync();
        //Fin

        await LoadDataBD.InsertData(context, userManager, roleManager);
    }catch(Exception ex){
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(ex, "Ocurrio un error en la inserción de datos inicial.");
    }
}
#endregion

app.Run();