 using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ActivitiesCO
    {
        public string Brc { get; set; }
        public List<SelectListItem> Brchlst { get; set; }

        public string Loc { get; set; }
        public List<SelectListItem> LocLst { get; set; }
        public string EnTy { get; set; }
        public List<SelectListItem> Entlst { get; set; }

        public string Docno { get; set; }
        public string DocDate { get; set; }
        public string ActId { get; set; }
        public List<SelectListItem> Actilst { get; set; }

        public string AcDat { get; set; }
        public string FrTim { get; set; }
        public string ToTim { get; set; }
        public string FrDat { get; set; }
        public string FrTimTy { get; set; }
        public string CaHrs { get; set; }
        public string ToDat { get; set; }
        public string ToTimTy { get; set; }
        public string PB { get; set; }
        public List<SelectListItem> PBlst { get; set; }
        public string MtId { get; set; }
        public List<SelectListItem> McTolst { get; set; }

        public string DeTyp { get; set; }
        public List<SelectListItem> DepTyplst { get; set; }

        public string MaTyp { get; set; }
        public List<SelectListItem> MaiTyplst { get; set; }

        public string PrePB { get; set; }
        public string SfHr { get; set; }
        public string MhYN { get; set; }
        public List<SelectListItem> MYNlst { get; set; }

        public string ddlStatus { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string ID { get; set; }

        public string Plac { get; set; }
        public string Acdo { get; set; }


        public string Conam { get; set; }
        public string Oth { get; set; }
        public string Tamo { get; set; }
        public string Emcos { get; set; }
        public string Rem { get; set; }
        public string Camo { get; set; }
        public string Samo { get; set; }




        public List<Cons> ConLst { get; set; }
        public List<Emp> EmpLst { get; set; }
        public List<Ser> SerLst { get; set; }
        public List<Chk> ChkLst { get; set; }


    }

   

    public class Cons
    {
        public string Itm { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public string Unit { get; set; }
        public string Active { get; set; }
        public string Curst { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amo { get; set; }
        public string Isvalid1 { get; set; }


    }
    public class Emp
    {
        public string EmplId { get; set; }
        public List<SelectListItem> EmplIdlst { get; set; }

        public string EmpName { get; set; }
        public string Edes { get; set; }
        public string Nhr { get; set; }
        public string Ohr { get; set; }
        public string Whr { get; set; }
        public string Ecost { get; set; }
        public string Isvalid2 { get; set; }


    }

    public class Ser
    {
        public string Pid { get; set; }
        public List<SelectListItem> Paridlst { get; set; }

        public string Sedec { get; set; }
        public string SQty { get; set; }
        public string SRate { get; set; }
        public string SAmo { get; set; }
        public string Isvalid3 { get; set; }


    }
    public class Chk
    {
        public string Ser { get; set; }
        public string Isvalid4 { get; set; }

    }

    public class ListActivitiesCO
    {
        public string loc { get; set; }
        public string enty { get; set; }
        public string did { get; set; }
        public string docdate { get; set; }
        public string actid { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }


    }

}
