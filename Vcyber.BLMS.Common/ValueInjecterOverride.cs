using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omu.ValueInjecter
{
    public class EnumToInt : LoopValueInjection
    {
        protected override bool TypesMatch(Type sourceType, Type targetType)
        {
            return sourceType.IsSubclassOf(typeof(Enum)) && targetType == typeof(int);
        }
    }

    public class IntToEnum : LoopValueInjection
    {
        protected override bool TypesMatch(Type sourceType, Type targetType)
        {
            return sourceType == typeof(int) && targetType.IsSubclassOf(typeof(Enum));
        }
    }
}
