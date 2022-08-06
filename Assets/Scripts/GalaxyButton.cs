using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GalaxyButton : MonoBehaviour
{
    [System.Serializable]
    public enum GalaxyButtonType
    {
        Play,
        Settings,
        Buy,
    }
    public GalaxyButtonType ButtonType;
    public GalaxyController GalaxyController;
    public GalaxyMenu GalaxyMenu;
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonAction);
    }

    void ButtonAction()
    {
        if (ButtonType == GalaxyButtonType.Play)
        {
            GalaxyController.StartGame();
        }
        else
        {
            GalaxyController.ZoomInGalaxy(transform, () =>
            {
                Debug.Log("enabling menu");
                GalaxyMenu.SetActiveMenu(ButtonType);
            });
        }
    }

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
