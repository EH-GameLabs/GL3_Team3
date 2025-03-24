using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapUI : BaseUI
{
    public void ExitMinimap()
    {
        CameraManager.instance.SwitchMinimapCam();
        UIManager.instance.ShowUI(UIManager.GameUI.HUD);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
