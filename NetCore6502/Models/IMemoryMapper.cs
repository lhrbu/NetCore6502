using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore6502.Models
{
    public interface IMemoryMapper
    {
        byte ReadMemory(UInt16 address);
        void WriteMemory(UInt16 address, byte value);
    }
}
