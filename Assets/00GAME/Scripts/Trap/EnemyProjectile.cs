using UnityEngine;

public class EnemyProjectile : EnemyDamage //will damage the player every time they touch
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifeTime;
    private Animator anim;
    private BoxCollider2D _coll;
    private bool hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        _coll = GetComponent<BoxCollider2D>();
    }
    public void ActivateProjectile()
    {
        hit = false;
        lifeTime = 0;
        gameObject.SetActive(true);
        _coll.enabled = true;
    }

    private void Update()
    {
        float momentSpeed = speed * Time.deltaTime;
        transform.Translate(momentSpeed, 0, 0);

        lifeTime += Time.deltaTime;
        if(lifeTime > resetTime) 
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision); //excute logic from parent scripts first
        _coll.enabled = false; 

        if (anim != null)
            anim.SetTrigger("explode");
        else
            gameObject.SetActive(false); //when this hits any object deactivate arrow
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
