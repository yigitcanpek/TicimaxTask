using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Enums;

namespace TicimaxTask.Entities.Entities.Models
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
          
            Status = DataStatus.Inserted;
        }

        public int ID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }


        public DataStatus Status { get; set; }

      
    }
}
