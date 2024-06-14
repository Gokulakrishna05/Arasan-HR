using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IHomeService
    {
        //DataTable IsQCNotify();
        //DataTable GetQCNotify();
        DataTable CuringGroup();
        DataTable curingsubgroup(string curingset);
        //DataTable GetMaterialnot();
        //DataTable GetAPout();
        DataTable GetquoteFollowupnextReport();
        DataTable Getinddentapprove();
        DataTable GetDamageGRN();
        DataTable GetIndent();
        DataTable GetDamageGRNDetail();
        DataTable GetMatDetail( );
        DataTable GetIssMatDetail( );
        DataTable GetMat(string id);
        DataTable GetEnqFollowupnextReport();
        DataTable GetSalesQuoteFollowupnextReport();
        DataTable GetCurInward();
        DataTable GetGRN(string id);
        DataTable GetCurInwardDoc(string id);
        DataTable GetMaterialnot();
        DataTable GetAllMenu(string userid);
    }
}
