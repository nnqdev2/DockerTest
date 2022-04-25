using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QMR.Models;
namespace QMR.Controllers
{
    public class BaseController : Controller
    {
        internal static string currentUser = "";
        public BaseViewModel BaseViewModel { get; set; }

        public BaseController()
        {

        }
        protected void  SetSessionInfo()
        {
            currentUser = HttpContext.User.Identity.Name;
            HttpContext.Session.SetString("currentUser", currentUser);
            HttpContext.Session.SetString("IsAdmin", IsInGroup(currentUser, "QMRAdmins").ToString());
            HttpContext.Session.SetString("IsDataSteward", IsInGroup(currentUser, "QMRStewards").ToString());

        }

        private bool IsInGroup(string currentuser, string GroupName)
        {

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "deq.state.or.us");
            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, currentuser);

            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, GroupName);

            HttpContext.Session.SetString("currentUserName", user.ToString());
            bool isUser = false;
            if (user.IsMemberOf(group))
            {
                isUser = true;
            }
            return isUser;
        }
        public bool IsAuthenticated
        {
            get { return User.Identity.IsAuthenticated; }
        }
    }
}
