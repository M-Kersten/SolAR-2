using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RetrobotView : MonoBehaviour
{
    public Camera PlayerCamera;
    public Transform PlayerPosition;
    public GameObject Thruster;
    public SkinnedMeshRenderer BotRenderer;
    public Renderer BellyIconRenderer;
    public Transform LookAtTransform;
    
    private Animator animator;
    private Vector3 velocity;
    private Vector3 objectPosition;
    private Vector3 Destination;
    private bool stickToPlayer;
    private bool stickToPlanet;
    private Vector3 _originalScale;
    private Vector3 _planetPosition;

    private DialogController _dialogController;
    private PlanetController _planetController;
    private BotController _botController;
    private DialogModel _dialogModel;
    private BotModel _botModel;
    private GalaxyModel _galaxyModel;
    private PlayerStateModel _playerStateModel;

    private static readonly int _motionX = Animator.StringToHash("Motion_X");
    private static readonly int _motionY = Animator.StringToHash("Motion_Y");
    private static readonly int _idlePattern = Animator.StringToHash("Idle_Pattern");
    private static readonly int _isFighting = Animator.StringToHash("Is_Fighting");
    private static readonly int _flip = Animator.StringToHash("Do_Flip_Happy");
    private static readonly int _giggle = Animator.StringToHash("Do_Giggle");
    
    void Start()
    {
        _planetController = FindObjectOfType<PlanetController>();
        _dialogController = FindObjectOfType<DialogController>();
        _botController = FindObjectOfType<BotController>();
        _dialogModel = FindObjectOfType<DialogModel>();
        _botModel = FindObjectOfType<BotModel>();
        _galaxyModel = FindObjectOfType<GalaxyModel>();
        _playerStateModel = FindObjectOfType<PlayerStateModel>();
        
        _dialogModel.OnTalk += AnimateTalk;
        
        _botModel.OnGoToPlanet += FlyToPlanet;
        _botModel.OnBackToPlayer += BackToPlayer;
        _botModel.OnBotStateChanged += StateChange;
        _botModel.OnGoToPosition += GoToPosition;
        
        _galaxyModel.OnStartGame += Spawn;
        
        _playerStateModel.OnStateChanged += CheckState;
        
        stickToPlayer = true;
        Thruster.SetActive(false);
        animator = GetComponent<Animator>();
        animator.SetFloat(_motionX, 0);
        animator.SetFloat(_motionY, 0);
        _originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void OnDestroy()
    {
        _dialogModel.OnTalk -= AnimateTalk;
        _botModel.OnGoToPlanet -= FlyToPlanet;
        _botModel.OnBackToPlayer -= BackToPlayer;
        _botModel.OnBotStateChanged -= StateChange;
        _botModel.OnGoToPosition -= GoToPosition;
        _galaxyModel.OnStartGame -= Spawn;
        _playerStateModel.OnStateChanged -= CheckState;
    }

    void CheckState(PlayerState state)
    {
        if (state == PlayerState.Surface)
            _botController.BackToPlayer();
    }

    void AnimateTalk(DialogItem dialog)
    {
        animator.SetTrigger(dialog.AnimationId());
    }

    void StateChange()
    {
        var stateInfo = _botModel.CurrentStateInfo();
        BotRenderer.material.SetColor("_EmissionColor" ,stateInfo.Color);
        BellyIconRenderer.material.mainTexture = stateInfo.Icon;
        BellyIconRenderer.gameObject.SetActive(stateInfo.Icon != null);
    }

    void Spawn()
    {
        transform.position = PlayerPosition.position;
        animator.SetTrigger(_flip);
        transform.DOScale(_originalScale, 2f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                Thruster.SetActive(true);
                _dialogController.Greet();
            });
    }

    void GoToPosition(Vector3 location, bool immediate)
    {
        stickToPlanet = false;
        stickToPlayer = false;
        Destination = location;
        
        if (immediate)
            transform.position = Destination;
    }
    
    public void BackToPlayer()
    {
        stickToPlanet = false;
        stickToPlayer = true;
    }
    
    void FlyToPlanet(Vector3 location, bool immediate)
    {
        _planetPosition = location;
        stickToPlanet = true;
        stickToPlayer = false;
    }
    
    void OnMouseDown()
    {
        if (_botModel.CurrentBotState == BotState.Visit)
        {
            _planetController.LandOnPlanet();
            return;
        }
        if (_botModel.CurrentBotState == BotState.PlanetIdle)
        {
            _planetController.BackToSpace();
            return;
        }
        animator.SetTrigger(_giggle);
    }

    private void Update()
    {
        var newVelocity = transform.InverseTransformDirection(transform.position - objectPosition) / Time.deltaTime;
        velocity = Vector3.Lerp(velocity, newVelocity, Time.deltaTime * 5);
        if (velocity.magnitude < .01f)
            velocity = Vector3.zero;
        
        objectPosition = transform.position;
        SetMotionXState(Mathf.Lerp(animator.GetFloat(_motionX), velocity.x * .75f, .5f));
        SetMotionYState(Mathf.Lerp(animator.GetFloat(_motionY), velocity.z * .75f, .5f));

        LookAtTransform.position = transform.position;
        LookAtTransform.LookAt(PlayerCamera.transform);
        transform.rotation = Quaternion.Lerp(transform.rotation, LookAtTransform.rotation, .5f);
        
        if (stickToPlayer)
        {
            var distance = transform.position - PlayerPosition.position;
            if (distance.magnitude > .1f)
                transform.position = Vector3.Lerp(transform.position, PlayerPosition.position, Time.deltaTime * 2);
        }
        else if (stickToPlanet)
        {
            Destination = _planetPosition + PlayerCamera.transform.right * .2f;
            var distance = transform.position - Destination;
            if (distance.magnitude > .01f)
                transform.position = Vector3.Lerp(transform.position, Destination, Time.deltaTime * 2);
        }
        else
        {
            var distance = transform.position - Destination;
            if (distance.magnitude > .01f)
                transform.position = Vector3.Lerp(transform.position, Destination, Time.deltaTime * 7);
        }
    }

    public void SetMotionXState(float value) => animator.SetFloat(_motionX, value);
    public void SetMotionYState(float value) => animator.SetFloat(_motionY, value);
    public void SetIdleState(float value) => animator.SetFloat(_idlePattern, value);
    public void SetFightIdle(bool isFighting) => animator.SetBool(_isFighting, isFighting);
}
