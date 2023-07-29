using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppLibro.Models.Domain;

namespace AppLibro.Models.Dto
{
    public class LibroListVm
    {
        public IQueryable<Libro>? LibroList { get; set; }

        public int PageSize {get;set;}

        public int CurrentPage {get;set;}

        public int TotalPages { get; set; }

        public string? Term { get; set; }
    }
}