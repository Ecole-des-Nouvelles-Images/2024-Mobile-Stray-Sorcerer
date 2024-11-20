using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper {
    public static double DistanceCalculator(Vector3 posA, Vector3 posB) {
        return Mathf.Sqrt((posB.x - posA.x)*(posB.x - posA.x) + 
            (posB.y - posA.y)*(posB.y - posA.y) + (posB.z - posA.z)*(posB.z - posA.z));
    }

    public static float LoadFactorCalculation(float currentValue, float maxValue) {
        return currentValue / maxValue;
    }
}
