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

    public void AddScore(int score)
    { 
        gameScore += score;
        FindAnyObjectByType<HudUI>().SetPoints(score);
    }
    public void AddKeys() { keysCollected++; FindAnyObjectByType<HudUI>().SetKey(keysCollected); }

    public void AddHostage() { hostageCollected++; }

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
