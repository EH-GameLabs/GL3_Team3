using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        instance = this;
    }
    #endregion

    public int keysCollected;
    [Header("Score")]
    public int gameScore;
    public int hostageCollected;
    public int pointsPerHostage;
    public int playerLife;
    public int pointsPerLife;
    public int secretDoors;

    [Header("UI Score")]
    private GameObject scorePanel;

    [Header("SecretRooms")]
    [SerializeField] private List<GameObject> room1;
    [SerializeField] private List<GameObject> room2;

    private void Start()
    {
        scorePanel = FindAnyObjectByType<ScoreUI>(FindObjectsInactive.Include).gameObject;
    }

    public void AddScore(int score)
    {
        gameScore += score;
        FindAnyObjectByType<HudUI>().SetPoints(gameScore);
    }
    public void AddKeys() { keysCollected++; FindAnyObjectByType<HudUI>().SetKey(keysCollected); }

    public void AddHostage() { hostageCollected++; }
    public void RemoveHostage()
    {
        hostageCollected = 0;
    }

    public void ShowWin()
    {
        Cursor.lockState = CursorLockMode.None;
        UIManager.instance.ShowUI(UIManager.GameUI.Win);
        ShowScore();
    }

    public void ShowLose()
    {
        Cursor.lockState = CursorLockMode.None;
        UIManager.instance.ShowUI(UIManager.GameUI.Lose);
        ShowScore();
    }

    private void ShowScore()
    {
        Player player = FindAnyObjectByType<Player>();
        scorePanel.SetActive(true);
        ScoreUI scoreUI = scorePanel.GetComponent<ScoreUI>();
        scoreUI.SetPoints(ScoreType.Nemici, gameScore);
        scoreUI.SetPoints(ScoreType.Scudo, player.shield);
        scoreUI.SetPoints(ScoreType.Energia, player.energy);
        scoreUI.SetPoints(ScoreType.Ostaggi, hostageCollected * pointsPerHostage);
        scoreUI.SetPoints(ScoreType.Totali, GetFinalScore());

        CalculateSecretDoors();
        scoreUI.SetPoints(ScoreType.StanzeSegrete, secretDoors);

    }

    private void CalculateSecretDoors()
    {
        foreach (var door in room1)
        {
            if (!door.activeInHierarchy)
            {
                secretDoors++;
                break;
            }
        }
        foreach (var door in room2)
        {
            if (!door.activeInHierarchy)
            {
                secretDoors++;
                break;
            }
        }
    }

    public int GetFinalScore()
    {
        Player player = FindAnyObjectByType<Player>();
        return
            hostageCollected * pointsPerHostage +
            playerLife * pointsPerLife +
            player.energy +
            player.shield +
            gameScore; // enemies
    }
}
