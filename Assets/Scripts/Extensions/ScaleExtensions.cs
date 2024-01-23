using UnityEngine;

public static class ScaleExtensions
{
    public static void ScaleFromPivot(this GameObject target, Vector3 pivot, Vector3 newScale)
    {
        var A = target.transform.localPosition;
        var B = pivot;
        var C = A - B;

        var RS = newScale.x / target.transform.localScale.x;

        // calc final position post-scale
        var FP = B + C * RS;

        // finally, actually perform the scale/translation
        target.transform.localScale = newScale;
        target.transform.localPosition = FP;
    }
}