﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Interfaces.Service
{
    public interface ISignatureService
    {
        bool InProcces { get; set; }
        void StartSign();
    }
}
