using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.MAP.NewFolder;

namespace TicimaxTask.MAP.DataBaseConfigurations
{
    public class CheckInOutConfiguration:BaseConfiguration<CheckInOut>
    {
        public override void Configure(EntityTypeBuilder<CheckInOut> builder)
        {
            base.Configure(builder);
        }
    }
}
