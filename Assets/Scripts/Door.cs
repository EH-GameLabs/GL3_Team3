using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIManager;

public class Door : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] DoorsType type;

    private void OnCollisionEnter(Collision collision)
    {
        if (type == DoorsType.Closed) return;
        if (collision.gameObject.CompareTag(Tags.Player))
        {
            OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if projectile interact applydamage()
    }

    public void ApplyDamage(float dmg) 
    {
        hp -= dmg;
        if (hp < 1) { OpenDoor(); }
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
}
