using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchMetadataOverTheCswService.Models;

namespace SearchMetadataOverTheCswService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<string>  GetCswServiceResult([FromBody]CswRequestModel cswRequestModelVM)
        {
            string result = "";

            try
            {
                string url = cswRequestModelVM.ServiceUrl;
                var request = (HttpWebRequest)WebRequest.Create(url);
                if (url.ToLowerInvariant().Contains("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                }
                //sistem basic authentication ile çalışıyorsa ilgili header bilgisi eklenmektedir
                if (!string.IsNullOrEmpty(cswRequestModelVM.UserName) && !string.IsNullOrEmpty(cswRequestModelVM.Password))
                {
                    string encoded = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(cswRequestModelVM.UserName + ":" + cswRequestModelVM.Password));
                    request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + encoded);
                }
                string post = cswRequestModelVM.PostContent;
                var postData = Encoding.UTF8.GetBytes(post);
                request.Method = "POST";
                request.ContentType = "application/xml";
                request.ContentLength = postData.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
                result=await new StreamContent(request.GetResponse().GetResponseStream()).ReadAsStringAsync();
               
            }
            catch(Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}
