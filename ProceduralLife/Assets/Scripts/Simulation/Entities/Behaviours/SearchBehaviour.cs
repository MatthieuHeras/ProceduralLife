namespace ProceduralLife.Simulation
{
    public class SearchBehaviour : ABehaviour
    {
        public SearchBehaviour(BehaviourContext context, SearchBehaviourParameter parameter) : base(context)
        {
            this.parameter = parameter;
        }

        private enum SearchState
        {
            LOOKING,
            CHASING
        }
        
        private readonly SearchBehaviourParameter parameter;
        private SearchState searchState = SearchState.LOOKING;
        private SimulationEntity target = null;
        
        protected override AState GetNewState()
        {
            return null;/*
            switch (this.searchState)
            {
                case SearchState.LOOKING:
                    bool foundTarget = this.TryFindTarget()
                    break;
                case SearchState.CHASING:
                    break;
            } */
        }

        /*private bool TryFindTarget()
        {
            SimulationContext.MapData.Tiles
            this.entity.SightRange
        }*/
    }
}