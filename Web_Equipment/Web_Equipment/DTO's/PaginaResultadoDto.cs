using Web_Equipment.Models;

namespace Web_Equipment.DTO_s
{
        public class PaginaResultadoDto
        {
            public List<Equipment> Items { get; set; } = new();

            public int Total { get; set; }

            public int Pagina { get; set; }

            public int Tamano { get; set; }

            public int TotalPaginas { get; set; }
        }
}
