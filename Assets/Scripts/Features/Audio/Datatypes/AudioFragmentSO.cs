using UnityEngine;

[CreateAssetMenu(fileName = "Fragment-", menuName = "Audio/Fragment", order = 0)]
public class AudioFragmentSO: ScriptableObject
{
    public AudioClip Clip;
    [Range(0.0f, 1.0f)]
    public float Volume = 1;
    public bool Loop;
    public float FadeInDuration;
}