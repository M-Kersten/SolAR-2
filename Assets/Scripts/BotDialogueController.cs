using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using TMPro;

using UnityEngine;

public class BotDialogueController : MonoBehaviour
{
    public CanvasGroup TalkBox;
    public TMP_Text DialogueText;
    public Animator BotAnimator;
    
    private static readonly int _isTalking = Animator.StringToHash("Do_Talking_Head_Hands_Body");
    
    public void Talk(string text)
    {
        TalkBox.alpha = 0;
        DialogueText.text = text;
        BotAnimator.SetTrigger(_isTalking);
        TalkBox.DOFade(1, .5f);
        TalkBox.DOFade(0, .5f).SetDelay(text.Length * .1f);
    }
}
