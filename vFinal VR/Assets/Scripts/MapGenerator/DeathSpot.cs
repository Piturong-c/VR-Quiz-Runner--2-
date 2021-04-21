using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSpot : MonoBehaviour
{
    public bool isHitBody = true;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isHitBody)
        {
            OnPlayerCollide();
        }

        if (other.CompareTag("MainCamera") && !isHitBody)
        {
            DeathSpot leftTreeDeathScript = transform.parent.parent.GetChild(1).GetChild(0).GetComponent<DeathSpot>();
            DeathSpot rightTreeDeathScript = transform.parent.parent.GetChild(0).GetChild(0).GetComponent<DeathSpot>();
            Destroy(leftTreeDeathScript.gameObject);
            Destroy(rightTreeDeathScript.gameObject);
            OnPlayerCollide();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player") && isHitBody)
        {
            OnPlayerCollide();
        }
        
        if (other.collider.CompareTag("MainCamera") && !isHitBody)
        {
            DeathSpot leftTreeDeathScript = transform.parent.parent.GetChild(1).GetComponent<DeathSpot>();
            DeathSpot rightTreeDeathScript = transform.parent.parent.GetChild(0).GetComponent<DeathSpot>();
            Destroy(leftTreeDeathScript.gameObject);
            Destroy(rightTreeDeathScript.gameObject);
            OnPlayerCollide();
        }
    }

    private void OnPlayerCollide()
    {
        Player.self.Die();
    }
}
