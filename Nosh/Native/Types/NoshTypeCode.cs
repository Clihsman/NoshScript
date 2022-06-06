using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NoshScript.Nosh.Native.Types
{
    [ComVisible(true)]
    public enum NoshTypeCode
    {
        Null = 0,
        Object = 1,
        Method = 2,
        Boolean = 3,
        Char = 4,
        SByte = 5,
        Byte = 6,
        Int16 = 7,
        UInt16 = 8,
        Int32 = 9,
        UInt32 = 10,
        Int64 = 11,
        UInt64 = 12,
        Float = 13,
        Double = 14,
        Decimal = 15,
        Target = 16,
        String = 18
    }
}
