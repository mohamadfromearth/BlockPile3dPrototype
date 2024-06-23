using UnityEngine;

namespace Utils
{
    public class PositionConverters
    {
        public static Vector3? ScreenToWorldPosition(Vector2 screenPosition, Camera camera, LayerMask layers)
        {
            Ray ray = camera.ScreenPointToRay(screenPosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layers))
            {
                return hit.point;
            }

            return null;
        }
    }
}