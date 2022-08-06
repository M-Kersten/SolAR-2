using UnityEngine;

public static class BotAnimationExtension
{
    public static int AnimationId(this DialogItem dialog)
    {
        return Animator.StringToHash(dialog.Animation.ToString());
    }
}