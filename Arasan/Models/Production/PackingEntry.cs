using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class PackingEntry
    {
        public PackingEntry()
        {
            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.Packlst = new List<SelectListItem>();
            this.DrumLoclst = new List<SelectListItem>();
            
            this.Itemlst = new List<SelectListItem>();
            this.Schlst = new List<SelectListItem>();
        }
        public string Branch { get; set; }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> Worklst;
        public List<SelectListItem> Itemlst;
        public List<SelectListItem> Schlst;
        public string Item { get; set; }
        public string Itemid { get; set; }
        public string WorkId { get; set; }
        public string Work { get; set; }
        public string basic { get; set; }
        public string toloc { get; set; }
        public string tothrs { get; set; }
        public string ID { get; set; }
        public string Docid { get; set; }
        public string Docdate { get; set; }
        public string Packnote { get; set; }
        public string ProdSchNo { get; set; }
        public string Schid { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string Shift { get; set; }
        public string Shiftid { get; set; }
        public string user { get; set; }
        public string Loc { get; set; }
        public List<SelectListItem> Packlst;
        public List<SelectListItem> DrumLoclst;
        public string NoteDate { get; set; }
        public string DrumQty { get; set; }
        public string LotNo { get; set; }
        public string PackYN { get; set; }
        public string Totinpqty { get; set; }
        public string Totoutqty { get; set; }
        public string Totoutrate { get; set; }
        public string totalisspamt { get; set; }
        public string ddlStatus { get; set; }
        public string Totoutamount { get; set; }
        public string Totinprate { get; set; }
        public string Remark { get; set; }
        public string TotConamount { get; set; }
        public string totalinpamt { get; set; }
        public List<PackInp> Inplst { get; set; }
        public List<PackMat> Matlst { get; set; }
        public List<PackEmp> Emplst { get; set; }
        public List<Packothcon> oconlst { get; set; }
        public List<PackMach> machlst { get; set; }
        public List<PackDet> Packdetlst { get; set; }

    }
    public class PackInp
    {
        public string ID { get; set; }
        public string drum { get; set; }
        public string drumid { get; set; }
        public string bqty { get; set; }
        
        public string batch { get; set; }
        public string batchno { get; set; }

        public double iqty { get; set; }


        public string comp { get; set; }
        public string packid { get; set; }
        public double rate { get; set; }
        public double amount { get; set; }
        public string Isvalid { get; set; }
  
    }
    public class PackMat
    {
        public string ID { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string item { get; set; }
        public string unit { get; set; }
        public string lotyn { get; set; }
        public string consqty { get; set; }

        public string conrate { get; set; }

        public double conamount { get; set; }


        
        public string Isvalid { get; set; }

    }
    public class PackEmp
    {
        public string ID { get; set; }
        public List<SelectListItem> Emplst { get; set; }
        public string empname { get; set; }
        public string empcode { get; set; }
        public string department { get; set; }

        public string empcost { get; set; }

        public double tempcost { get; set; }



        public string Isvalid { get; set; }

    }
    public class Packothcon
    {
        public string ID { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string item { get; set; }
        public string unit { get; set; }
        public string value { get; set; }

        public string conqty { get; set; }
        public string conrate { get; set; }
        public string conamount { get; set; }

        public double clstk { get; set; }



        public string Isvalid { get; set; }

    }
    public class PackMach
    {
        public string ID { get; set; }
        public List<SelectListItem> Machlst { get; set; }
        public string maccost { get; set; }
        public string macid { get; set; }
       

      



        public string Isvalid { get; set; }

    }
    public class PackDet
    {
        public string ID { get; set; }
        public string drum { get; set; }
        public string drumid { get; set; }
        public string dqty { get; set; }

        public string batch { get; set; }

        public double eqty { get; set; }


        public string comp { get; set; }
        public string pdaid { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string loc { get; set; }
        public string prefix { get; set; }
        public string Isvalid { get; set; }

    }
    public class PackingList
    {
        public string id { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string item { get; set; }
        public string shiftno { get; set; }
        public string packing { get; set; }
        public string location { get; set; }

        public string viewrow { get; set; }
        public string delrow { get; set; }


    }
}
