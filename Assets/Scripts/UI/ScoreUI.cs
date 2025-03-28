using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI puntiNemici;
    [SerializeField] private TextMeshProUGUI puntiScudo;
    [SerializeField] private TextMeshProUGUI puntiEnergia;
    [SerializeField] private TextMeshProUGUI puntiOstaggi;
    [SerializeField] private TextMeshProUGUI puntiTotali;
    [SerializeField] private TextMeshProUGUI stanzeSegrete;


    private string TextFormat(int value)
    {
        return value.ToString("D6"); // "D6" forza il numero a 6 cifre, riempiendo con zeri a sinistra
    }

    public void SetPoints(ScoreType scoreType, int value)
    {
        switch (scoreType)
        {
            case ScoreType.Nemici:
                puntiNemici.text = TextFormat(value);
                break;
            case ScoreType.Scudo:
                puntiScudo.text = TextFormat(value);
                break;
            case ScoreType.Energia:
                puntiEnergia.text = TextFormat(value);
                break;
            case ScoreType.Ostaggi:
                puntiOstaggi.text = TextFormat(value);
                break;
            case ScoreType.Totali:
                puntiTotali.text = TextFormat(value);
                break;
            case ScoreType.StanzeSegrete:
                stanzeSegrete.text = value + "/2";
                break;
        }
    }

}

public enum ScoreType
{
    Nemici, Scudo, Energia, Ostaggi, Totali, StanzeSegrete,
}