using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Dialog", order = 0)]
public class DialogItem: ScriptableObject
{
    [TextArea]
    public string Text;
    public BotAnimation Animation;
    public AudioClip Sound;
    public float CustomDuration;
    public bool EndlessDuration;
}