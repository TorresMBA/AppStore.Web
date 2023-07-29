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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
