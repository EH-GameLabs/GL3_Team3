using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : BaseUI
{
    [Header("Player Health")]
    [SerializeField] private GameObject healthSlot;
    [SerializeField] private List<Sprite> healthPoints;

    [Header("Points")]
    [SerializeField] private TextMeshProUGUI points;

    [Header("Player Shield")]
    [SerializeField] private Image shieldImage;

    [Header("Player Energy")]
    [SerializeField] private Image energyImage;

    [Header("Player Key")]
    [SerializeField] private GameObject playerKey;
    [SerializeField] private List<Sprite> keys;

    [Header("Player weapons")]
    [SerializeField] private Image primaryWeapon;
    [SerializeField] private Image secondaryWeapon;

    private void Start()
    {
        healthSlot.GetComponent<Image>().sprite = healthPoints[^1];
    }

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

    public void SetHealth(int health)
    {
        healthSlot.GetComponent<Image>().sprite = healthPoints[health];
    }

    public void SetEnergy(float value)
    {
        energyImage.fillAmount = value / 100f;
    }

    public void SetShield(float value)
    {
        shieldImage.fillAmount = value / 100;
    }

    public void SetKey(int num)
    {
        playerKey.GetComponent<Image>().sprite = keys[num];
    }

    public void SetWeapon(Sprite weapon, GunType type) 
    {
        if (type == GunType.Primary)
        {
            primaryWeapon.sprite = weapon;
            primaryWeapon.gameObject.SetActive(true);
        }
        else 
        {
            secondaryWeapon.sprite = weapon;
            secondaryWeapon.gameObject.SetActive(true);
        }
    }

    public void SetPoints(int value) 
    {
        points.text = value.ToString();
    }
}
