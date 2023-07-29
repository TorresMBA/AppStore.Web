using AppLibro.Models.Domain;
using AppLibro.Models.Dto;

namespace AppLibro.Repositories.Abstract
{
    public interface ILibroService
    {
        bool Add(Libro libro);

        bool Update(Libro libro);

        Libro GetById(int id);

        bool Delete(int id);

        LibroListVm List(string term ="", bool paging = false, int currentPage = 0);

        List<int> GetCategoriaByLibroId(int libroId);
    }
}