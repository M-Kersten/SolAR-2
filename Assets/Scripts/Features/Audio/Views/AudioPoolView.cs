using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class AudioPoolView : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSourcePrefab;
    
    private ObjectPool<AudioSource> _pool;
    private AudioModel _audioModel;

    private void Awake()
    {
        _audioModel = FindObjectOfType<AudioModel>();
        _audioModel.AudioPoolView = this;
        _audioModel.PlayClipHandler += PlayFragment;
    }

    private void Start()
    {
        _pool = new ObjectPool<AudioSource>(
            () => Instantiate(audioSourcePrefab, transform),
            source =>
            {
                source.gameObject.SetActive(true);
            },
            source =>
            {
                source.gameObject.SetActive(false);
            },
            source =>
            {
                Destroy(source.gameObject);
            }, false, 5, 10);
    }


    private void PlayFragment(AudioFragmentSO fragment)
    {
        var source = _pool.Get();
        source.clip = fragment.Clip;
        source.volume = fragment.Volume;
        source.loop = fragment.Loop;
        source.Play();

        if (fragment.FadeInDuration > 0)
        {
            source.volume = 0;
            DOVirtual.Float(0, fragment.Volume, fragment.FadeInDuration, value =>
            {
                source.volume = value;
            });
        }

        if (!source.loop)
            this.ActionAfterSeconds(() => _pool.Release(source), source.clip.length);
    }
}
