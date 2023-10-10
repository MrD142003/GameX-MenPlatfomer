using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip CollectCoins;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerMovement.numberOfCoins++;
            SoundManager.instance.PlaySound(CollectCoins);
            //PlayerPrefs.SetInt("NumberOfCoins", PlayerMovement.numberOfCoins);
            Destroy(gameObject);
        }
    }

    

    
}
