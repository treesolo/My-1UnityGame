using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public EnemySkill enemyskill; 
    public float speed = 1.0f;
    public float boostSpeed;
    public float skillSpeedMP = 3.0f;
    public float projectileSpeed = 10.0f;
    private float skillCD = 1.0f;
    //private float rotationSpeed = 50.0f;
    public GameObject minionPrefab;
    public GameObject projectilePrefab;
    private Rigidbody enemyRb;
    private GameObject player;
    public float blastStrength = 10.0f;
    private float afterShockStrength = 10.0f;
    private float prMaxFly = 30.0f;
    private PlayerController playerCont;

    public enum EnemySkill
    {
        SpeedBoost,
        SummonMinoins,
        ShootProjectiles
    }
    private void Awake()
    {
        AssignRandomSkill();
    }
    void AssignRandomSkill()
    {
        enemyskill = (EnemySkill)Random.Range(0, System.Enum.GetValues(typeof(EnemySkill)).Length);
        ApplySkill();
    }

    void ApplySkill()
    {
        switch(enemyskill)
        {
            case EnemySkill.SpeedBoost:
                boostSpeed = speed * skillSpeedMP;
                break;
            case EnemySkill.SummonMinoins:
                InvokeRepeating(nameof(SummonMinion), skillCD, skillCD);
                break;
            case EnemySkill.ShootProjectiles:
                InvokeRepeating(nameof(ShootProjectile), skillCD, skillCD);
                break;
        }
    }

    void SummonMinion()
    {
        Instantiate(minionPrefab, transform.position + Vector3.forward, Quaternion.identity);
    }

    void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        if (player != null)
        {
            Vector3 projectileLookDirection = (player.transform.position - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(projectileLookDirection);
            projectile.transform.rotation = targetRotation;

            projectileRb.AddForce(projectileLookDirection * projectileSpeed, ForceMode.Impulse);
        }
        else 
        {
            Destroy(gameObject);
        }
        if (projectile.transform.position.x > prMaxFly || projectile.transform.position.z > prMaxFly)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
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
        if (enemyRb.linearVelocity.y > 2)
        {
            enemyRb.linearVelocity = new Vector3(enemyRb.linearVelocity.x, 0, enemyRb.linearVelocity.z);
        }

        if (gameObject.transform.position.y < -1)
        {
            playerCont.enemyBossKC++;
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