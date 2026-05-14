using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal_Control_Full.Services
{
    public interface IIRService
    {
        bool HasIrEmitter();
        void Transmit(int carrierFrequency, int[] pattern);
    }
}
