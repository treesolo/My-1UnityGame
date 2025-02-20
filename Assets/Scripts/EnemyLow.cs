using UnityEngine;

public class EnemyLow : MonoBehaviour
{
    private float speed = 2.0f;
    public float blastStrength = 30.0f;
    private Rigidbody enemyRb;
    private GameObject player;
    private float afterShockStrength = 30.0f;
    private PlayerController playerCont;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        Renderer rendHard = GetComponent<Renderer>();
        player = GameObject.Find("Player");
        rendHard.material.color = Color.white;
        playerCont = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
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
            playerCont.lowEnemyKC++;
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
