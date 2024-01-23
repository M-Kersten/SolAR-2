using UnityEngine;

public static class AutoReferenceBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute() => Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("AutoReferenceService")));
}