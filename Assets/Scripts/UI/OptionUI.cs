using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : BaseUI
{


    [Header("Sensitivity")]
    [SerializeField] private Slider sensSlider;

    public void GoToMainMenu()
    {
        UIManager.instance.ShowUI(UIManager.GameUI.MainMenu);
    }

    public void SetPlayerSens(Slider slider)
    {
        Player.Instance.SetSensMultiplier(slider.value + 0.5f);
    }
}
