using DG.Tweening;

using UnityEngine;

public class DialogController : MonoBehaviour
{
    public DialogModel DialogModel;

    public void Greet()
    {
        DialogModel.StartTalking(DialogModel.GreetDialog);
    }

    public void ExplainPlacement()
    {
        DialogModel.StartTalking(DialogModel.ExplainPlacementDialog);
    }

    public void WellDone()
    {
        DialogModel.StartTalking(DialogModel.WellDoneDialog);
    }

    public void Visit(string planetName)
    {
        var visitDialog = DialogModel.VisitDialog;
        var dialog = ScriptableObject.CreateInstance<DialogItem>();
        dialog.Text = visitDialog.Text;
        dialog.Animation = visitDialog.Animation;
        dialog.Sound = visitDialog.Sound;
        dialog.Text = dialog.Text.Replace("[Planet]", planetName);
        DOVirtual.DelayedCall(.7f, () => DialogModel.StartTalking(dialog));
    }
    
    public void Talk(DialogItem dialog)
    {
        DialogModel.StartTalking(dialog);
    }
}
