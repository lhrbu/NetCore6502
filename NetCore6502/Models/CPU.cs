using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore6502.Models
{
    public class CPU
    {
        private IMemoryMapper _memoryMapper;
        public CPU(IMemoryMapper memoryMapper)
        { _memoryMapper = memoryMapper; }
        public void ChangeMemoryMapper(IMemoryMapper memoryMapper) => _memoryMapper = memoryMapper;
        public byte A { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte SP { get; set; }
        public UInt16 PC { get; set; }
        private long _CycleCount { get; set; } = 0;
        public event EventHandler<CPUCycleIncreasedEventArgs>? CycleCountIncreased;
        private void IncreaseCycleCount()=> CycleCountIncreased?.Invoke(this, new(++_CycleCount));
        public byte ReadMemory(UInt16 address)
        {
            byte result = _memoryMapper.ReadMemory(address);
            IncreaseCycleCount();
            return result;
        }
        public void WriteMemory(UInt16 address,byte value)
        {
            IncreaseCycleCount();
            _memoryMapper.WriteMemory(address, value);
        }
        public byte CurrentOpCode { get; private set; }
    }
}
