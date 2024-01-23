using System.Linq;

using UnityEngine;

public class AudioModel : MonoBehaviour
{
    public AudioCollection AudioCollection;
    public AudioListener RecordingAudioListener { get; private set; }
    public AudioPoolView AudioPoolView { get; set; }

    public AudioSource BackgroundTrackSource;
    
    public delegate void ClipEventHandler(AudioFragmentSO fragment);
    public event ClipEventHandler PlayClipHandler;

    public void RegisterAudioListener(AudioListener listener)
    {
        RecordingAudioListener = listener;
    }

    internal void PlayAudioClipIdentifier(AudioIdentifier identifier)
    {
        var fragment = AudioCollection.Fragments.First(fragment => fragment.Identifier == identifier).AudioFragmentObject;
        PlayClipHandler?.Invoke(fragment);
    }
    
    internal void PlayAudioClip(AudioFragmentSO fragment) => PlayClipHandler?.Invoke(fragment);
}
