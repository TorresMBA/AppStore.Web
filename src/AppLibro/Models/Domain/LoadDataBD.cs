using Microsoft.AspNetCore.Identity;

namespace AppLibro.Models.Domain
{
    public class LoadDataBD
    {
        public static async Task InsertData(DatabaseContext context, 
                                            UserManager<ApplicationUser> userManager, 
                                            RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
            
            if (!userManager.Users.Any())
            {
                var usuario = new ApplicationUser{
                    Nombre = "Generico",
                    UserName = "admin.bri",
                    Email = "admin@gmail.com",
                };

                await userManager.CreateAsync(usuario, "Password123$!!");
                await userManager.AddToRoleAsync(usuario, "ADMIN");
            }

            if (!context.Categorias!.Any())
            {
                await context.Categorias!.AddRangeAsync(
                    new Categoria { Nombre = "Drama" },
                    new Categoria { Nombre = "Comedia" },
                    new Categoria { Nombre = "Acción" },
                    new Categoria { Nombre = "Terror" },
                    new Categoria { Nombre = "Aventura" }
                );

                await context.SaveChangesAsync();
            }

            if (!context.Libros!.Any())
            {
                await context.Libros!.AddRangeAsync(
                    new Libro { 
                        Titulo = "El quijote de la mancha",
                        CreateDate = "06/06/2020",
                        Imagen = "quijote.jpg",
                        Autor = "Miguel de Cervantes"
                    },
                    new Libro { 
                        Titulo = "Harry Potter",
                        CreateDate = "06/01/2021",
                        Imagen = "harry.jpg",
                        Autor = "Juan de la Vega"
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!context.LibroCategorias!.Any())
            {
                await context.LibroCategorias!.AddRangeAsync(
                    new LibroCategoria { CategoriaId = 1, LibroId = 1 },
                    new LibroCategoria { CategoriaId = 1, LibroId = 2 }
                );
                
                await context.SaveChangesAsync();
            }

        }
    }
}