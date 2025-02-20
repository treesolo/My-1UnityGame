using UnityEngine;

public class ProjectileReciever : MonoBehaviour
{
    public GameObject projectilePrefab;
    private GameObject player;
    private PlayerController playerCont;
    private float strDel = 0f;
    private float repDel = 1.0f;
    private bool spawning = false;
    private float projectileSpeed = 5.0f;
    public int id { get; private set; }
    void Awake()
    {
        id = PlayerController.GetNextID();
    }
    void Start()
    {
        player = GameObject.Find("Player");
        playerCont = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCont.hasAutoProjectiles && !spawning)
        {
            InvokeRepeating("SpawnProjectiles", strDel, repDel);
            spawning = true;    
        } 
        else if (!playerCont.hasAutoProjectiles && spawning)
        {
            CancelInvoke("SpawnProjectiles");
            spawning = false;
        }
    }
    void SpawnProjectiles()
    {
        GameObject projectile = Instantiate(projectilePrefab, player.transform.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        AutoProjectile ownProjectile = projectile.GetComponent<AutoProjectile>();
        Vector3 direction = (transform.position - player.transform.position).normalized;
        projectileRb.AddForce(direction * projectileSpeed, ForceMode.Impulse);
        if (ownProjectile != null)
        {
            ownProjectile.SetID(id);
        }
    }
}
