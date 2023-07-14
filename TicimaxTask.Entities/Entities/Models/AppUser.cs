using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Enums;

namespace TicimaxTask.Entities.Entities.Models
{
    public class AppUser:BaseEntity
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public CheckStatus? CheckStatus { get; set; }

        public List<CheckInOut> Checks { get; set; }
    }
}
