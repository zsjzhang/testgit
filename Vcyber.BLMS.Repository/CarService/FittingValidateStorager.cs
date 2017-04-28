
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    using PetaPoco;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Repository.Entity.Generated;
    public class FittingValidateStorager : IFittingValidateStorager 
    {
        public void Add(FittingValidate entity)
        {
            entity.Createtime   = DateTime.Now;
            StringBuilder sql = new StringBuilder();
            //sql.Append(" insert into  FittingValidate ( Code, UserAddress , Longitude , Latitude , Altitude , Userid, Ctype ,Result  ,Createtime )");
            //sql.Append(" values(@Code, @UserAddress , @Longitude , @Latitude , @Altitude , @Userid, @Ctype ,@Result ,@Createtime");

           sql.Append  ("insert into  FittingValidate ( Code, UserAddress , Longitude , Latitude , Altitude , Userid, Ctype ,Result  ,Createtime ) ");
           sql.Append(" values (  @Code, @UserAddress , @Longitude , @Latitude , @Altitude , @Userid, @Ctype ,@Result  ,@Createtime )");
            DbHelp.Execute (sql.ToString(), entity);
        }

    }
}
