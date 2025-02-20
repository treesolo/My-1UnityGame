using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 3.0f;
    public float blastStrength = 20.0f;
    private Rigidbody enemyRb;
    private GameObject player;
    private float afterShockStrength = 20.0f;
    private PlayerController playerCont;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerCont = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        if (!playerCont.gameOver)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed);
        } else { Destroy(gameObject); }
        if (enemyRb.linearVelocity.y > 0) 
        {
            enemyRb.linearVelocity = new Vector3(enemyRb.linearVelocity.x, 0, enemyRb.linearVelocity.z);
        }    

        if (gameObject.transform.position.y < -1)
        {
            playerCont.midEnemyKC++;
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AfterShock"))
        {
            Vector3 afterShockLook = transform.position - other.gameObject.transform.position;
            float afterShockEffect = (transform.position - other.gameObject.transform.position).magnitude;
            enemyRb.AddForce(afterShockLook * afterShockStrength / afterShockEffect, ForceMode.Impulse);
            Debug.Log("Distance was " + afterShockEffect + ", strength of aftershock: " + afterShockStrength / afterShockEffect);
        } 
        if (other.CompareTag("Projectile"))
        {
            Vector3 blast = transform.position - other.gameObject.transform.position;
            enemyRb.AddForce(blast * blastStrength, ForceMode.Impulse);
            Destroy(other.gameObject);
        }
    }
}
