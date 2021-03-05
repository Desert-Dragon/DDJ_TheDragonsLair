using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public GameBehavior gameManager;
    public EnemyBehavior enemy;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.name == "Player")
        {
            Destroy(this.transform.parent.gameObject);
            Debug.Log("Item Collected");
            gameManager.Items += 1;
            if(this.gameObject.name == "Invincibility")
            {   
                gameManager.isTickingDamage = false;
                gameManager.Invoke("resetTickingDamage", 10f);

            }
            if(this.gameObject.name == "Invisibility")
            {
                enemy.isTracking = false;
                enemy.Invoke("resetisTracking", 5f);
            }
        
        
        }
    }
}