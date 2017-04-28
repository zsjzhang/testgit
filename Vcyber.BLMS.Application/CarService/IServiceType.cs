using System.Collections.Generic;

namespace Vcyber.BLMS.Application.CarService
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;

    public interface IServiceType
    {
        IEnumerable<KeyValuePair<int, string>> GetServiceType();
    }
}
