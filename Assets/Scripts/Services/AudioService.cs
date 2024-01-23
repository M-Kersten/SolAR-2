using UnityEngine;

public static class AudioService
{
    public static void PlayAudio(AudioIdentifier identifier)
    {
        GameObject.FindObjectOfType<AudioController>().PlayIdentifier(identifier);
    }
}
