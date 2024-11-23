using UnityEngine;

namespace Utils
{
    public static class Helper {
        public static double DistanceCalculator(Vector3 posA, Vector3 posB) {
            return Mathf.Sqrt((posB.x - posA.x)*(posB.x - posA.x) + 
                              (posB.y - posA.y)*(posB.y - posA.y) + (posB.z - posA.z)*(posB.z - posA.z));
        }

        public static float LoadFactorCalculation(float currentValue, float maxValue) {
            return currentValue / maxValue;
        }
        public static bool DirectViewBetweenTwoObject(GameObject origineObject, GameObject targetObject, bool displayRaycast) {
            RaycastHit hit;
            if (displayRaycast)
            {
                if(Physics.Raycast(origineObject.transform.position, 
                       targetObject.transform.position - origineObject.transform.position,out hit, 
                       Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore)){
                    Debug.DrawRay(origineObject.transform.position, 
                        (targetObject.transform.position - origineObject.transform.position) * hit.distance, Color.yellow);
                    Debug.Log("Did Hit");
                    if(hit.collider.gameObject == targetObject) return true;
                    return false;
                }
                else
                {
                    Debug.DrawRay(origineObject.transform.position, 
                        (targetObject.transform.position - origineObject.transform.position) * 1000, Color.white); 
                    Debug.Log("Did not Hit");
                    return false;
                }  
            }
            if(Physics.Raycast(origineObject.transform.position, 
                   targetObject.transform.position - origineObject.transform.position,out hit, 
                   Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore)){
                Debug.Log("Did Hit");
                if(hit.collider.gameObject == targetObject) return true;
                return false;
            }
            return false;
        
        }
    }
}