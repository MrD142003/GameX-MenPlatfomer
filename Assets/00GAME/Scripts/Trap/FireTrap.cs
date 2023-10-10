using UnityEngine;
using System.Collections;
public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator _anim;
    private SpriteRenderer _spriteRend;

    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private bool triggered; //when the trap gets triggered
    private bool active; //when the trap is active and can hurt the player

    private HealthController playerHealth;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerHealth != null && active)      
            playerHealth.TakeDamage(damage);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<HealthController>();

            if (!triggered)           
                //trigger the firetrap
                StartCoroutine(ActivateFiretrap());   
            
            if (active)           
                collision.GetComponent<HealthController>().TakeDamage(damage);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")       
            playerHealth = null;
        
    }

    private IEnumerator ActivateFiretrap()
    {
        //turn the sprite red to notify the player and trigger the trap
        triggered = true;
        _spriteRend.color = Color.red; 

        //wait for delay, activate trap, turn on animation, return color back to normal.
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        _spriteRend.color = Color.white; //turn the sprite back to its initial color
        active = true;
        _anim.SetBool("activated", true);

        //wait until X seconds, deactive trap and reset all variables and animator

        yield return new WaitForSeconds(activeTime); 
        active = false;
        triggered = false;
        _anim.SetBool("activated", false);
        

    }

}
