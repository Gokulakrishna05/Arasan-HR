using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ItemName
    {
        public ItemName()
        {
            this.IgLst = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
            this.Iclst = new List<SelectListItem>();
            this.Isglst = new List<SelectListItem>();
            this.unitlst = new List<SelectListItem>();
            this.Hsn = new List<SelectListItem>();
            this.Bin = new List<SelectListItem>();
            this.Ledgerlst = new List<SelectListItem>();
            this.Tarrifflst = new List<SelectListItem>();
            this.purlst = new List<SelectListItem>();
            this.costlst = new List<SelectListItem>();
            //this.Itemlst = new List<SelectListItem>();

        }
        public List<SelectListItem> Ledgerlst;

        public string Ledger { get; set; }
        public List<SelectListItem> IgLst;
        public string ItemGroup { get; set; }
        public string ItemCategory { get; set; }

        public List<SelectListItem> Iclst;
        public string ItemSubGroup { get; set; }

        public List<SelectListItem> Isglst;
        public List<SelectListItem> Tarrifflst;
        public List<SelectListItem> Itemlst;
        
        public string ddlStatus { get; set; }
        public string HSNcode { get; set; }
        public string AddItem { get; set; }

        public List<SelectListItem> Hsn;
        public string BinId { get; set; }

        public List<SelectListItem> Bin { get; set; }

        public List<BinItem> Binlst;
        public List<SelectListItem> qclst;
        public List<SelectListItem> unitlst;
        public List<SelectListItem> purlst;
        public List<SelectListItem> fqclst;
        public List<SelectListItem> costlst;
        public List<SelectListItem> classlst;
        public List<SelectListItem> valuelst;
        public List<SelectListItem> subcategorylst;
        public List<SelectListItem> RawMateriallst;
        public string ID { get; set; }
        public string Unit { get; set; }
        public string ItemG { get; set; }
        public string lastdate { get; set; }
        public string runhrs { get; set; }
        public string runhrsqty { get; set; }
        public string autocon { get; set; }
        public string costcat { get; set; }
        public string rundet { get; set; }
        public string createdby { get; set; }
        public string purchasecate { get; set; }
        public string flow { get; set; }
        public string ItemSub { get; set; }
        public string QCTemp { get; set; }
        public string FQCTemp { get; set; }
        public string SubCat { get; set; }
        public string subcategory { get; set; }
        public string ItemCode { get; set; }
        //public List<SelectListItem> Itemlst;
        public string Item { get; set; }
        public string ItemDes { get; set; }
        public string Reorderqu { get; set; }
        public string Reorderlvl { get; set; }
        public string Maxlvl { get; set; }
        public string Minlvl { get; set; }
        public string Con { get; set; }
        public string Uom { get; set; }
        public string Hcode { get; set; }
        public string Selling { get; set; }
        public string Stock { get; set; }
        public string Control { get; set; }
        public string SupName { get; set; }
        public string SupPartNo { get; set; }
        public string Price { get; set; }
        public string Dy { get; set; }
        public string BinID { get; set; }
        public string BinYN { get; set; }
        //public string ItemMas { get; set; }
        public string EditRow { get; set; }
        public string DelRow { get; set; }
        public List<ItemName> pflst { get; set; }

        public string StackAccount { get; set; }
        public string Expiry { get; set; }
        public string ValuationMethod { get; set; }
        public string Serial { get; set; }
        public string lot { get; set; }
        public string Batch { get; set; }
        public string QCTemplate { get; set; }
        public string QCRequired { get; set; }
        public string Drumyn { get; set; }
        public string major { get; set; }
        public string bankst { get; set; }
        public string qctest { get; set; }
        public string Latest { get; set; }
        public string SubHeading { get; set; }
        public string Rejection { get; set; }
        public string clssscode { get; set; }
        public string imgpath { get; set; }
        public string leadtime { get; set; }
        public string Percentage { get; set; }
        public string itemfrom { get; set; }
        public string Tarriff { get; set; }
        public string PercentageAdd { get; set; }
        public string Additive { get; set; }
        public string RawMaterial { get; set; }
        public string Curing { get; set; }
        public string Auto { get; set; }

        public List<UnitItem> unititemlst { get; set; }
        public List<SupItem> Suplst { get; set; }
        public List<LocdetItem> locdetlst { get; set; }
        public List<uplodItem> Uplst { get; set; }
    }

    public class SupItem
    {

        public List<SelectListItem> Suplierlst { get; set; }
        
        public string SupName { get; set; }
        public string SupplierPart { get; set; }
        public string PurchasePrice { get; set; }
        public string Preforder { get; set; }
        public string Delivery { get; set; }
        public string Isvalid { get; set; }

        public string ID { get; set; }
        //public string BinID { get; set; }
        //public string BinYN { get; set; }
        // public string ItemMas { get; set; }


    }
    public class UnitItem
    {

        public List<SelectListItem> UnitLst { get; set; }
        public List<SelectListItem> UnittypeLst { get; set; }

        public string Unit { get; set; }
        public string cf { get; set; }
        public string unittype { get; set; }
        public string uniqid { get; set; }
        
        public string Isvalid { get; set; }

        public string ID { get; set; }
         

    }
    public class LocdetItem
    {

        public List<SelectListItem> locLst { get; set; }

        public string loc { get; set; }
        public string bank { get; set; }
        public string maxlevel { get; set; }
        public string minlevel { get; set; }

        public string reorder { get; set; }
        public string Isvalid { get; set; }

        public string ID { get; set; }


    }
    public class uplodItem
    {

         
        public string docpath { get; set; }

        public string ID { get; set; }


    }
    public class BinItem
    {
        public string Isvalid { get; set; }
        public string BinID { get; set; }
        public string BinYN { get; set; }
    }

    public class ItemList
    {
        public string id { get; set; }
        public string itemgroup { get; set; }
        public string itemsubgroup { get; set; }
        public string itemname { get; set; }
        public string bin { get; set; }
        public string cf { get; set; }
        public string uom { get; set; }
        public string hsncode { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string itemcode { get; set; }
        public string itemcate { get; set; } 
        public string rrow { get; set; }

    }

    public class itemupload
    {
        public string id { get; set; }
        public string branch { get; set; }
        public string supplier { get; set; }
        public string quono { get; set; }
        public string podate { get; set; }
        public string mailrow { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string genpo { get; set; }
        public string upload { get; set; }
        public string pdf { get; set; }
        public string view { get; set; }
        public string move { get; set; }
        public string doc { get; set; }
        public string pono { get; set; }
        public string download { get; set; }
    }


}
