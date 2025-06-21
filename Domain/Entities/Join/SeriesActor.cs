using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Join
{
    public class SeriesActor
    {
        public int SeriesId { get; set; }
        public Series Series { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }

        public string CharacterName { get; set; }
    }
}
