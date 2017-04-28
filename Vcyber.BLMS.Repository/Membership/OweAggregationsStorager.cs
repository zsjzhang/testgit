using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Repository.Entity.Generated;
using Vcyber.BLMS.IRepository;
namespace Vcyber.BLMS.Repository
{
    public class OweAggregationsStorager : IOweAggregationsStorager
    {
        public void Add(OweAggregations obj) 
        {
            var sql = Sql.Builder
                .Append("INSERT dbo.OweAggregations(RankID,UserID,CreateTime,PhoneNumber,IdentityNumber,Mlevel,MlevelChange,Province,City,CardCout,TotalIntegral,IsChange)")
                .Append("VALUES(@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)", obj.RankID, obj.UserID, obj.CreateTime, obj.PhoneNumber, obj.IdentityNumber, obj.Mlevel, obj.MlevelChange, obj.Province, obj.City, obj.CardCout, obj.TotalIntegral, obj.IsChange);
            PocoHelper.CurrentDb().Execute(sql);
        }
    }
}
