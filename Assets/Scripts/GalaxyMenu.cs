using Michsky.UI.ModernUIPack;

using UnityEngine;
using UnityEngine.UI;
public class GalaxyMenu : MonoBehaviour
{
    public Button SettingsButton;
    public Button BuyButton;
    public Button[] BackButtons;

    public GalaxyController GalaxyController;
    public ModalWindowManager SettingsWindow;
    public ModalWindowManager BuyWindow;

    private GalaxyButton.GalaxyButtonType _buttonType;
    
    private void Start()
    {
        foreach (var backButton in BackButtons)
            backButton.onClick.AddListener(GoBack);
    }

    void GoBack()
    {
        Debug.Log("go back");
        Transform currentButton = _buttonType switch
        {
            GalaxyButton.GalaxyButtonType.Settings => SettingsButton.transform,
            GalaxyButton.GalaxyButtonType.Buy => BuyButton.transform,
            _ => SettingsButton.transform
        };
        if (SettingsWindow.isOn)
            SettingsWindow.CloseWindow();

        if (BuyWindow.isOn)
            BuyWindow.CloseWindow();
        
        GalaxyController.ZoomOutGalaxy(currentButton);   
    }

    public void SetActiveMenu(GalaxyButton.GalaxyButtonType buttonType)
    {
        _buttonType = buttonType;
        switch (_buttonType)
        {
            case GalaxyButton.GalaxyButtonType.Settings:
                SettingsWindow.OpenWindow();
                break;
            case GalaxyButton.GalaxyButtonType.Buy:
                BuyWindow.OpenWindow();
                break;
        }
    }
}