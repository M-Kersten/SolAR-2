using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TutorialBotView : MonoBehaviour
{
    [SerializeField]
    private Camera _tutorialCamera;
    [SerializeField]
    private Transform _lookAtTransform;
    
    private TutorialModel _tutorialModel;
    protected TutorialController _tutorialController;
    private Animator animator;
    
    private Vector3 velocity;
    private Vector3 objectPosition;
    private Vector3 _originalPosition;
    
    private static readonly int _motionX = Animator.StringToHash("Motion_X");
    private static readonly int _motionY = Animator.StringToHash("Motion_Y");
    
    void Start()
    {
        _tutorialController = FindObjectOfType<TutorialController>();
        _tutorialModel = FindObjectOfType<TutorialModel>();

        _tutorialModel.OnTutorialStart += AnimateTutorial;
        _tutorialModel.OnNextTutorialStep += AnimateTutorial;
        _tutorialModel.OnAllStepsCompleted += Disable;

        animator = GetComponent<Animator>();
        animator.SetFloat(_motionX, 0);
        animator.SetFloat(_motionY, 0);
        
        if (_tutorialModel.TutorialSeen)
        {
            gameObject.SetActive(false);
            return;
        }
        
        _originalPosition = transform.position;
        transform.position += Vector3.right * 3;
        transform.DOMove(_originalPosition, 1.5f).SetEase(Ease.OutBack);
    }

    private void OnDestroy()
    {
        _tutorialModel.OnTutorialStart -= AnimateTutorial;
        _tutorialModel.OnNextTutorialStep -= AnimateTutorial;
        _tutorialModel.OnAllStepsCompleted -= Disable;
    }

    void Update()
    {
        var realVelocity = transform.InverseTransformDirection(transform.position - objectPosition) / Time.deltaTime;
        velocity = Vector3.Lerp(velocity, realVelocity, Time.deltaTime * 5);
        if (velocity.magnitude < .01f)
            velocity = Vector3.zero;

        _lookAtTransform.position = transform.position;
        _lookAtTransform.LookAt(_tutorialCamera.transform);
        transform.rotation = Quaternion.Lerp(transform.rotation, _lookAtTransform.rotation, .5f);

        objectPosition = transform.position;
        animator.SetFloat(_motionX, velocity.x * .2f);
        animator.SetFloat(_motionY, velocity.z * .2f);
    }
    
    void Disable()
    {
        transform.DOMoveY(transform.position.y + 3, 1f).SetEase(Ease.OutBack);
        transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutSine).OnComplete( () =>
        {
            _tutorialController.EndTutorial();
            gameObject.SetActive(false);
        });
    }
    
    void AnimateTutorial()
    {
        var tutorial = _tutorialModel.CurrentTutorial;
        transform.DOMove(_originalPosition + tutorial.MoveLocation, .5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
        {
            animator.SetTrigger(tutorial.AnimationId());
        });
    }
    
    void OnMouseDown()
    {
        _tutorialController.NextTutorialStep();
    }
}
