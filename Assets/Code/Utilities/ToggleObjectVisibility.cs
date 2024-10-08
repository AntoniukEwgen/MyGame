using UnityEngine;

namespace Vampire
{
    public class ManagerObjectVisibility : MonoBehaviour
    {
        private GameObject targetObject; // Целевой объект, который будет включаться и выключаться

        void Update()
        {
            if (IsVisibleFrom(Camera.main))
            {
                this.targetObject.SetActive(true);
            }
            else
            {
                this.targetObject.SetActive(false);
            }
        }

        bool IsVisibleFrom(Camera camera)
        {
            if (this.targetObject == null) return false;

            Renderer objectRenderer = this.targetObject.GetComponent<Renderer>();
            if (objectRenderer == null) return false;

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, objectRenderer.bounds);
        }
    }
}

