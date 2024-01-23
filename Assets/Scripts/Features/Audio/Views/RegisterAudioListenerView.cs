using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class RegisterAudioListenerView: MonoBehaviour
{
    private AudioListener _listener;
    private AudioModel _audioModel;

    private void Awake()
    {
        _audioModel = FindObjectOfType<AudioModel>();
        _listener = GetComponent<AudioListener>();
        _audioModel.RegisterAudioListener(_listener);
    }
}