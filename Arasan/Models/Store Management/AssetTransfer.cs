using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class AssetTransfer
    {
        public AssetTransfer()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
            this.Bin = new List<SelectListItem>();
            this.ToBin = new List<SelectListItem>();
            this.ToLoc = new List<SelectListItem>();
           
        }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }

        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public List<SelectListItem> ToLoc;
        public string Location { get; set; }
        public string ToLocation { get; set; }

        public List<SelectListItem> Bin;
        public List<SelectListItem> ToBin;
        public string BinId { get; set; }
        public string ToBinId { get; set; }
        public string Reason { get; set; }
        public string Order { get; set; }
        public string Gross { get; set; }
        public string Net { get; set; }
        public string Narration { get; set; }
        public string ddlStatus { get; set; }

        public List<AssetTransferItem> Assetlst { get; set; }


    }
    public class AssetTransferItem
    {
        public string id { get; set; }
        public string Isvalid { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string ItemId { get; set; }
        public string Unit { get; set; }
        public string Current { get; set; }
        public string Quantity { get; set; }
        public string rate { get; set; }
        public string Amount { get; set; }
      
        public string saveItemId { get; set; }
        
    }
    public class ListAssetTransferItem
    {
        public double id { get; set; }
        public string branch { get; set; }
        public string docid { get; set; }
        public string docDate { get; set; }
        public string loc { get; set; }
        public string view { get; set; }
        public string delrow { get; set; }
    }
}
