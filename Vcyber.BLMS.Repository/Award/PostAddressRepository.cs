using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Repository
{
    public class PostAddressRepository
    {
        public int Add(PostAddress address)
        {
            string sql = @"
insert into PostAddress
(
Name,
PhoneNumber,
OpenId,
Address,
Created
)
values
(
@Name,
@PhoneNumber,
@OpenId,
@Address,
@Created
)
";
            return DbHelp.Execute(sql, new { @Name = address.Name, @PhoneNumber = address.PhoneNumber, @OpenId = address.OpenId, @Address = address.Address, @Created = DateTime.Now });
        }
    }
}
