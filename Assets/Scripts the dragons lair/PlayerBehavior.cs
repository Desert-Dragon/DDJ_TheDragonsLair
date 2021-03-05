using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBehavior : MonoBehaviour
{  
    public GameBehavior gameManager;

    //1
    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;
    public float jumpVelocity = 5f;
    public float distanceToGround = .1f;
    public LayerMask groundLayer;
    public int playerHealth = 100;
    //1.01
    public GameObject bullet;
    public float bulletSpeed = 100f;

    private CapsuleCollider _col;
    //2
    private float vInput;
    private float hInput;

    //1.1
    private Rigidbody _rb;
    private GameBehavior _gameManager;

    //2.1
    void Start()
    {
        //3.1
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }


    void Update ()
    {


        //3
        vInput = Input.GetAxis("Vertical") * moveSpeed;

        //4
        hInput = Input.GetAxis("Horizontal") * rotateSpeed;

        /*5
        this.transform.Translate(Vector3.forward * vInput * Time.deltaTime);
        
        //6
        this.transform.Rotate(Vector3.up * hInput * Time.deltaTime);
        */
        if(IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            Debug.Log("Jump performed");
        }

        //1.02
        if (Input.GetMouseButtonDown(0))
        {
            // GameObject newBullet = Instantiate(bullet, this.transform.position + new Vector3(1,0,0), this.transform.rotation) as GameObject;
            GameObject newBullet = Instantiate(bullet, this.transform.position + this.transform.right, this.transform.rotation) as GameObject; 
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = this.transform.forward * bulletSpeed;
        }
    }
    void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);
        _rb.MovePosition(this.transform.position + this.transform.forward * vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);
        
    }
    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center,capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        /*
        Debug.Log(grounded);
        */
        return grounded;
    }
       
}
