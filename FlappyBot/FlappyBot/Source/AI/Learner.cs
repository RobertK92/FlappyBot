using Microsoft.Xna.Framework;
using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBot.AI
{
    public class Learner : BaseObject
    {
        /* static to persist through level loads (should change the instanceiation design instead) */
        public static List<SerializedSample> TrainingData { get; private set; } = new List<SerializedSample>();

        public Policy Policy { get; private set; }
        public uint GenerationSize { get; set; } = 6;
        public Action InputAction { get; set; }
        public static SerializedSample CurrentSample;

        private bool running;
        
        public Learner()
        {
            Policy = new Policy();
        }

        public void Start()
        {
            running = true;
            Log.Message("Learner started");
            RunAsyncLoop();
        }

        public void Stop()
        {
            running = false;
        }

        private async void RunAsyncLoop()
        {
            bool first = true;
            float lastDelay = 0.0f;
            int samplesCollected = 0;
            List<SerializedEnvironmentVariable> lastVars = new List<SerializedEnvironmentVariable>();
            object locker = new object();

            while (running)
            {
                if (!first)
                {
                    lock (locker)
                    {
                        SerializedSample sample = new SerializedSample()
                        {
                            Delay = lastDelay,
                            Vars = lastVars,
                            MaxDistance = Policy.InitialMaxSampleDistance
                        };

                        TrainingData.Add(sample);
                        
                        samplesCollected++;
                        Log.Message(string.Format("Adding sample to training data (len = {0})", TrainingData.Count));
                    }
                }

                lastDelay = Policy.GetNextDelay();
                lastVars.Clear();
                foreach (KeyValuePair<string, object> envVar in Environment.Vars)
                {
                    object envValue = null;
                    if (envVar.Value.GetType().GetGenericArguments()[0] == typeof(float))
                    {
                         envValue = ((EnvironmentVar<float>)envVar.Value).GetValue();
                    }
                    else if (envVar.Value.GetType().GetGenericArguments()[0] == typeof(Vector2))
                    {
                        envValue = ((EnvironmentVar<Vector2>)envVar.Value).GetValue();
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Value type '{0}' is not supported", envVar.Value.GetType().GetGenericArguments()[0].Name));
                    }

                    lastVars.Add(new SerializedEnvironmentVariable()
                    {
                        Name = envVar.Key,
                        Value = envValue
                    });
                }
                
                await PerformActionWithDelay(lastDelay);
                first = false;
            }

            Log.Message(string.Format("Learner stopped ({0} samples collected and added to training data", samplesCollected));
        }

        private async Task PerformActionWithDelay(float delay)
        {
            await Task.Delay((int)(delay * 1000));
            InputAction();
        }
    }
}
