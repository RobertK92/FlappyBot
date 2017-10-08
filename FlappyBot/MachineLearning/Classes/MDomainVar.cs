using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class MDomainVar
    {
        public delegate IEnumerable<float> MValuesDelegate();
        private MValuesDelegate valueDelegate;

        public MDomainVar(MValuesDelegate valueDelegate)
        {
            this.valueDelegate = valueDelegate;
        }

        public IEnumerable<float> GetValues()
        {
            return valueDelegate();
        }
    }
}
