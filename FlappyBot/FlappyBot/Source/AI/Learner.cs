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
        public Vector2 NextGapPosition { get; set; }

        private List<Action> inputActions = new List<Action>();
        private Environment environment;

        public Learner(Environment environment)
        {
            this.environment = environment;
        }

        public void AddInputAction(Action action)
        {
            if (!inputActions.Contains(action))
            {
                inputActions.Add(action);
            }
        }
        
    }
}
