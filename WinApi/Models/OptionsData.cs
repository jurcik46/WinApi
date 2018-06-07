﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Models
{
    class OptionsData
    {
        public string ApiLink { get; set; }
        public string Apikey { get; set; }
        public string UserID { get; set; }
        public string ObjecID { get; set; }
        public string ModuleID { get; set; }
        public string ProgramPath { get; set; }
        public string ProcessName { get; set; }
        public bool Succes { get; set; }
        public bool InProcess { get; set; }


        public override string ToString()
        {
            return base.ToString() + ": ApiLink - " + ApiLink + ", Apikey - " + Apikey + ", UserID - " + UserID + ", ModuleID - " + ModuleID + ", ProgramPath - "
                + ProgramPath + ", ProcessName - " + ProcessName + ", Succes - " + Succes + ", InProcess - " + InProcess;
        }
    }
}