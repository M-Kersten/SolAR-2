using UnityEngine;

public class AudioController: MonoBehaviour
{
    [SerializeField]
    private AudioModel _audioModel;

    public void PlayClip(AudioFragmentSO clip)
    {
        _audioModel.PlayAudioClip(clip);
    }

    public void PlayIdentifier(AudioIdentifier identifier)
    {
        _audioModel.PlayAudioClipIdentifier(identifier);
    }
}