using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
namespace Vcyber.BLMS.Domain
{
    public class FittingValidateApp : IFittingValidateApp
    {
        public void Add(FittingValidate entity)
        { 
        
        _DbSession.FittingValidateStorager.Add (entity);
               
        }
    }
}
