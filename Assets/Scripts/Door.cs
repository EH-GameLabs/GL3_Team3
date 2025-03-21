using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //if (type == DoorsType.Key) //if (player.keys < 1) return; else keys -= 1
        gameObject.SetActive(false);
    }
}
