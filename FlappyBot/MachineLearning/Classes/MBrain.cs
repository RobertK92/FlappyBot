using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MachineLearning
{
    /// <summary>
    /// Singleton that manages and pulses all domains and learners.
    /// </summary>
    public class MBrain
    {
        private static MBrain instance;
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static MBrain Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new MBrain();
                }
                return instance;
            }
        }

        /// <summary>
        /// Wheter or not the brain is currently pulsing.
        /// </summary>
        public bool IsPulsing { get; private set; }


        private float pulseRate;
        /// <summary>
        /// The (normalized) rate at which the brain pulses. <para />
        /// 1.0f means without sleeping the thread at all. 
        /// 0.0f means a 100 milisecond thread sleep.
        /// </summary>
        public float PulseRate 
        {
            get { return pulseRate; }
            set
            {
                if(value > 1.0f || value < 0.0f)
                {
                    throw new ArgumentException("Failed to set pulse rate: must not be bigger than 1.0f or smaller than 0.0f");
                }
                pulseRate = value;
            }
        }

        public int LearnerCount => learners.Count;
        public int DomainCount => domains.Count;

        internal List<MLearner> learners = new List<MLearner>();
        internal List<MDomain> domains = new List<MDomain>();
        internal Thread pulseThread;

        private MBrain()
        {
            pulseThread = new Thread(new ThreadStart(Pulse))
            {
                Name = "PulseThread"
            };
            PulseRate = 1.0f;
        }

        private void Pulse()
        {
            while(IsPulsing)
            {
                foreach(IMPulsable domain in domains)
                {
                    domain.Pulse();
                }

                foreach(IMPulsable learner in learners)
                {
                    learner.Pulse();
                }

                float sleepDelay = 1.0f - Math.Min(PulseRate, 1.0f);
                Thread.Sleep((int)(sleepDelay * 100));
            }
        }

        /// <summary>
        /// Starts the pulse thread.
        /// </summary>
        public void StartPulsing()
        {
            if (IsPulsing)
            {
                throw new Exception("Failed to start pulsing: brain is already pulsing");
            }

            IsPulsing = true;
            pulseThread.Start();
        }

        /// <summary>
        /// Stops the pulse thread. <para />
        /// The pulse thread will finish its current execution before terminating.
        /// </summary>
        public void StopPulsing()
        {
            if(!IsPulsing)
            {
                throw new Exception("Failed to stop pulsing: brain is not currently pulsing");
            }
            IsPulsing = false;
        }

        /// <summary>
        /// Calls UnRegisterAllLearners and UnRegisterAllDomains respectively.
        /// </summary>
        public void UnRegisterAllDomainsAndLearners()
        {
            UnRegisterAllLearners();
            UnRegisterAllDomains();
        }

        /// <summary>
        /// Clears the learner array.
        /// </summary>
        public void UnRegisterAllLearners()
        {
            learners.Clear();
        }

        /// <summary>
        /// Clears the domain array.
        /// </summary>
        public void UnRegisterAllDomains()
        {
            domains.Clear();
        }

        /// <summary>
        /// Registers a new learner to be taken into account on the next pulse.
        /// </summary>
        /// <param name="learner"></param>
        public void RegisterLearner(MLearner learner)
        {
            if(learners.Contains(learner))
            {
                throw new ArgumentException("Failed to register learner: learner was already registered");
            }
            learners.Add(learner);
        }

        /// <summary>
        /// UnRegisteres a learner, the learner will no longer be pulsed.
        /// </summary>
        /// <param name="learner"></param>
        public void UnRegisterLearner(MLearner learner)
        {
            if(!learners.Contains(learner))
            {
                throw new ArgumentException("Failed to un-register learner: learner was not registered");
            }
            learners.Remove(learner);
        }

        /// <summary>
        /// Registeres a new domain to be taken into account on the next pulse. <para />
        /// All learners using this domain will start receiving information from it.
        /// </summary>
        /// <param name="domain"></param>
        public void RegisterDomain(MDomain domain)
        {
            if(domains.Contains(domain))
            {
                throw new ArgumentException("Failed to register domain: domain was already registered");
            }
            domains.Add(domain);
        }

        /// <summary>
        /// UnRegisteres a domain, the domain will no longer be pulsed. <para />
        /// All learners using this domain will no longer receive information from it.
        /// </summary>
        /// <param name="domain"></param>
        public void UnRegisterDomain(MDomain domain)
        {
            if(!domains.Contains(domain))
            {
                throw new ArgumentException("Failed to un-register domain: domain was not registered");
            }
            domains.Add(domain);
        }
    }
}
