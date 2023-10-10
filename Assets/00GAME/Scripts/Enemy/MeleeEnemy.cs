using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;  
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D _boxCollider;

    [Header("Player Layers")]
    [SerializeField] private LayerMask _playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;
    


    private Animator _anim;
    private HealthController playerHealth;

    private EnemyPatrol enemyPatrol;
    private void Awake()
    {
        _anim = GetComponent<Animator>(); 
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight?
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
            {
                cooldownTimer = 0;
                _anim.SetTrigger("attack");
                SoundManager.instance.PlaySound(attackSound);
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
         
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(_boxCollider.bounds.size.x * range, _boxCollider.bounds.size.y, _boxCollider.bounds.size.z),
            0, Vector2.left, 0, _playerLayer);

        if(hit.collider != null)
            playerHealth = hit.transform.GetComponent<HealthController>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
             new Vector3(_boxCollider.bounds.size.x * range, _boxCollider.bounds.size.y, _boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        //if player still in range damage him
        if(PlayerInSight())
        {
            //Damage player health
            playerHealth.TakeDamage(damage);

        }    
    }
}
