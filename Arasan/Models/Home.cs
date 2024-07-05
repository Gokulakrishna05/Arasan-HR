using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class Home
    {
        
        public List<CuringGroup> curgroups { get; set; }
        public double loaded { get; set; }
        public string user { get; set; }
        public double empty { get; set; }
        public int res { get; set; }
        public double rem { get; set; }
        public List<GridDisplay> Folllst { get; set; }
        public List<EnqDisplay> Enqlllst { get; set; }
        public List<SalesQuoteDisplay> SalesQuotelllst { get; set; }
        public int Quotefollowcunt { get; set; }
        public int indcnt { get; set; }
        public int minstkcnt { get; set; }
        public int appcnt { get; set; }
        public int supcnt { get; set; }
        public string suppliar { get; set; }
        public string itemcnt { get; set; }
        public int dagrncnt { get; set; }
        public int gatcnt { get; set; }
        public int EnqFollowcunt { get; set; }
        public int indent { get; set; }
        public int enqury { get; set; }
        public int qout { get; set; }
        public int po { get; set; }
        public int grn { get; set; }
        public int direct { get; set; }
        public int SalesQuoteFollowcunt { get; set; }
        public List<CurIn> CurInlst { get; set; }
        public List<PurchaseDash> purlst { get; set; }
        public List<IndentCreate> indlst { get; set; }
        public List<IssuePen> penlst { get; set; }
        public List<indentapp1> applst { get; set; }
        public List<indentsup> suplst { get; set; }
        public List<MatNotifys> Materialnotification { get; set; }
        public List<MenuList> Menulst { get; set; }
        public List<GateIn> GateInlst { get; set; }
        public List<ChartData> chrtlst { get; set; }    
        public List<StoreChartData> schrtlst { get; set; }
        public List<LocDetail> Locdet { get; set; }

        public string Empid { get; set; }
        public string Empname { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Ads { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public string Username { get; set; }
        public string Edep { get; set; }
        public string Edes { get; set; }
        public string Jdate { get; set; }

        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Marsts { get; set; }
        public string Blood { get; set; }
        public string Commu { get; set; }


        public string Edu { get; set; }
        public string College { get; set; }
        public string EduPlc { get; set; }
        public string YOP { get; set; }
        public string Grade { get; set; }
        public string Skill { get; set; }


        public string Ecat { get; set; }
        public string Ebas { get; set; }
        public string PFN { get; set; }
        public string ESIN { get; set; }
        public string PFD { get; set; }
        public string ESID { get; set; }
        public string ECOST { get; set; }

    }
    public class LocDetail
    {
        public string Loc { get; set; }
    }





public class salesdash { 
       public List<topsellpro> topsellpros { get; set; }
       public List<Salespar> Salesparlst { get; set; }
}
    public class topsellpro
    {
        public int sno { get; set;}
        public string itemname { get; set;}
        public double per { get; set;}
    }
    public class Salespar
    {
        public int sno { get; set; }
        public string itemname { get; set; }
        public string import { get; set; }
        public double per { get; set; }
    }
    public class MatNotifys
    {
        public string Date { get; set; }
        public string LocationName { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string Unit { get; set; }
        public string stockQty { get; set; }
    }
    public class indentapp1
    {
        public string indentno { get; set; }
        public string indentdate { get; set; }
        public string ItemName { get; set; }
        public string Qty { get; set; }
        public string loc { get; set; }
        public string stockQty { get; set; }
    }
    public class GateIn
    {
        public string GateDate { get; set; }
        public string GateinDate { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string Unit { get; set; }
        public string id { get; set; }
        public string gateid { get; set; }
    }
    public class indentsup
    {
        public string indentno { get; set; }
        public string indentdate { get; set; }
        public string ItemName { get; set; }
        public string Qty { get; set; }
        public string loc { get; set; }
        public string stockQty { get; set; }
    }
    public class PurchaseDash
    {
        public string Date { get; set; }
        public string grn { get; set; }
        public string ItemName { get; set; }
        public string party { get; set; }
        public string qty { get; set; }
        public string grndetid { get; set; }
        public string days { get; set; }
        public string grnbasicid { get; set; }
        public string notify { get; set; }
        public string stockQty { get; set; }
    }
    public class IndentCreate
    {
        public string Date { get; set; }
        public string docid { get; set; }
        public string ItemName { get; set; }
        public string location { get; set; }
        public string qty { get; set; }
        public string notify { get; set; }
        public string days { get; set; }
        public string basicid { get; set; }
        public string detid { get; set; }
        public string stockQty { get; set; }
    }
    public class IssuePen
    {
        public string Date { get; set; }
        public string docid { get; set; }
        public string ItemName { get; set; }
        public string location { get; set; }
        public string qty { get; set; }
        public string notify { get; set; }
        public string days { get; set; }
        public string basicid { get; set; }
        public string detid { get; set; }
        public string stockQty { get; set; }
    }
    public class CuringGroup
    {
        public string curinggroup { get; set; }
        public List<CuringSet> curset { get; set; }
    }
    public class CuringSet
    {
        public string Roomno { get; set; }
        public string Capacity { get; set; }
        public string status { get; set; }
        public string Occupied { get; set; }

    }
    public class GridDisplay
    {
        public string displaytext { get; set; }

        public string followedby { get; set; }

        public string Count { get; set; }

        public string status { get; set; }

        public string CPerson { get; set; }

    }
    public class EnqDisplay
    {
        public string displaytext { get; set; }

        public string followedby { get; set; }

        public string Count { get; set; }

        public string status { get; set; }

        public string CPerson { get; set; }

    }
    public class ChartData
    {
        public string cvalue { get; set; }

        public string ctext { get; set; }

    }
    public class StoreChartData
    {
        public string cvalue { get; set; }

        public string ctext { get; set; }

    }

    public class SalesQuoteDisplay
    {
        public string displaytext { get; set; }

        public string followby { get; set; }

        public string Count { get; set; }

        public string status { get; set; }

        public string CPerson { get; set; }

    }
    public class CurIn
    {
        public string Docid { get; set; }

        public string Drum { get; set; }

        public string Item { get; set; }

        public string Due { get; set; }

        public string Id { get; set; }

    }
    public class MenuItem
    {
        public bool Isdashborad { get; set; }
        public bool Isaccounts { get; set; }
        public bool Issubcontract { get; set; }
        public bool IsMaster { get; set; }
        public bool IsPurchse { get; set; }
        public bool IsStore { get; set; }
        public bool Issales { get; set; }
        public bool Isproduction { get; set; }
        public string dashparent { get; set; }
        public string masterparent { get; set; }
        public string purchaseparent { get; set; }
        public string storeparent { get; set; }
        public string accountsparent { get; set; }
        public string subcontractparent { get; set; }
        public List<MenuList> DmenuLists { get; set; }
        public List<MenuList> MmenuLists { get; set; }
        public List<MenuList> PmenuLists { get; set; }
    }
    public class MenuList
    {
        public string Title { get; set; }
        public string Parent { get; set; }
        public string Groupid { get; set; }
        public string IsHead { get; set; }
        public string IsDisable { get; set; }
        public string IsView { get; set; }

    }
}
