using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;
using AspNetCore.Reporting;

namespace Arasan.Controllers.Report
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IPO _po;
        public ReportController (IWebHostEnvironment WebHostEnvironment,IPO po)
        {
           this. _WebHostEnvironment = WebHostEnvironment;
            this._po = po;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Print()
        {
            string mimtype="";
            int extension = 1;
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\Report1.rdlc";
            Dictionary<string,string> Parameters= new Dictionary<string,string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var product = await _po.GetPOItem();
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", product);
             var result=localReport.Execute(RenderType.Pdf,extension,Parameters,mimtype);

            return File(result.MainStream,"application/Pdf");
        }
    }
}
