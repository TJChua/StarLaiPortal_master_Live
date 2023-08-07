using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using StarLaiPortal.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarLaiPortal.Module
{
    public class ChangeDatabaseActiveDirectoryAuthentication : AuthenticationActiveDirectory<SecuritySystemUser, CustomLogonParametersForActiveDirectoryAuthentication>
    {
        public ChangeDatabaseActiveDirectoryAuthentication()
        {
            CreateUserAutomatically = true;
        }
        public override bool IsLogoffEnabled
        {
            get
            {
                return true;
            }
        }
        public override bool AskLogonParametersViaUI
        {
            get
            {
                return true;
            }
        }
    }
}
