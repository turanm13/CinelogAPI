using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Account
{
    public class RoleDto
    {
        public string Name { get; set; }
        public IEnumerable<UsersInRolesDto> Users { get; set; }
    }
}
