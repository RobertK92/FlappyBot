using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public struct FMTrainingData
    {
        public readonly List<FMSample> Samples;

        public FMTrainingData(List<FMSample> samples = null)
        {
            this.Samples = samples;
            if(Samples == null)
            {
                Samples = new List<FMSample>();
            }
        }
        
    }
}
