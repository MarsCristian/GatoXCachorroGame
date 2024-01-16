using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;            
    public float input_x = 0, input_y = 0;
    public float normalMove = 1;
    private Rigidbody2D rb;
    public float maxDashTime = 0.2f;
    public float dashStoppingSpeed = 0.1f;
    public float currentDashTime;
    public Vector3 moveDirection;
    public float dashSpeed = 6f;
    public bool canMove;

    public bool alert;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start(){
        alert = false;
        currentDashTime = 0f;
        canMove = true;
    }

    void Update()
    {
        if(canMove){
            input_x = Input.GetAxisRaw("Horizontal");
            input_y = Input.GetAxisRaw("Vertical");
            if (input_x != 0 && input_y != 0)
                normalMove = 0.7071067812f;
            else
                normalMove = 1f;
            if(Input.GetButtonDown("Fire2") && canMove){
                canMove = false;
                Debug.Log("semata");
            }
        }
        else{
            if(currentDashTime <= maxDashTime){
                moveDirection = new Vector3(input_x * Time.deltaTime * dashSpeed * normalMove * moveSpeed, input_y * Time.deltaTime * dashSpeed * normalMove * moveSpeed, 0);
                currentDashTime += dashStoppingSpeed * Time.deltaTime;
            }
            else{
                moveDirection = Vector3.zero;
                canMove = true;
                currentDashTime = 0f;
            }
            rb.velocity = moveDirection;
        }
    }

    void FixedUpdate()
    {
        if(canMove)
            rb.velocity = new Vector3(input_x * moveSpeed * normalMove, input_y * moveSpeed * normalMove, 0);
    }

    void OnTriggerStay2D(Collider2D sensorCollision){
        if (sensorCollision.gameObject.tag == "Sensors"){
            alert = true;
        }
    }


/*
    void Update () {
        if (Input.GetButtonDown("Fire2")) //Right mouse button
        {
            currentDashTime = 0;                
        }
        if(currentDashTime < maxDashTime)
        {
            moveDirection = transform.forward * dashDistance;
            currentDashTime += dashStoppingSpeed;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
        controller.Move(moveDirection * Time.deltaTime * dashSpeed);
	}
*/

}
