using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string CreatedDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }  
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public string Content { get; set; }
    }
}
