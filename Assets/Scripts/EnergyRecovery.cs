using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRecovery : MonoBehaviour
{
    // Valore intero da incrementare
    public int currentValue = 0;
    // Incremento desiderato per secondo (modificabile dall'Inspector)
    public float incrementPerSecond = 1f;

    // Variabile interna per accumulare incrementi parziali
    private float accumulator = 0f;
    // Flag per verificare se il player è all'interno della zona
    private bool playerInside = false;

    void Update()
    {
        if (playerInside)
        {
            // Accumula l'incremento parziale in base al tempo trascorso
            accumulator += incrementPerSecond * Time.deltaTime;
            // Quando l'accumulatore supera 1, si incrementa il valore intero
            if (accumulator >= 1f)
            {
                int inc = Mathf.FloorToInt(accumulator);
                currentValue += inc;
                accumulator -= inc;
            }
        }
    }

    // Quando il player entra nella zona trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            playerInside = true;
        }
    }

    // Quando il player esce dalla zona trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            playerInside = false;
        }
    }
}
