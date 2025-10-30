using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public Transform player;
    public float backstabAngleThreshold = -0.5f; 
    public float backstabRange = 2f;

    private bool canBackstab = false;
    private float backstabTimer = 0f;
    private bool isDead = false;

    void Update()
    {
        if (isDead) return;

        Vector3 enemyForward = transform.forward;
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float dot = Vector3.Dot(enemyForward, toPlayer);
        float distance = Vector3.Distance(player.position, transform.position);

        bool inBackstabZone = (dot < backstabAngleThreshold && distance < backstabRange);

        // Smooth small jitters
        if (inBackstabZone && !canBackstab)
        {
            backstabTimer += Time.deltaTime;
            if (backstabTimer >= 0.1f) canBackstab = true;
        }
        else if (!inBackstabZone && canBackstab)
        {
            backstabTimer -= Time.deltaTime;
            if (backstabTimer <= -0.1f) canBackstab = false;
        }
        else
        {
            backstabTimer = Mathf.Clamp(backstabTimer, -0.1f, 0.1f);
        }
    }

    public bool CanBeBackstabbed()
    {
        return canBackstab && !isDead;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.back * 3f + Vector3.up * 2f, ForceMode.Impulse);
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Destroy(gameObject);
    }
}
