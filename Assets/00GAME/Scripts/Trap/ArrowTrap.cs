using UnityEngine;
public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float _attackCooldown;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject[] arrows;
    private float _cooldownTimer;

    [Header("SFX")]
    [SerializeField] private AudioClip arrowSound;
    private void Attack()
    {
        _cooldownTimer = 0;

        SoundManager.instance.PlaySound(arrowSound);
        arrows[FindArrow()].transform.position = _firePoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }
    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
           if (!arrows[i].activeInHierarchy)
             return i;
        }
        return 0;
    }

    private void Update()
    {
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer >= _attackCooldown)
            Attack();
    }
    
}
