using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBot.AI
{
    public class Policy
    {
        public const uint MinSampleCount = 4;
        public const float InitialMaxSampleDistance = 5.0f;

        private List<Sample> samples = new List<Sample>();
        private Random rand;
        
        public Policy()
        {
            rand = new Random(Guid.NewGuid().GetHashCode());
        }

        public float GetNextDelay()
        {
            Learner.CurrentSample = null;
            /* Not enough samples in generation */
            if(Learner.TrainingData.Count < MinSampleCount)
            {
                Log.Warning("Using randomized delay (not enough samples in training data)");
                return (float)rand.NextDouble();
            }

            float closest = float.PositiveInfinity;
            SerializedSample closestSample = null;
            for (int i = 0; i < Learner.TrainingData.Count; i++)
            {
                float d = Learner.TrainingData[i].GetAverageDistanceToEnvironment();
                if(d < closest)
                {
                    closest = d;
                    closestSample = Learner.TrainingData[i];
                }
            }

            if (closest > closestSample.MaxDistance)
            {
                Log.Warning("No sample close enough, applying mutation to closest sample");
                return(float)(rand.NextDouble());
            }

            if (closestSample == null)
            {
                Log.Warning("Using randomized delay (no sample found)");
                return (float)rand.NextDouble();
            }

            Log.Success(string.Format("Using closest sample delay from training data (d = {0})", closest));
            Learner.CurrentSample = closestSample;
            return closestSample.Delay;
        }
    }
}
