using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBot.AI
{
    public struct Sample
    {
        public float Rating { get; set; }

        public float Delay { get; set; }
        public readonly Dictionary<string, object> Vars;

        public Sample(float value, Dictionary<string, object> vars)
        {
            Delay = value;
            Vars = vars;
            Rating = 0.0f;
        }
    }
}
