using MHLib.ConfigurableSO;
using System;

namespace ProceduralLife.Simulation
{
    [Serializable]
    public class BehaviourField : AConfigurableField<ABehaviourDefinition>
    {
        public ABehaviour GetBehaviour(BehaviourContext context) => this.configurableSO.GetBehaviour(context, this.parameter);
        
        protected override string configurableSOLabel => "Behaviour";
    }
}