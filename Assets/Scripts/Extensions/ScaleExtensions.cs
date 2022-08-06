using UnityEngine;

public static class ScaleExtensions
{
    public static void ScaleFromPivot(this GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.transform.localPosition;
        Vector3 B = pivot;
        Vector3 C = A - B;

        float RS = newScale.x / target.transform.localScale.x;

        // calc final position post-scale
        Vector3 FP = B + C * RS;

        // finally, actually perform the scale/translation
        target.transform.localScale = newScale;
        target.transform.localPosition = FP;
    }
}