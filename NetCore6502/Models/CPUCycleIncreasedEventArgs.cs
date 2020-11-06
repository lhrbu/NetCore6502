using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore6502.Models
{
    public class CPUCycleIncreasedEventArgs:EventArgs
    {
        public long CycleCount { get; }
        public CPUCycleIncreasedEventArgs(long cycleCount) => CycleCount = cycleCount;
    }
}
