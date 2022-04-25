using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QMR.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using QMR.Services.Messaging;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System.DirectoryServices.AccountManagement;

namespace QMR.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _mailer;
       
        internal static string  _ADgroup="";
        public HomeController(ILogger<HomeController> logger, IEmailSender mailer)
        {
            _logger = logger;
            _mailer = mailer;
        }
        
        public IActionResult Index()
        {
            SetSessionInfo();
            if ((HttpContext.Session.GetString("IsAdmin")=="True") || (HttpContext.Session.GetString("IsDataSteward") == "True"))
                {
                    return View();
                }
            else
                return View("Error");
        }

        public IActionResult Privacy()
        {
            SetSessionInfo();
            return View();
        }
        public IActionResult Chart()
        {
            SetSessionInfo();
            return PartialView("Chart");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            SetSessionInfo();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
