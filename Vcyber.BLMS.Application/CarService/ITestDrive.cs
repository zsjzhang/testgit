using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application.CarService
{

    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Generated;

    public interface ITestDrive : ICarServiceBase<TestDriveEntity, CSTestDrive>
    {
    }
}
