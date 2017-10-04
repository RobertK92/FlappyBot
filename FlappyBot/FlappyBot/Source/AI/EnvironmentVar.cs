using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBot.AI
{
    public class EnvironmentVar<T>
    {
        public delegate T Getter();

        private Getter getter;

        public EnvironmentVar(Getter getter) 
        {
            this.getter = getter;
        }

        public T GetValue()
        {
            return getter();
        }

        
    }
}
