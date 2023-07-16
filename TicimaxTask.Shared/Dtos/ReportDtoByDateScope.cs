using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TicimaxTask.Shared.Dtos
{
    public class ReportDtoByDateScope
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string UserName { get; set; }
        public int InCount { get; set; }
        public int OutCount { get; set; }
        public TimeSpan? InTimeCount { get; set; }
    }
}
