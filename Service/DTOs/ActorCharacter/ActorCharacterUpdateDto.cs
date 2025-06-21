using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.ActorCharacter
{
    public class ActorCharacterUpdateDto
    {
        public int? ActorId { get; set; }
        public string? CharacterName { get; set; }
    }
}
