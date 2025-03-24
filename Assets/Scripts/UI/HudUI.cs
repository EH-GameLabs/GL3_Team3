using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudUI : BaseUI
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.instance.ShowUI(UIManager.GameUI.Pause);
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            CameraManager.instance.SwitchMinimapCam();
            UIManager.instance.ShowUI(UIManager.GameUI.MiniMap);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
