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
        public ReportController (IWebHostEnvironment WebHostEnvironment)
        {
           this. _WebHostEnvironment = WebHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Print()
        {
            string mimtype="";
            int extension = 1;
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\Report1.rdlc";
            Dictionary<string,string> Parameters= new Dictionary<string,string>();
            Parameters.Add("rp1", " Hi Everyone");
            LocalReport localReport = new LocalReport(path);
            var result=localReport.Execute(RenderType.Pdf,extension,Parameters,mimtype);

            return File(result.MainStream,"application/Pdf");
        }
    }
}
