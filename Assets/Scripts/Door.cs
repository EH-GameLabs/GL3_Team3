using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIManager;

public class Door : MonoBehaviour, IDamageable
{
    [SerializeField] int hp;
    [SerializeField] DoorsType type;

    public int currentHp { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (type == DoorsType.Closed) return;
        if (collision.gameObject.CompareTag(Tags.Player))
        {
            OpenDoor();
        }
    }

    private void OpenDoor() 
    {
        if (type == DoorsType.Key)
        {
            if (GameManager.instance.keysCollected < 1)
            {
                return;
            }
            else
            {
                GameManager.instance.keysCollected -= 1;
            }
        }
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 1) { OpenDoor(); }
    }
}
