﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatiCsharp.Interfaces
{
    public interface IPage: ISite
    {
        /// Hierachy of the page. (i.e. "dev" for rolandbraun.com/dev)
        string Hierarchy { get; set; }
    }
}
