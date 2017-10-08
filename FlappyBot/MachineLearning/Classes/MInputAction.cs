using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class MInputAction
    {
        public delegate void MInputActionDelegate();
        private MInputActionDelegate inputActionDelegate;
        public readonly string Id;

        public MInputAction(string id, MInputActionDelegate inputActionDelegate)
        {
            this.Id = id;
            this.inputActionDelegate = inputActionDelegate;
        }

        internal void PerformAction()
        {
            inputActionDelegate.Invoke();
        }
    }
}
