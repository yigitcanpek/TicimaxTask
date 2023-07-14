﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Enums;

namespace TicimaxTask.Entities.Entities.Models
{
    public class CheckInOut:BaseEntity
    {
        public CheckInOut()
        {
            CheckTime = DateTime.Now;
        }

        public DateTime CheckTime { get; private set; }

        public CheckStatus CheckType { get; set; }


        //Relational Properties
        public AppUser AppUser { get; set; }

        public int AppUserID { get; set; }
    }
}
