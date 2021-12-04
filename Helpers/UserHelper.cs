using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTrace.Helpers
{
    public static class UserHelper
    {
        public static int GetUserId(System.Security.Claims.ClaimsPrincipal User)
        {
            int id = Convert.ToInt32(User.Claims.First(c => c.Type == "ID").Value);
            return id;
        }
    }
}
