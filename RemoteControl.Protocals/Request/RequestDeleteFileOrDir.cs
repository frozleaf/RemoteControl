﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public class RequestDeleteFileOrDir
    {
        public ePathType PathType;
        public string Path;
    }
}
