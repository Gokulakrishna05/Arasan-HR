﻿using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface 
{
    public interface IDrumLocation
    {
        IEnumerable<DrumLocation> GetAllDrumLocation( );
    }
}