using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehavior : MonoBehaviour
{
    public GameBehavior gameManager;
    public bool isTracking = true;

    public Transform patrolRoute;
    public List<Transform> locations;

    private int locationIndex = 0;
    private UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    private int _lives = 3;
    public int EnemyLives
    {
        get { return _lives;}
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Tango Down.");
            }
        }
    }


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }
    void InitializePatrolRoute()
    {  
        foreach(Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }
    void Update()
    {
        if(agent.remainingDistance < 0.2f && !agent.pathPending)
        { 
            MoveToNextPatrolLocation();
        }
    }
    void MoveToNextPatrolLocation()
    {
        if (locations.Count == 0)
            return;
        agent.destination = locations[locationIndex].position;

        locationIndex = (locationIndex + 1) % locations.Count;
    }
    
    void resetisTracking()
    {
        isTracking = true;
    }
        
    
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player" && isTracking)
        {   Debug.Log("Player detected --- ATTACK!");
            agent.destination = player.position;
            if(gameManager.isTickingDamage)
            {   
                gameManager.damage = true;
                Debug.Log("Player taking damage");
                gameManager.InvokeRepeating("damageT", 0.1f, 0.5f);
            }

            else
            {   Debug.Log("Player is Immune to my attack!");}
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.name == "Player" && isTracking)
        {Debug.Log("Player out of range, resume patrol");

        }
        if(other.name == "Player")
        {gameManager.damage = false;}
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Hit!");
        }
        if(collision.gameObject.name == "Player")
        {
            gameManager.Invoke("instaDeath", 0.1f);
        }
    }
}
