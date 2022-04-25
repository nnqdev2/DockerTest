using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using System.DirectoryServices.AccountManagement;
using QMR.Services.Auth;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace QMR.Models
{
    public abstract class BaseViewModel
    {
        public  bool _Admin = false, _DataStewards = false;
        public  string _currentUser = "",_ADgroup="";


        public static List<Principal> memberList = new List<Principal>();
        public bool IsInGroup(string currentuser, string GroupName)
        {

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "deq.state.or.us");
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, currentuser);

            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, GroupName);
            _currentUser = user.ToString();
            bool isUser = false;
            if (user.IsMemberOf(group))
            {
                isUser = true;
            }
            return isUser;
        }
        
        public string getCurrentUser(string currentuser)
        {
            { return _currentUser= currentuser; }
        }

    }
}
