using System;
namespace Vcyber.BLMS.Domain.CarService
{
    using System.Collections.Generic;
    using System.Linq;

    using Omu.ValueInjecter;

    using PetaPoco;

    using Vcyber.BLMS.Application.CarService;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Domain.Common;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository;

    public class ServiceTypeApp : IServiceType
    {
        public IEnumerable<KeyValuePair<int, string>> GetServiceType()
        {
            return ((EOrderType[])Enum.GetValues(typeof(EOrderType))).ToList()
                .Select(x => new KeyValuePair<int, string>((int)x, x.DisplayName()));
        }
    }
}
