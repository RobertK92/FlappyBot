using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class MDomain : IMPulsable
    {
        private List<MDomainVar> domainVariables = new List<MDomainVar>();

        public MDomain()
        {

        }
        
        public void Pulse()
        {
            
        }

        public void AddVariable(MDomainVar var)
        {
            if(domainVariables.Contains(var))
            {
                throw new ArgumentException("Failed to add domain variable: domain variable already present");
            }
            domainVariables.Add(var);
        }
    }
}
