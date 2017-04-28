using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using PetaPoco;
using Vcyber.BLMS.Entity.Generated;


namespace Vcyber.BLMS.Repository.Entity.Generated
{
    public class PocoHelper
    {
        public static Database CurrentDb()
        {
            if (HttpContext.Current.Items["CurrentDb"] == null)
            {
                var retval = BMDb.GetInstance();
                HttpContext.Current.Items["CurrentDb"] = retval;
                return retval;
            }
            return (Database)HttpContext.Current.Items["CurrentDb"];
        }
        public static Database DBContext()
        {
            return new Database("DbConnection");
        }
    }
}
