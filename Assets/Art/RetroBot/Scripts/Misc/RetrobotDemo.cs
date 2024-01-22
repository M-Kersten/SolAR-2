using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RetrobotDemo : MonoBehaviour
{

    public GameObject[] retrobots;
    public Transform retrobotAnchor;

    private RetrobotView _retrobotView;
    public GameObject retroBot;

    public Dropdown actions;

    public Transform cameraRig;

    public Slider backwardForwardSlider;
    public Slider leftRightSlider;
    public Slider idlePatternSlider;
    public Slider rotationSlider;
    public Dropdown iconsDropdown;
    public Dropdown iconColorDropdown;
    public Toggle iconToggle;
    public Toggle isFighting; 

    private int lastIconTexture = 26;
    private int lastIconColor = 4;
    private bool lastIconToggle = false;

    public Text robotName;

    public float turnTableSpeed = 5;
    private Quaternion startingRotation;
    private bool doTurnTable;
    
    // Start is called before the first frame update
    void Start()
    {
        _retrobotView = retroBot.GetComponent<RetrobotView>();
        SpawnRobot(0);
        robotName.text = "RetroBot";
         
    }

    void Update()
    {
        if (doTurnTable) {
            retroBot.transform.Rotate(Vector3.up * Time.deltaTime*turnTableSpeed);
        }
    }

    public void SetRobotName(string name) {
        robotName.text = name;
    }

    public void SpawnRobot(int index) {
        GameObject robot = Instantiate(retrobots[index]);
        retroBot = robot;
        retroBot.transform.position = retrobotAnchor.transform.position;
        retroBot.transform.rotation = retrobotAnchor.transform.rotation;
        retroBot.transform.parent = retrobotAnchor.transform;
        _retrobotView = retroBot.GetComponent<RetrobotView>();

        RetrobotIconManager icons = retroBot.GetComponent<RetrobotIconManager>();

        iconsDropdown.ClearOptions();

        foreach ( Texture icon in icons.iconTextures)
        {
            iconsDropdown.options.Add(new Dropdown.OptionData(icon.name));
        }

        ChangeIcon(lastIconTexture);
        iconsDropdown.value = lastIconTexture;
        ChangeIconColor(lastIconColor);
        iconColorDropdown.value = lastIconColor;
        ToggleIcon(lastIconToggle);
        iconToggle.isOn = lastIconToggle;

        isFighting.isOn = false;

        startingRotation = retroBot.transform.rotation;
    }

    public void ChangeIcon(int index) {
        retroBot.GetComponent<RetrobotIconManager>().SetIconTexture(index);
        lastIconTexture = index;
    }

    public void ChangeIconColor(int index) {
        retroBot.GetComponent<RetrobotIconManager>().SetIconColor(index);
        lastIconColor = index;
    }

    public void ToggleIcon(bool on) {
        retroBot.GetComponent<RetrobotIconManager>().ActivateFrontIcon(on);
        retroBot.GetComponent<RetrobotIconManager>().ActivateHeadIcons(on);
        lastIconToggle = on;
    }

    public void SwitchRetrobot(int index) {
        Destroy(retroBot);
        SpawnRobot(index);
    }
    public void DoAction() {        
        //_retrobotView.DoAction(actions.options[actions.value].text);
    }

    public void UpdateMotionX(float value)
    {
        _retrobotView.SetMotionXState(value);
    }
    public void UpdateMotionY(float value) {
        _retrobotView.SetMotionYState(value);
    }

    public void UpdateIdlePattern(float value) {
        _retrobotView.SetIdleState(value);
    }

    public void UpdateRotation(float value)
    {
        cameraRig.transform.rotation = Quaternion.Euler(0, value, 0);       
    }

    public void ToggleFightingPose(bool on) {
        _retrobotView.SetFightIdle(on);
    }

    public void ActivateTurnTable(bool on) {
        doTurnTable = on;
    }

    public void Reset()
    {
        backwardForwardSlider.value = 0;
        leftRightSlider.value = 0;
        rotationSlider.value = 0;
        idlePatternSlider.value = 0;
        retroBot.transform.rotation = startingRotation;
    }
}
