﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Watchlist
{
    public class WatchlistCreateDto
    {
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
    }
}
