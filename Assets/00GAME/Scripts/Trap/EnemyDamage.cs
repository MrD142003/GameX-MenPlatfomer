using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float _damage;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.GetComponent<HealthController>().TakeDamage(_damage);
    }
}
