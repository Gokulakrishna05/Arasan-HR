﻿using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IPackDrumAllocation
    {
       
        DataTable GetLoc();
        DataTable GetAllPackDerum();
        DataTable GetDetails(string id);
        DataTable GetPackDrum(string id);
        DataTable EditDrumDetail(string id);

        string PackDrumCRUD(PackDrumAllocation Cy);
    }
}