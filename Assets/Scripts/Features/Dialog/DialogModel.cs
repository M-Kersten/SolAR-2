using UnityEngine;

public class DialogModel : MonoBehaviour
{
    public delegate void Eventhandler(DialogItem dialog);
    public event Eventhandler OnTalk;

    public DialogItem GreetDialog;
    public DialogItem ExplainPlacementDialog;
    public DialogItem VisitDialog;
    public DialogItem WellDoneDialog;
    
    public void StartTalking(DialogItem dialog)
    {
        OnTalk?.Invoke(dialog);
    }
}