using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEngine;

public static class AsyncService
{
    public static void ActionAfterFrame(this MonoBehaviour monoBehaviour, Action onComplete)
    {
        monoBehaviour.StartCoroutine(ActionAfterFrameRoutine(onComplete));
    }

    private static IEnumerator ActionAfterFrameRoutine(Action onComplete)
    {
        yield return new WaitForEndOfFrame();
        onComplete?.Invoke();
    }

    /// <summary>
    /// Here's an example of how to use this method from a MonoBehaviour:
    /// this.WaitForCondition(() => localvalue == true/100/whatever, () => { do something });
    /// </summary>
    public static void WaitForCondition(this MonoBehaviour monoBehaviour, Func<bool> condition, Action onConditionMet)
    {
        monoBehaviour.StartCoroutine(ActionAfterCondition(condition, onConditionMet));
    }

    private static IEnumerator ActionAfterCondition(Func<bool> condition, Action onConditionMet)
    {
        yield return new WaitUntil(condition);
        onConditionMet?.Invoke();
    }
    
    public static void ActionAfterSeconds(this MonoBehaviour monoBehaviour, Action onComplete, float seconds)
    {
        monoBehaviour.StartCoroutine(ActionAfterSecondsRoutine(onComplete, seconds));
    }
    
    private static IEnumerator ActionAfterSecondsRoutine(Action onComplete, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onComplete?.Invoke();
    }
}