using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ProductionForecasting
    {
        public ProductionForecasting()
        {
            this.Brlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> RecList;
        public string Enterd { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string PType { get; set; }
        public string ForMonth { get; set; }
        public string Ins { get; set; }
        public string Hd { get; set; }
        public string Fordate { get; set; }
        public string Enddate { get; set; }
        public string ddlStatus { get; set; }

        public List<PFCItem> PFCILst { get; set; }

        public List<PFCDGItem> PFCDGILst { get; set; }
        public List<PFCPYROItem> PFCPYROILst { get; set; }

        public List<PFCPOLIItem> PFCPOLILst { get; set; }
        public List<PFCRVDItem> PFCRVDLst { get; set; }
        public List<PFCPASTEItem> PFCPASTELst { get; set; }
        public List<PFCAPPRODItem> PFCAPPRODLst { get; set; }
        public List<PFCPACKItem> PFCPACKLst { get; set; }

        public string plantype { get; set; }
        public string apspowder { get; set; }
        public string reqqty { get; set; }
        public string avlstk { get; set; }
        public string ministk { get; set; }
        public string reqappowder { get; set; }
        public string appyro { get; set; }
        public string appaste { get; set; }
        public string apfg { get; set; }
        public string reqappow { get; set; }
        public string apsministk { get; set; }
        public string apstk { get; set; }
        public string coarse { get; set; }
        public string power { get; set; }
         

        public List<SelectListItem> mnthlst { get; set; }
    }
    public class PFCItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Isvalid { get; set; }

        public string Unit { get; set; }
        public string PType { get; set; }
        public string Fqty { get; set; }

        public string PtmQty { get; set; }
        public string PysQty { get; set; }
       
    }
    public class PFCDGItem
    {
        public string ID { get; set; }
        public string itemid { get; set; }
        public string saveitemid { get; set; }
        public List<SelectListItem> PItemlst { get; set; }
        public string Isvalid { get; set; }

        public string target { get; set; }
        public string min { get; set; }
        public string stock { get; set; }
        public string reqadditive { get; set; }
        public string rawmaterial { get; set; }
        public string rawmaterialid { get; set; }
        public string reqpyro { get; set; }
         

        public string required { get; set; }
        public string dgaddit { get; set; }
        public string dgadditid { get; set; }

    }
    public class PFCPYROItem
    {

        public string ID { get; set; }
        public string itemid { get; set; }
        public string saveitemid { get; set; }
        public List<SelectListItem> PYItemlst { get; set; }

        public string WorkId { get; set; }
        public List<SelectListItem> Worklst { get; set; }
        public string Isvalid { get; set; }

        public string CDays { get; set; }
        public string minstock { get; set; }
        public string pasterej { get; set; }
        public string GradeChange { get; set; }
        public string rejqty { get; set; }
        public string required { get; set; }
        public string target { get; set; }
        public string ProdDays { get; set; }
        public string ProdQty { get; set; }
        public string RejMat { get; set; }
        public string RejMatReq { get; set; }
        public string BalanceQty { get; set; }
        public string additive { get; set; }
        public string additiveid { get; set; }
        public string per { get; set; }
        public string AllocAdditive { get; set; }
        public string ReqPowder { get; set; }
        public string WStatus { get; set; }
        public string PowderRequired { get; set; }
        public string stock { get; set; }

    }
    public class PFCPOLIItem
    {
        public string ID { get; set; }
        public string itemid { get; set; }
        public string saveitemid { get; set; }
        public List<SelectListItem> POItemlst { get; set; }
        public string workid { get; set; }
        public List<SelectListItem> POWorklst { get; set; }
        public string Isvalid { get; set; }
        public string wcdays { get; set; }
        public string target { get; set; }
        public string capacity { get; set; }
        public string stock { get; set; }
        public string minstock { get; set; }
        public string required { get; set; }
        public string days { get; set; }
        public string additive { get; set; }
        public string additiveid { get; set; }
        public string add { get; set; }
        public string rejmat { get; set; }
        public string reqper { get; set; }
        public string rvdqty { get; set; }
        public string pyropowder { get; set; }
        public string pyroqty { get; set; }
        public string rawmat { get; set; }
        public string rawmatid { get; set; }
        public string powderrequired { get; set; }

    }
    public class PFCRVDItem
    {
        public string ID { get; set; }
        public string itemid { get; set; }
        public List<SelectListItem> POItemlst { get; set; }
        public string workid { get; set; }
        public List<SelectListItem> POWorklst { get; set; }
        public string Isvalid { get; set; }
        public string wcdays { get; set; }
        
       
        public string days { get; set; }
        public string mto { get; set; }
        public string mtoloss { get; set; }
        public string qty { get; set; }
        public string cons { get; set; }
        public string consqty { get; set; }
       
        public string prodqty { get; set; }
        public string rawmat { get; set; }
        public string powderrequired { get; set; }

    }
    public class PFCPASTEItem
    {

        public string ID { get; set; }
        public string itemid { get; set; }
      
        public string saveitemid { get; set; }
        public List<SelectListItem> PYItemlst { get; set; }

        public string WorkId { get; set; }
        public List<SelectListItem> Worklst { get; set; }
        public string Isvalid { get; set; }

        public string CDays { get; set; }
        public string minstock { get; set; }
        public string addcost { get; set; }
        public string toreq { get; set; }
        public string rejqty { get; set; }
        public string required { get; set; }
        public string target { get; set; }
        public string proddays { get; set; }
        public string production { get; set; }
        public string appowder { get; set; }
        public string balance { get; set; }
        public string charge { get; set; }
        public string RejMat { get; set; }
        public string mtocost { get; set; }
        public string missmto { get; set; }
        public string additive { get; set; }
        public string additiveid { get; set; }
        public string toloss { get; set; }
        public string allocadditive { get; set; }
        public string toprod { get; set; }
        public string rvdloss { get; set; }
        public string coarse { get; set; }
        public string stock { get; set; }
        public string powerrequired { get; set; }
    }
    public class PFCAPPRODItem
    {

        public string ID { get; set; }
        public string itemid { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> PYItemlst { get; set; }

        public string WorkId { get; set; }
        public List<SelectListItem> Worklst { get; set; }
        public string Isvalid { get; set; }

        public string wdays { get; set; }
      
        public string target { get; set; }
        public string proddays { get; set; }
        public string production { get; set; }
        
        public string fuelreq { get; set; }
        public string ingotreq { get; set; }
        public string capacity { get; set; }
      
        public string powerrequired { get; set; }
    }
    public class PFCPACKItem
    {

        public string ID { get; set; }
        public string itemid { get; set; }
        public string saveitemid { get; set; }
        public List<SelectListItem> partylst { get; set; }

        public string party { get; set; }
        public string partyid { get; set; }
        public List<SelectListItem> Worklst { get; set; }
        public string Isvalid { get; set; }

        public string targetitem { get; set; }

        public string target { get; set; }
        public string proddays { get; set; }
        public string production { get; set; }

        public string targetqty { get; set; }
        public string rawmat { get; set; }
        public string rawmatid { get; set; }
         
        public string packmat { get; set; }
        public string packmatid { get; set; }
        public string packqty { get; set; }
        public string reqmat { get; set; }
    }

    public class ProdFCList
    {
        public string id { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string month { get; set; }
        public string plan { get; set; }
       
        public string viewrow { get; set; }
        public string delrow { get; set; }


    }
}
