using MHLib.Hexagon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife.Simulation.View
{
    public class SimulationEntityView : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject viewParent = null;
        
        [SerializeField, Required]
        private Transform hungerTransform = null;
        
        private bool isMoving = false;
        private ulong moveStartMoment;
        private ulong moveEndMoment;
        private Vector3 moveStartPosition = Vector3.zero;
        private Vector3 moveEndPosition = Vector3.zero;
        private Vector3 originalHungerScale;
        
        private SimulationEntity entity;
        private bool hooked = false;
        
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
        
        private void OnMoveStart(Vector2Int newPosition, ulong startMoment, ulong duration, bool forward)
        {
            this.isMoving = true;
            
            this.moveStartMoment = startMoment;
            this.moveEndMoment = forward ? (startMoment + duration) : (startMoment - duration);
            
            Vector3 worldPosition = HexagonHelper.TileToWorld(newPosition, Constants.TILE_SIZE);
            
            Transform selfTransform = this.transform;
            
            this.moveStartPosition = selfTransform.position;
            this.moveEndPosition = worldPosition;
            
            Vector3 positionDiff = worldPosition - this.moveStartPosition;
            float rotation = (Mathf.Atan2(positionDiff.z, positionDiff.x) + (forward ? 0f : Mathf.PI)) / Mathf.PI * 180f;
            selfTransform.rotation = Quaternion.Euler(0f, -rotation, 0f);
        }
        
        private void OnMoveEnd(Vector2Int newPosition)
        {
            this.isMoving = false;
            
            Vector3 worldPosition = HexagonHelper.TileToWorld(newPosition, Constants.TILE_SIZE);
            this.transform.position = worldPosition;
        }

        private void OnBirth(bool timeIsForward)
        {
            this.viewParent.SetActive(timeIsForward);
        }
        
        private void OnDeath(bool timeIsForward)
        {
            this.viewParent.SetActive(!timeIsForward);
        }
        
        private void OnHungerChanged()
        {
            float hungerRatio = (float)this.entity.Hunger / (float)this.entity.Definition.MaxHunger;
            hungerRatio = 0.5f + (hungerRatio / 2f);
            this.hungerTransform.localScale = this.originalHungerScale * hungerRatio;
        }
        
        private void HookToEntity()
        {
            if (this.hooked)
                return;
            
            this.entity.MoveStartEvent += this.OnMoveStart;
            this.entity.MoveEndEvent += this.OnMoveEnd;
            this.entity.BirthEvent += this.OnBirth;
            this.entity.DeathEvent += this.OnDeath;
            this.entity.HungerChanged += this.OnHungerChanged;
            
            SimulationTime.CurrentTimeChanged += this.UpdateTime;

            this.hooked = true;
        }
        
        private void UnhookToEntity()
        {
            if (!this.hooked)
                return;
            
            this.entity.MoveStartEvent -= this.OnMoveStart;
            this.entity.MoveEndEvent -= this.OnMoveEnd;
            SimulationTime.CurrentTimeChanged -= this.UpdateTime;
            this.entity.BirthEvent -= this.OnBirth;
            this.entity.DeathEvent -= this.OnDeath;
            this.entity.HungerChanged -= this.OnHungerChanged;
            
            this.hooked = false;
        }
        
        #region UNITY METHODS
        private void Awake()
        {
            this.viewParent.SetActive(false);
            this.originalHungerScale = this.hungerTransform.localScale;
        }

        private void OnEnable()
        {
            if (this.entity == null)
                return;
            
            this.HookToEntity();
        }
        
        private void OnDisable()
        {
            this.UnhookToEntity();
        }
        #endregion UNITY METHODS

    }
}