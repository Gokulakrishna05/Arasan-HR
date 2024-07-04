
using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.InkML;

namespace Arasan.Models
{
    public class Machine
    {
        public Machine()
        {
            this.LocLst = new List<SelectListItem>();
            this.WrkCentlst = new List<SelectListItem>();
            this.SubProclst = new List<SelectListItem>();
            this.MMadelst = new List<SelectListItem>();
            this.Purlst = new List<SelectListItem>();
            this.MMTypelst = new List<SelectListItem>();
            this.MMModelst = new List<SelectListItem>();
            //this.Ledgerlst = new List<SelectListItem>();
            //this.Tarrifflst = new List<SelectListItem>();
            //this.purlst = new List<SelectListItem>();
            //this.costlst = new List<SelectListItem>();
            ////this.Itemlst = new List<SelectListItem>();

        }
        public string ID { get; set; }
        public string MId { get; set; }
        public string MName { get; set; }

        public string MLoc { get; set; }
        public List<SelectListItem> LocLst;

        public string MWrkCent { get; set; }
        public List<SelectListItem> WrkCentlst;

        public string MSupply { get; set; }

        public string MSerial { get; set; }
        public string MModel { get; set; }
        public string MManname { get; set; }

        public string MSubProc { get; set; }
        public List<SelectListItem> SubProclst;

        public string MMade { get; set; }
        public List<SelectListItem> MMadelst;

        public string MPur { get; set; }
        public List<SelectListItem> Purlst;
        public string MSer { get; set; }
        public string MSerCmp { get; set; }
        public string MIncharge { get; set; }

        public string MMType { get; set; }
        public List<SelectListItem> MMTypelst;

        public string Aux { get; set; }
        public string MUse { get; set; }

        public string MMMode { get; set; }
        public List<SelectListItem> MMModelst;

        public string MCap { get; set; }
        public string MCapUnit { get; set; }
        public List<SelectListItem> MCaplst;

        public string MELife { get; set; }
        public string MELifeUnit { get; set; }
        public List<SelectListItem> MELifeLst;

        public string MPower { get; set; }
        public string MPowerUnit { get; set; }
        public List<SelectListItem> MPowerlst;

        public string MPFactor { get; set; }

        public string MRun { get; set; }
        public string MRunUnit { get; set; }
        public List<SelectListItem> MRunlst;

        public string MRunH { get; set; }
        public string MRunHUnit { get; set; }
        public List<SelectListItem> MRunHLst;

        public string MLead { get; set; }
        public string MLeadUnit { get; set; }
        public List<SelectListItem> MLeadlst;

        public string MMaintain { get; set; }
        public string MMaintainUnit { get; set; }
        public List<SelectListItem> MMaintainLst;

        public string DOP { get; set; }
        public string DOS { get; set; }

        public string InsDate { get; set; }

        public string DOLMain { get; set; }
        public string NMainDate { get; set; }
        public string MLCost { get; set; }
        public string Dep { get; set; }
        public string Int { get; set; }
        public string SOYear { get; set; }
        public string MCYear { get; set; }
        public string Rent { get; set; }
        public string Tot { get; set; }
        public string PCUnit { get; set; }
        public string AvgCost { get; set; }
        public string FixCost { get; set; }
        public string CostRH { get; set; }
        public string PUtilize { get; set; }
        public string DepValue { get; set; }
        public string IntValue { get; set; }
        public string InsValue { get; set; }
        public string PCostH { get; set; }
        public string AddMCost { get; set; }



        public string ddlStatus { get; set; }


        public List<Compdetails> Complst { get; set; }
        public List<Majorpart> Majorlst { get; set; }
        public List<Checklistdetails> Checklistlst { get; set; }





    }


    public class Compdetails
    {

        public List<SelectListItem> Itemlst { get; set; }

        public string PartNumber { get; set; }
        public string DateOfIssue { get; set; }
        public string LifeTimeInHrs { get; set; }
        public string Isvalid { get; set; }
     

    }
    public class Majorpart
    {

        public List<SelectListItem> MajorPartlst { get; set; }

        public string Majorparts { get; set; }
        public string Active { get; set; }
        public string Critical { get; set; }
        public string Isvalid { get; set; }



    }
    public class Checklistdetails
    {

        public List<SelectListItem> Checklistlst { get; set; }

        public string Service { get; set; }
        public string Type { get; set; }
        public string Isvalid { get; set; }


    }
    public class MachineList
    {
        public string id { get; set; }
        public string mcode { get; set; }
        public string mname { get; set; }
        public string mloc { get; set; }
        public string mserialno { get; set; }
        public string mmodel { get; set; }
       
        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }
     
        public string rrow { get; set; }

    }



}
