using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GalaxyButtonView : MonoBehaviour
{
    public GalaxyButtonType ButtonType;

    private GalaxyController _galaxyController;
    private Camera _arCamera;
    
    private void Start()
    {
        _arCamera = Camera.main.GetComponent<Camera>();
        _galaxyController = FindObjectOfType<GalaxyController>();
        GetComponent<Button>().onClick.AddListener(ButtonAction);
    }

    void ButtonAction()
    {
        _galaxyController.ZoomInGalaxy(transform, ButtonType);
    }

    void Update()
    {
        transform.rotation = _arCamera.transform.rotation;
    }
}
