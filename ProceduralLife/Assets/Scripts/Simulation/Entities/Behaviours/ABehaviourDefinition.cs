using MHLib.ConfigurableSO;
using System;

namespace ProceduralLife.Simulation
{
    [Serializable]
    public abstract class ABehaviourDefinition : AConfigurableSO
    {
        public abstract ABehaviour GetBehaviour(BehaviourContext context, object parameter);
    }
}