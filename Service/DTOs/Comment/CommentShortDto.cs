using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Comment
{
    public class CommentShortDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }

    }
}
