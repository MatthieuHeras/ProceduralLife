using ProceduralLife.Map;
using ProceduralLife.Simulation;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralLife.Inputs
{
    public class InputManager : MonoBehaviour
    {
        // [TODO] Proper spawn/place
        [SerializeField]
        private SimulationEntityDefinition testEntity;
        
        [SerializeField]
        private SimulationEntityDefinition treeEntity;

        public static event Action SimulationChanged = delegate { };
        public static event Action<SimulationEntityDefinition, Vector2Int> SpawnEntityEvent = delegate { };
        
        [Button]
        public void AddSheepButton()
        {
            // [TODO] Proper spawn/place
            SpawnEntityEvent.Invoke(this.testEntity, new Vector2Int(0, 0));
            
            SimulationChanged.Invoke();
        }
        
        [Button]
        public void Add100SheepButton()
        {
            for (int i = 0; i < 100; i++)
                SpawnEntityEvent.Invoke(this.testEntity, new Vector2Int(0, 0));
            
            SimulationChanged.Invoke();
        }

        [Button]
        public void AddRandomTree()
        {
            Dictionary<Vector2Int, Tile> tiles = SimulationContext.MapData.Tiles;
            
            List<Vector2Int> availablePositions = new(tiles.Count);
            
            foreach ((Vector2Int pos, Tile tile) in tiles)
            {
                bool hasTree = false;
                
                foreach (SimulationEntity entity in tile.Entities)
                {
                    if (entity.Definition == this.treeEntity)
                    {
                        hasTree = true;
                        break;
                    }
                }
                
                if (!hasTree)
                    availablePositions.Add(pos);
            }
            
            SpawnEntityEvent.Invoke(this.treeEntity, availablePositions[Random.Range(0, availablePositions.Count)]);
            
            SimulationChanged.Invoke();
        }
    }
}