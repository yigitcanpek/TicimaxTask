﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicimaxTask.Shared.DTO_s
{
    public class ErrorDto
    {
        public List<string> Errors { get; set; } = new List<string>();

        public bool IsShow { get; private set; }



        public ErrorDto(string error, bool isShow)
        {

            Errors.Add(error);
            IsShow = isShow;
        }

        public ErrorDto(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }
    }
}