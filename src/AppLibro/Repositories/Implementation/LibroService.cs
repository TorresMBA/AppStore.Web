using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppLibro.Models.Domain;
using AppLibro.Models.Dto;
using AppLibro.Repositories.Abstract;

namespace AppLibro.Repositories.Implementation
{
    public class LibroService : ILibroService
    {
        private readonly DatabaseContext _context;

        public LibroService(DatabaseContext context)
        {
            _context = context;
        }

        public bool Add(Libro libro)
        {
            try
            {
                _context.Add(libro);
                _context.SaveChanges();

                foreach (var item in libro.Categorias!)
                {
                    var LibroCategoria = new LibroCategoria(){
                        LibroId = libro.Id,
                        CategoriaId = item
                    };

                    _context.LibroCategorias!.Add(LibroCategoria);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = GetById(id);
                if (data is null) return false;

                var libroCategorias = _context.LibroCategorias!.Where(c => c.Id == id).ToList();
                _context.LibroCategorias!.RemoveRange(libroCategorias);
                _context.Libros!.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public Libro GetById(int id)
        {
            return _context.Libros!.Find(id)!;
        }

        public List<int> GetCategoriaByLibroId(int libroId)
        {
            return _context.LibroCategorias!.Where(x => x.LibroId == libroId).Select(a => a.CategoriaId).ToList();
        }

        public LibroListVm List(string term = "", bool paging = false, int currentPage = 0)
        {
            var data = new LibroListVm();

            var list = _context.Libros!.ToList();

            if (!string.IsNullOrEmpty(term)){
                term = term.ToLower();
                list = list.Where(x => x.Titulo!.ToLower().StartsWith(term)).ToList();
            }

            if(paging){
                int pageZise = 5;
                int count = list.Count;

                int totalPages = (int)Math.Ceiling(count / (double)pageZise);
                list = list.Skip((currentPage-1)*pageZise).Take(totalPages).ToList();
                data.PageSize = pageZise;
                data.CurrentPage = currentPage;
                data.TotalPages = totalPages;
            }

            foreach (var item in list)
            {
                var cateegorias = (
                    from categoria in _context.Categorias
                    join lc in _context.LibroCategorias!
                    on categoria.Id equals lc.CategoriaId
                    where lc.LibroId == item.Id
                    select categoria.Nombre
                ).ToList();

                string categoriaNombre = string.Join(",", cateegorias);

                item.CategoriasName = categoriaNombre;
            }

            data.LibroList = list.AsQueryable();
            return data;
        }

        public bool Update(Libro libro)
        {
            try
            {
                var categoriasParaEliminar = _context.LibroCategorias!.Where(x => x.LibroId == libro.Id);

                foreach (var categoriaPara in categoriasParaEliminar){
                    _context.LibroCategorias!.Remove(categoriaPara);
                }

                foreach (var categoriaId in libro.Categorias!)
                {
                    var LibroCategoria = new LibroCategoria(){
                        CategoriaId = categoriaId,
                        LibroId = libro.Id
                    };

                    _context.LibroCategorias!.Add(LibroCategoria);                    
                }

                _context.Libros!.Update(libro);

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}