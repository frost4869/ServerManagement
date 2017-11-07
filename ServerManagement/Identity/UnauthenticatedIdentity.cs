using ServerManagement.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagement.Identity
{
    class UnauthenticatedIdentity : CustomIdentity
    {
        public UnauthenticatedIdentity() : base(string.Empty, "Guest", 0)
        {
        }
    }
}
