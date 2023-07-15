using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicimaxTask.Shared.Dtos
{
    public class ReportDto
    {
        public string UserName { get; set; }
        public int InCount { get; set; }
        public int OutCount { get; set; }
        public TimeSpan InTimeCount { get; set; }

    }
}
