using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Favorite
{
    public class FavoriteCreateDto
    {
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
    }
}
