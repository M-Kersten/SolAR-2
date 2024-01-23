using UnityEngine;

[CreateAssetMenu(fileName = "AudioCollection-", menuName = "Audio/FragmentCollection", order = 0)]
public class AudioCollection : ScriptableObject
{
    public AudioFragment[] Fragments;
}
