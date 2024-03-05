using Sirenix.OdinInspector;
using UnityEngine;

namespace ProceduralLife
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField, Required]
        private Transform cameraTransform;

        [SerializeField, Required]
        private float speed = 1f;

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
                this.cameraTransform.position += Time.deltaTime * this.speed * Vector3.forward;
            else if (Input.GetKey(KeyCode.S))
                this.cameraTransform.position -= Time.deltaTime * this.speed * Vector3.forward;
            if (Input.GetKey(KeyCode.A))
                this.cameraTransform.position -= Time.deltaTime * this.speed * Vector3.right;
            else if (Input.GetKey(KeyCode.D))
                this.cameraTransform.position += Time.deltaTime * this.speed * Vector3.right;
        }
    }
}