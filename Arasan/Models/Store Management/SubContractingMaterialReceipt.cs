using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SubContractingMaterialReceipt
    {
        public SubContractingMaterialReceipt()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.Loc = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> Suplst;
        public List<SelectListItem> assignList;
        public List<SelectListItem> Loc;
        public List<SelectListItem> DClst;
        public string ID { get; set; }
        public string DCNo { get; set; }

        public string Branch { get; set; }
        public string Supplier { get; set; }
        public string Location { get; set; }
        public string ddlStatus { get; set; }

        public string enterd { get; set; }
        public string Docdate { get; set; }
        public string DocId { get; set; }
        public string Enterd { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string WorkCenter { get; set; }
        public string itemrate { get; set; }
        public string Narration { get; set; }

        public string item { get; set; }
        public string itemid { get; set; }
        public string Pri { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Totdrum { get; set; }
        public string qty { get; set; }
        public string rate { get; set; }
        public string DQty { get; set; }
        public string qtyrec { get; set; }
        public string TotRecqty { get; set; }

        public List<SubMaterialItem> SubMatlilst { get; set; }
        public List<SubContractItem> Contlilst { get; set; }

        public List<DrumItemDeatil> drumlist { get; set; }
        public List<pendingitem> penlst { get; set; }
    }
    public class SubMaterialItem
    {
        public string id { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public string item { get; set; }
        public string itemid { get; set; }
        public string qty { get; set; }
        public string unit { get; set; }
        public string unitid { get; set; }


        public string rate { get; set; }
        public string Iitemid { get; set; }
        public string amount { get; set; }
        public string supid { get; set; }

        public string lot { get; set; }
        public string drate { get; set; }
        public string dqty { get; set; }
        public string drumno { get; set; }
        public string damount { get; set; }

        public string Isvalid { get; set; }
    }
    public class DrumItemDeatil
    {
        public string ID { get; set; }


        public string item { get; set; }
        public string Pri { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string drumqty { get; set; }
        public string itemid { get; set; }
        public string drumamount { get; set; }
        public string drumrate { get; set; }
        public List<SelectListItem> drulist { get; set; }
        public string drumno { get; set; }
        public string prefix { get; set; }
        public string totaldrum { get; set; }
        public string unitid { get; set; }

        public string qty { get; set; }

        public string rate { get; set; }
        public string amount { get; set; }

        public string Isvalid { get; set; }

        public string uniqueid { get; set; }
    }
    public class SubContractItem
    {
        public string id { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public string item { get; set; }
        public string itemid { get; set; }
        public string recqty { get; set; }
        public string qty { get; set; }
        public string cf { get; set; }
        public string unit { get; set; }
        public string unitid { get; set; }

       
        public string rate { get; set; }
       
        public string amount { get; set; }
        public string billrate { get; set; }
        public string supid { get; set; }
        public string detid { get; set; }



        public string Isvalid { get; set; }
    }
    public class MaterialRecItem
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string supplier { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string loc { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string move { get; set; }
        public string view { get; set; }
        public string recept { get; set; }
        public string viewpen { get; set; }

    }
    public class pendingitem
    {
        public long id { get; set; }
        public string item { get; set; }
        public string supplier { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string loc { get; set; }
        public string qty { get; set; }
        public string penqty { get; set; }
        public string recqty { get; set; }
        public string baid { get; set; }
         

    }

    public class SubMrdc
    {
        public string DOCDATE { get; set; }
        public string DOCID { get; set; }


        public string ITEMID { get; set; }
        public string PARTYID { get; set; }
        public string CPNAME { get; set; }
        public string ENTEREDBY { get; set; }

        public string ADD1 { get; set; }
        public string ADD2 { get; set; }
        public string CITY { get; set; }
        public string THROUGH { get; set; }
        public string ADDRESS { get; set; }

 
        public string GSTNO { get; set; }
   
    }
    public class SubMrdcdet
    {

        public string ITEMID { get; set; }
        public string HSN { get; set; }
        public string UNIT { get; set; }



        public double RECQTY { get; set; }
        public double AMOUNT { get; set; }
        public double COSTRATE { get; set; }

    }
    public class SubActMrdcdet
    {

        public string ITEMID { get; set; }
        public string HSN { get; set; }
        public string UNIT { get; set; }



        public double MRQTY { get; set; }
        public double MRAMOUNT { get; set; }
        public double MRRATE { get; set; }

    }
}
