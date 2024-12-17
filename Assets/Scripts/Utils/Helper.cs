using UnityEngine;

namespace Utils
{
    public static class Helper
    {
        public static double DistanceCalculator(Vector3 posA, Vector3 posB)
        {
            return Mathf.Sqrt((posB.x - posA.x) * (posB.x - posA.x) +
                              (posB.y - posA.y) * (posB.y - posA.y) + (posB.z - posA.z) * (posB.z - posA.z));
        }

        public static bool DirectViewBetweenTwoObject(GameObject origineObject, GameObject targetObject, bool displayRaycast)
        {
            RaycastHit hit;
            if (displayRaycast)
            {
                if (Physics.Raycast(origineObject.transform.position,
                        targetObject.transform.position - origineObject.transform.position, out hit,
                        Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    Debug.DrawRay(origineObject.transform.position,
                        (targetObject.transform.position - origineObject.transform.position) * hit.distance, Color.yellow);
                    if (hit.collider.gameObject == targetObject) return true;
                    return false;
                }
                else
                {
                    Debug.DrawRay(origineObject.transform.position,
                        (targetObject.transform.position - origineObject.transform.position) * 1000, Color.white);
                    return false;
                }
            }

            if (Physics.Raycast(origineObject.transform.position, targetObject.transform.position - origineObject.transform.position, out hit,
                    Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore)
                && hit.collider.gameObject == targetObject)
                return true;
            return false;
        }
    }
}