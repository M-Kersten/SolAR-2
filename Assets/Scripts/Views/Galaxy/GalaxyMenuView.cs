using System;
using Michsky.UI.ModernUIPack;

using UnityEngine;
using UnityEngine.UI;
public class GalaxyMenuView : MonoBehaviour
{
    public Button SettingsButton;
    public Button BuyButton;
    public Button[] BackButtons;

    public ModalWindowManager SettingsWindow;
    public ModalWindowManager BuyWindow;

    private GalaxyController _galaxyController;
    private GalaxyButtonType _buttonType;
    private GalaxyModel _galaxyModel;
    
    private void Start()
    {
        _galaxyModel = FindObjectOfType<GalaxyModel>();
        _galaxyController = FindObjectOfType<GalaxyController>();
        
        foreach (var backButton in BackButtons)
            backButton.onClick.AddListener(GoBack);
        _galaxyModel.OnZoomInComplete += SetActiveMenu;
    }

    private void OnDestroy()
    {
        _galaxyModel.OnZoomInComplete -= SetActiveMenu;
    }

    void GoBack()
    {
        Transform currentButton = _buttonType switch
        {
            GalaxyButtonType.Settings => SettingsButton.transform,
            GalaxyButtonType.Buy => BuyButton.transform,
            _ => SettingsButton.transform
        };
        if (SettingsWindow.isOn)
            SettingsWindow.CloseWindow();

        if (BuyWindow.isOn)
            BuyWindow.CloseWindow();
        
        _galaxyController.ZoomOutGalaxy(currentButton);   
    }

    public void SetActiveMenu()
    {
        _buttonType = _galaxyModel.SelectedGalaxyButtonType;
        switch (_buttonType)
        {
            case GalaxyButtonType.Settings:
                SettingsWindow.OpenWindow();
                break;
            case GalaxyButtonType.Buy:
                BuyWindow.OpenWindow();
                break;
        }
    }
}