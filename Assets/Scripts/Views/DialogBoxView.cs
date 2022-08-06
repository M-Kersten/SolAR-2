using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogBoxView : MonoBehaviour
{
    public CanvasGroup TalkBox;
    public TMP_Text DialogueText;
    
    private DialogModel _dialogModel;
    private Camera _arCamera;

    private void Start()
    {
        _arCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        TalkBox.alpha = 0;
        _dialogModel = FindObjectOfType<DialogModel>();
        if (_dialogModel)
            _dialogModel.OnTalk += Talk;
    }
    
    public void Talk(DialogItem dialog)
    {
        TalkBox.alpha = 0;
        DialogueText.text = dialog.Text;
        DOTween.Sequence()
            .Append(TalkBox.DOFade(1, .5f))
            .AppendInterval(dialog.Text.Length * .1f)
            .Append(TalkBox.DOFade(0, .5f));
    }

    void Update()
    {
        transform.rotation = _arCamera.transform.rotation;
    }
}
