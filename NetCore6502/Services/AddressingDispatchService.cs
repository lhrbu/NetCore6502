using NetCore6502.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore6502.Services
{
    public class AddressingDispatchService
    {
        public AddressingDispatchService()
        {
            AddressingMethods = new Func<CPU, ushort>[]{
                Absolute,AbsoluteX,AbsoluteY,Accumulator,Immdiate,Implied,
                IndirectX,IndirectY,Indirect,Relative,ZeroPage,ZeroPageX,ZeroPageY
            };
        }
        private Func<CPU, UInt16>[] AddressingMethods { get; }
        public UInt16 Dispatch(CPU cpu, AddressingMode addressingMode) =>
            AddressingMethods[(uint)addressingMode].Invoke(cpu);
        public UInt16 Absolute(CPU cpu)
        {
            uint lowAddress = cpu.ReadMemory(cpu.PC++);
            uint highAddress = cpu.ReadMemory(cpu.PC++);
            return CombineAddress(lowAddress, highAddress);
        }


        private byte[] _absoluteXCheckedOpcodes = { 0x1E, 0xDE, 0xFE, 0x3E, 0x7E, 0x9D };
        public UInt16 AbsoluteX(CPU cpu)
        {
            uint lowAddress = cpu.ReadMemory(cpu.PC++);
            uint highAddress = cpu.ReadMemory(cpu.PC++);
            uint combinedAddress = CombineAddress(lowAddress, highAddress);

            if (lowAddress + cpu.X > 0xFF &&
                (!_absoluteXCheckedOpcodes.Contains(cpu.CurrentOpCode)))
            {
                uint readAddress = (combinedAddress + cpu.X - 0xFF) & 0xFFFF;
                cpu.ReadMemory((UInt16)readAddress);

            }
            return (UInt16)((combinedAddress + cpu.X) & 0xFFFF);
        }

        public UInt16 AbsoluteY(CPU cpu)
        {
            uint lowAddress = cpu.ReadMemory(cpu.PC++);
            uint highAddress = cpu.ReadMemory(cpu.PC++);
            uint combinedAddress = CombineAddress(lowAddress, highAddress);

            if (lowAddress + cpu.Y > 0xFF && cpu.CurrentOpCode != 0x99)
            {
                uint readAddress = (combinedAddress + cpu.Y - 0xFF) & 0xFFFF;
                cpu.ReadMemory((UInt16)readAddress);
            }
            return (UInt16)((combinedAddress + cpu.Y) & 0xFFFF);
        }

        public UInt16 Immdiate(CPU cpu) => cpu.PC++;

        public UInt16 IndirectX(CPU cpu)
        {
            uint address = cpu.ReadMemory(cpu.PC++);
            cpu.ReadMemory((UInt16)address);
            address += cpu.X;

            uint lowAddress = cpu.ReadMemory((UInt16)(address & 0xFF));
            uint indirectHighAddress = ((address + 1) & 0xFF) << 8;
            uint highAddress = cpu.ReadMemory((UInt16)indirectHighAddress);
            return CombineAddress(lowAddress, highAddress);
        }

        public UInt16 IndirectY(CPU cpu)
        {
            uint address = cpu.ReadMemory(cpu.PC++);
            uint lowAddress = cpu.ReadMemory((UInt16)address);
            uint indirectHighAddress = ((address + 1) & 0xFF) << 8;
            uint highAddress = cpu.ReadMemory((UInt16)indirectHighAddress);
            uint combinedAddress = CombineAddress(lowAddress, highAddress);
            if ((combinedAddress & 0xFF) + cpu.Y > 0xFF && cpu.CurrentOpCode != 0x91)
            {
                cpu.ReadMemory((UInt16)((combinedAddress + cpu.Y - 0xFF) & 0xFFFF));
            }
            return (UInt16)((combinedAddress + cpu.Y) & 0xFFFF);
        }

        public UInt16 Relative(CPU cpu) => cpu.PC;
        public UInt16 ZeroPage(CPU cpu) => cpu.ReadMemory(cpu.PC++);
        public UInt16 ZeroPageX(CPU cpu)
        {
            uint address = cpu.ReadMemory(cpu.PC++);
            cpu.ReadMemory((UInt16)address);
            address += cpu.X;
            address &= 0xFF;
            if (address > 0xFF)
            { address -= 0x100; }
            return (UInt16)address;
        }

        public UInt16 ZeroPageY(CPU cpu)
        {
            uint address = cpu.ReadMemory(cpu.PC++);
            cpu.ReadMemory((UInt16)address);

            address += cpu.Y;
            address &= 0xFF;
            return (UInt16)address;
        }

        public UInt16 Accumulator(CPU cpu) => throw new InvalidOperationException("Accumulator mode doesn't need addressing");
        public UInt16 Implied(CPU cpu) => throw new InvalidOperationException("Implied mode doesn't need addressing");
        public UInt16 Indirect(CPU cpu) => throw new InvalidOperationException("Indirect mode doesn't need addressing");
        private UInt16 CombineAddress(uint lowAddress, uint highAddress) =>
            (UInt16)(lowAddress | highAddress << 8);
    }
}
