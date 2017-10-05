using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace FlappyBot.AI
{
    [XmlInclude(typeof(Vector2))]

    public class SerializedEnvironmentVariable
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Value")]
        public object Value { get; set; }
    }

    public class SerializedSample
    {
        [XmlElement("MaxDistance")]
        public float MaxDistance { get; set; }

        [XmlElement("Delay")]
        public float Delay { get; set; }

        [XmlElement("EnvironmentVar")]
        public List<SerializedEnvironmentVariable> Vars { get; set; }
        
        public float GetAverageDistanceToEnvironment()
        {
            List<float> distances = new List<float>();
            foreach(SerializedEnvironmentVariable serVar in Vars)
            {
                object envValue = Environment.Vars[serVar.Name];
                object serValue = serVar.Value;
                
                if(envValue.GetType().GetGenericArguments()[0] == typeof(float))
                {
                    distances.Add(Math.Abs(((EnvironmentVar<float>)envValue).GetValue() - (float)serValue));
                }
                else if(envValue.GetType().GetGenericArguments()[0] == typeof(Vector2))
                {
                    distances.Add(Vector2.Distance(((EnvironmentVar<Vector2>)envValue).GetValue(), ((Vector2)serValue)));
                }
                else
                {
                    throw new ArgumentException(string.Format("Value type '{0}' is not supported", envValue.GetType().Name));
                }
            }
            return distances.Average();
        }
    }

    [XmlRoot("TrainingData")]
    public class SerializedTrainingData
    {
        [XmlElement("Sample")]
        public List<SerializedSample> Samples { get; set; }
    }
}
