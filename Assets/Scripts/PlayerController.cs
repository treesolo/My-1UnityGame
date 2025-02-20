using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private float blastStrength = 15.0f;
    private float powerUpStrength = 15.0f;
    private float jumpStrength = 100.0f;
    private float speed = 5.0f;
    public bool hasPowerup = false;
    public bool hasAutoProjectiles = false;
    public bool rdyToJump = true;
    public bool chargeAfterShock = false;
    public GameObject autoProjectileIndicator;
    public GameObject powerupIndicator;
    public GameObject afterShockPrefab;
    private float deathPoint;
    public bool gameOver = false;
    public int midEnemyKC = 0;
    private int midEnemyKillPoints = 10;
    public int lowEnemyKC = 0;
    private int lowEnemyKillPoints = 5;
    public int hardEnemyKC = 0;
    private int hardEnemyKillPoints = 15;
    public int expertEnemyKC = 0;
    private int expertEnemyKillPoints = 20;
    public int enemyBossKC = 0;
    private int bossKillPoints = 50;
    public int totalScore;
    private static int nextID = 0;

    public static int GetNextID()
    {
        return nextID++;
    }
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && rdyToJump && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            rdyToJump = false;
            chargeAfterShock = true;
        }
        if (playerRb.transform.position.y > 8 && !gameOver)
        {
            playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, -50, playerRb.linearVelocity.z);
        }
        if (Input.GetKeyDown(KeyCode.V) && !gameOver)
            {
                playerRb.position = new Vector3(0, 2, 0);
            }
        totalScore = (lowEnemyKC * lowEnemyKillPoints) + (midEnemyKC * midEnemyKillPoints) + (hardEnemyKC * hardEnemyKillPoints) + (expertEnemyKC * expertEnemyKillPoints) + (enemyBossKC * bossKillPoints);
        if (playerRb.transform.position.y < deathPoint)
        {
            Destroy(gameObject);
            gameOver = true;
            Debug.Log("Game is over!!! \n You killed: " + lowEnemyKC + " minions, " + midEnemyKC + " default enemies, " + hardEnemyKC + " strong enemies, " + expertEnemyKC + " elite enemies, and " + enemyBossKC + "bosses. \n CONGRATULATIONS!!! \n Your total score is: " + totalScore);
            FindFirstObjectByType<TextLog>().ShowLogOnScreen();
        }

        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        autoProjectileIndicator.transform.position = transform.position + new Vector3(0, 1.0f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Powerup"))
        {
            Debug.Log("Power Up Collected! Good Job!");
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
        if (other.CompareTag("Autocanon"))
        {
            Debug.Log("Auto Canon Activated!");
            hasAutoProjectiles = true;
            autoProjectileIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(AutoCanonCountdown());
        }

        if (other.CompareTag("BossProjectile"))
        {
            Vector3 blast = transform.position - other.gameObject.transform.position;
            playerRb.AddForce(blast * blastStrength, ForceMode.Impulse);
            Destroy(other.gameObject);
        }
    }
    IEnumerator AutoCanonCountdown()
    {
        yield return new WaitForSeconds(5);
        hasAutoProjectiles = false;
        autoProjectileIndicator.gameObject.SetActive(false);
    }
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && chargeAfterShock)
        {
            rdyToJump = true;
            chargeAfterShock = false;
            Invoke("SpawnAfterShock",0f);
        }
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
            Debug.Log("Collided with: " + collision.gameObject.name + " with powerup set to " + hasPowerup);
        }
    }
    void SpawnAfterShock()
    {
        Instantiate(afterShockPrefab, transform.position, Quaternion.identity);
    }
}
