using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public struct FMSample
    {
        public readonly float Delay;
        public readonly float[] Values;
        public readonly string InputActionId;

        public FMSample(string inputActionId, float delay, IEnumerable<float> values)
        {
            this.InputActionId = inputActionId;
            this.Delay = delay;
            this.Values = values.ToArray();
        }
    } 
}
