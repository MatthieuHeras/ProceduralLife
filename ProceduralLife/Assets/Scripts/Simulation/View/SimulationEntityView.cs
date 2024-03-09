using MHLib.Hexagon;
using UnityEngine;

namespace ProceduralLife.Simulation.View
{
    public class SimulationEntityView : MonoBehaviour
    {
        private bool isMoving = false;
        private ulong moveStartMoment;
        private ulong moveEndMoment;
        private Vector3 moveStartPosition = Vector3.zero;
        private Vector3 moveEndPosition = Vector3.zero;
        
        private SimulationEntity entity;
        
        public void Init(SimulationEntity newEntity)
        {
            this.entity = newEntity;
            this.HookToEntity();
        }
        
        private void UpdateTime(ulong oldTime, ulong newTime)
        {
            if (!this.isMoving)
                return;
            
            this.transform.position = MHLib.Math.Remap(this.moveStartMoment, this.moveEndMoment, this.moveStartPosition, this.moveEndPosition, newTime);
        }
        
        private void OnMoveStart(Vector2Int newPosition, ulong startMoment, ulong duration)
        {
            this.isMoving = true;
            
            this.moveStartMoment = startMoment;
            this.moveEndMoment = startMoment + duration;
            
            Vector3 worldPosition = HexagonHelper.TileToWorld(newPosition, Constants.TILE_SIZE);
            
            Transform selfTransform = this.transform;
            
            this.moveStartPosition = selfTransform.position;
            this.moveEndPosition = worldPosition;

            Vector3 positionDiff = worldPosition - this.moveStartPosition;
            float rotation = Mathf.Atan2(positionDiff.z, positionDiff.x) / Mathf.PI * 180f;
            selfTransform.rotation = Quaternion.Euler(0f, -rotation, 0f);
        }
        
        private void OnMoveEnd(Vector2Int newPosition)
        {
            this.isMoving = false;
            
            Vector3 worldPosition = HexagonHelper.TileToWorld(newPosition, Constants.TILE_SIZE);
            this.transform.position = worldPosition;
        }
        
        private void OnMoveStartBackward(Vector2Int oldPosition, ulong startMoment, ulong duration)
        {
            this.isMoving = true;
            
            this.moveStartMoment = startMoment;
            this.moveEndMoment = startMoment - duration;
            
            Vector3 worldPosition = HexagonHelper.TileToWorld(oldPosition, Constants.TILE_SIZE);
            
            Transform selfTransform = this.transform;
            
            this.moveStartPosition = selfTransform.position;
            this.moveEndPosition = worldPosition;

            Vector3 positionDiff = this.moveStartPosition - worldPosition;
            float rotation = Mathf.Atan2(positionDiff.z, positionDiff.x) / Mathf.PI * 180f;
            selfTransform.rotation = Quaternion.Euler(0f, -rotation, 0f);
        }
        
        private void OnMoveEndBackward(Vector2Int oldPosition)
        {
            this.isMoving = false;
            
            Vector3 worldPosition = HexagonHelper.TileToWorld(oldPosition, Constants.TILE_SIZE);
            this.transform.position = worldPosition;
        }
        
        private void HookToEntity()
        {
            this.entity.MoveStartEvent += this.OnMoveStart;
            this.entity.MoveStartBackwardEvent += this.OnMoveStartBackward;
            this.entity.MoveEndEvent += this.OnMoveEnd;
            this.entity.MoveEndBackwardEvent += this.OnMoveEndBackward;
            SimulationTime.CurrentTimeChanged += this.UpdateTime;
        }
        
        private void UnhookToEntity()
        {
            this.entity.MoveStartEvent -= this.OnMoveStart;
            this.entity.MoveStartBackwardEvent -= this.OnMoveStartBackward;
            this.entity.MoveEndEvent -= this.OnMoveEnd;
            this.entity.MoveEndBackwardEvent -= this.OnMoveEndBackward;
            SimulationTime.CurrentTimeChanged -= this.UpdateTime;
        }
        
        private void OnEnable()
        {
            if (this.entity == null)
                return;
            
            this.UnhookToEntity();
            this.HookToEntity();
        }
        
        private void OnDisable()
        {
            this.UnhookToEntity();
        }
    }
}