﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal interface IStoreRepository
    {
        Dictionary<int, string> RetriveStores();
    }
}
