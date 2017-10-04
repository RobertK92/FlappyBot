using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBot.AI
{
    public class Environment 
    {
        private Dictionary<string, object> varMap = new Dictionary<string, object>();

        public Environment()
        {
            
        }

        public void AddVar<T>(string name, EnvironmentVar<T> var)
        {
            varMap.Add(name, var);
        }

        public EnvironmentVar<T> GetVar<T>(string name)
        {
            if (!varMap.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("No EnvironmentVar found with name '{0}'", name));
            }

            object var = varMap[name];

            if(var.GetType() != typeof(EnvironmentVar<T>))
            {
                throw new ArgumentException(string.Format("EnvironmentVar with name '{0}' was found but is not of type '{1}', it is of type '{2}'",
                    name, typeof(T).Name, var.GetType().Name));
            }

            return (EnvironmentVar<T>)var;
        }
    }
}
