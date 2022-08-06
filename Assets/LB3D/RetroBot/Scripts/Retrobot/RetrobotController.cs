using System;

using DG.Tweening;

using UnityEngine;

public class RetrobotController : MonoBehaviour
{
    public Camera PlayerCamera;
    public Transform PlayerPosition;
    public Animator animator;
    public BotDialogueController DialogueController;
    public string Greeting;
    
    private Vector3 velocity;
    private Vector3 objectPosition;
    
    private static readonly int _motionX = Animator.StringToHash("Motion_X");
    private static readonly int _motionY = Animator.StringToHash("Motion_Y");
    private static readonly int _idlePattern = Animator.StringToHash("Idle_Pattern");
    private static readonly int _isFighting = Animator.StringToHash("Is_Fighting");
    private static readonly int _flip = Animator.StringToHash("Do_Flip_Happy");
    private static readonly int _yes = Animator.StringToHash("Do_Nod_Yes");
    private static readonly int _giggle = Animator.StringToHash("Do_Giggle");
    

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat(_motionX, 0);
        animator.SetFloat(_motionY, 0);
    }

    private void OnEnable()
    {
        transform.position = PlayerPosition.position;
        var originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        animator.SetTrigger(_flip);
        transform.DOScale(originalScale, 1f).SetEase(Ease.InOutSine);

        DOVirtual.DelayedCall(4, () => { DialogueController.Talk(Greeting); });
    }

    void OnMouseDown()
    {
        animator.SetTrigger(_giggle);
    }
    
    public void Talk(string text)
    {
        animator.SetTrigger(_yes);
        DialogueController.Talk(text);
    }

    private void Update()
    {
        var distance = transform.position - PlayerPosition.position;
        if (distance.magnitude > .2f)
        {
            transform.position = Vector3.Lerp(transform.position, PlayerPosition.position, Time.deltaTime * 2);
        }
        
        var realVelocity = transform.InverseTransformDirection(transform.position - objectPosition) / Time.deltaTime;
        velocity = Vector3.Lerp(velocity, realVelocity, Time.deltaTime * 5);
        if (velocity.magnitude < .01f)
            velocity = Vector3.zero;
        
        objectPosition = transform.position;
        animator.SetFloat(_motionX, velocity.x * .75f);
        animator.SetFloat(_motionY, velocity.z * .75f);
        
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            180 + PlayerCamera.transform.eulerAngles.y,
            0);
    }
    
    public void DoAction(string action) {
        animator.SetTrigger(action);
    }
    public void SetMotionXState(float value) {
        animator.SetFloat(_motionX, value);
    }
    public void SetMotionYState(float value)
    {
        animator.SetFloat(_motionY, value);
    }

    public void SetIdleState(float value) {
        animator.SetFloat(_idlePattern, value);
    }

    public void SetFightIdle(bool isFighting) {
        animator.SetBool(_isFighting, isFighting);
    }
}
