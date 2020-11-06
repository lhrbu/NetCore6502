using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore6502.Models
{
    public enum AddressingMode
    {
        Absolute, AbsoluteX, AbsoluteY, Accumulator, Immdiate, Implied,
        IndirectX, IndirectY, Indirect, Relative, ZeroPage, ZeroPageX, ZeroPageY
    }
}
