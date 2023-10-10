using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private HealthController playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<HealthController>();       
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        //check if checkpoint available
        if (currentCheckpoint == null)
        {
            //Show game over screen
            uiManager.GameOver();
            return;
        }
        transform.position = currentCheckpoint.position; //move player to checkpoint position
        playerHealth.Respawn(); //Restore player health and reset animation

        //Move camera to checkpoint
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //Activate checkpoints
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
            
        }
    }
}
