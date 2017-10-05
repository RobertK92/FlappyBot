using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBot.AI
{
    public static class Environment 
    {
        internal static Dictionary<string, object> Vars { get; private set; } = new Dictionary<string, object>();
        
        public static void ClearVars()
        {
            Vars.Clear();
        }

        public static void AddVar<T>(string name, EnvironmentVar<T> var)
        {
            Vars.Add(name, var);
        }

        public static EnvironmentVar<T> GetVar<T>(string name)
        {
            if (!Vars.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("No EnvironmentVar found with name '{0}'", name));
            }

            object var = Vars[name];

            if(var.GetType() != typeof(EnvironmentVar<T>))
            {
                throw new ArgumentException(string.Format("EnvironmentVar with name '{0}' was found but is not of type '{1}', it is of type '{2}'",
                    name, typeof(T).Name, var.GetType().Name));
            }

            return (EnvironmentVar<T>)var;
        }
    }
}
