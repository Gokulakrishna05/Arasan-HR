using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IQcDashboardService
    {
        DataTable IsQCNotify();
        DataTable GetQCNotify();
        DataTable CuringGroup();
        DataTable curingsubgroup(string curingset);
        //DataTable GetMaterialnot();
        DataTable GetAPout();
        DataTable GetAPout1(string id);
        DataTable GetPout();
        DataTable GetDis(string id);
        DataTable GetAPoutItem();
        DataTable GetGRNItem();
        DataTable GetFinal(string id);
        //DataTable GetquoteFollowupnextReport();
        //DataTable GetEnqFollowupnextReport();
        //DataTable GetSalesQuoteFollowupnextReport();
    }
}
