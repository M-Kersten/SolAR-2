using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using TMPro;

using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AnimatedText : MonoBehaviour
{
    public float AnimateSpeed;
    
    private TextMeshProUGUI _tmp;
    
    public void SetText(string newText)
    {
        if (_tmp == null)
            _tmp = GetComponent<TextMeshProUGUI>();
        
        _tmp.maxVisibleCharacters = 0;
        var textLength = newText.Length;
        _tmp.text = newText;

        DOVirtual.Int(0, textLength, textLength / AnimateSpeed, value => _tmp.maxVisibleCharacters = value);
    }
}
