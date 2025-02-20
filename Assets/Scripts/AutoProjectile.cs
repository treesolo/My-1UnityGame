using UnityEngine;
using UnityEngine.EventSystems;

public class AutoProjectile : MonoBehaviour
{
    public float speed = 7.0f;
    private Rigidbody projectileRb;
    private GameObject targetEnemy;
    private int parentEnemyID;

    public void SetID(int id)
    {
        parentEnemyID = id;
        FindTarget();
    }
    void Start()
    {
        projectileRb = GetComponent<Rigidbody>();
    }
    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            ProjectileReciever enemyScript = enemy.GetComponent<ProjectileReciever>();
            if (enemyScript != null && enemyScript.id == parentEnemyID)
            {
                targetEnemy = enemy; 
                break;
            }
        }
    }
    void FixedUpdate()
    {
        if (targetEnemy != null)
        {
            Vector3 projectileLookDirection = (targetEnemy.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(projectileLookDirection);
            projectileRb.MovePosition(transform.position + projectileLookDirection * speed * Time.fixedDeltaTime);
        } else { Destroy(gameObject);}
    }
}

