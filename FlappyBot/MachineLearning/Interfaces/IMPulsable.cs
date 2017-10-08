
namespace MachineLearning
{
    /// <summary>
    /// Interface for objects that can be pulsed by the brain.
    /// </summary>
    public interface IMPulsable
    {
        /// <summary>
        /// Called by the brain.
        /// </summary>
        void Pulse();
    }
}
