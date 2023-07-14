using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IHomeService
    {
        DataTable IsQCNotify();
        DataTable GetQCNotify();
        DataTable CuringGroup();
        DataTable curingsubgroup(string curingset);
        DataTable GetMaterialnot();

        DataTable GetquoteFollowupnextReport();
        DataTable GetEnqFollowupnextReport();
        DataTable GetSalesQuoteFollowupnextReport();
    }
}
