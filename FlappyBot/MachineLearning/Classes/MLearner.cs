using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class MLearner : IMPulsable
    {
        public FMTrainingData TrainingData { get; private set; }

        private List<MInputAction> inputActions = new List<MInputAction>();

        public MLearner()
        {

        }

        public void Pulse()
        {
            
        }

        public void AddInputAction(MInputAction inputAction)
        {
            if(inputActions.Contains(inputAction))
            {
                throw new ArgumentException("Failed to add input action: input action already present");
            }

            foreach(MInputAction action in inputActions)
            {
                if(inputAction.Id == action.Id)
                {
                    throw new ArgumentException(
                        string.Format("Failed to add input action: already an input action present with id '{0}' (id must be unique)", inputAction.Id));
                }
            }

            inputActions.Add(inputAction);
        }
    }
}
