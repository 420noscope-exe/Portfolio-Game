using System.Collections;
using System.Collections.Generic;
//using UnityEngine.EventSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Rigidbody rb;
	Camera cam;
	float moveSpeed;
	float airborneMoveSpeed;
	float currentMoveSpeed;
	float sprintSpeed;
	float jumpForce;
	float individualGravityAcceleration;
	float mouseX;
	float mouseY;
	float sensitivity;
	float kbX;
	float kbZ;
	bool sprint;
	bool jump;
	public bool cursorLock;
    public GameObject PauseMenu;
	
// Start is called before the first frame update
// MAKE SURE TO CORRECTLY SET MASS OF PLAYER'S RIGIDBODY
// MASS = 50 
    void Start()
    {
    	//Initializing variables
    	rb = gameObject.GetComponent<Rigidbody>();
    	cam = GetComponentInChildren<Camera>();
        moveSpeed = 5;
        airborneMoveSpeed = 4;
        sprintSpeed = 7;
        jumpForce = 150;
        sensitivity = 200;
        cursorLock = true;
        Cursor.lockState = CursorLockMode.Locked;
        mouseIn();
        kbIn();
        SprintIn();
        jumpIn();
    }

// Update is called once per frame
    void Update()
    {	
    	//Collecting input data
        mouseIn();
        kbIn();
        SprintIn();
        jumpIn();
        cursorLockIn();
        cursorLockInA();
        rotate();
        cursorLockExec();
    }
    
//FixedUpdate is called once every .02 seconds
    void FixedUpdate()
    {
    	//Executing physics
        //rotate();
        SpeedCap();
        move();
        jumpExec();
    
        //individualGravity();
    }
    
//Defining input functions
    public void mouseIn()
    {
    	mouseX = Input.GetAxisRaw("Mouse X");
    	mouseY = Input.GetAxisRaw("Mouse Y");
    }
    
    public void kbIn()
    {
    	kbX = Input.GetAxisRaw("Horizontal");
    	kbZ = Input.GetAxisRaw("Vertical");
    }
    
    public void SprintIn()
    {
    	sprint = Input.GetButton("Sprint");
    }
    
    public void jumpIn()
    {
    	jump = Input.GetButton("Jump");
    }
    
    public void cursorLockIn()
    {
    	if(Input.GetButtonDown("Menu"))
    	{
            print("Menu Button Pressed");
    		cursorLock = !cursorLock;
    	}
    }

    public void cursorLockInA()
    {
    	if(Input.GetButtonDown("Cancel"))
    	{
            print("Cancel Button Pressed");
    	
    	}
    }
    
//Additional utilty functions
    public bool isGrounded()
    {
    	return Physics.Raycast(transform.position, Vector3.down, gameObject.GetComponent<CapsuleCollider>().bounds.extents.y + .1f);
    }
    
    public void SpeedCap()
    {
    	if(isGrounded())
    	{
            if(sprint)
            {
            currentMoveSpeed = sprintSpeed;
            }
            else
            {
			currentMoveSpeed = moveSpeed;
            }    	
    	}
    	else
    	{
    		currentMoveSpeed = airborneMoveSpeed;
    	}
        //print(currentMoveSpeed);
    }

    public void cursorLockExec()
    {
        if(cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Resume();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }

    public void resumeButton()
    {
        cursorLock = !cursorLock;
        print("Resume Button Pressed");
    }
    
    public void restartButton()
    {
        print("Restart Button Pressed");
    }

//Physics execution functions
    public void rotate()
    {
    	Vector3 playerRotation = new Vector3(0f, mouseX, 0f) * sensitivity;
    	Vector3 cameraRotation = new Vector3(mouseY, 0f, 0f) * sensitivity;
    	
    	gameObject.transform.localEulerAngles += playerRotation * Time.deltaTime;

        if (cam.transform.rotation.eulerAngles.z != 180)
        {
            cam.transform.localEulerAngles += (-cameraRotation) * Time.deltaTime;
        }
        else if (cam.transform.rotation.eulerAngles.x > 260)
        {
            cam.transform.rotation = Quaternion.Euler(new Vector3(270.0f, gameObject.transform.rotation.eulerAngles.y, 0f));
        }
        else
        {
            cam.transform.rotation = Quaternion.Euler(new Vector3(90.0f, gameObject.transform.rotation.eulerAngles.y , 0f));
        }
    }
    
    public void move()
    {	
    	Vector3 movementVector = transform.TransformDirection(new Vector3((kbX * currentMoveSpeed), rb.velocity.y , (kbZ * currentMoveSpeed)));
        //print(currentMoveSpeed);
        //print(movementVector);
    	rb.velocity = movementVector;
    }
    
    public void jumpExec()
    {
    	if(isGrounded() && jump)
    	{
    		Vector3 jumpVector = new Vector3(0f, jumpForce, 0f);
                rb.AddForce(jumpVector, ForceMode.Impulse);
    	}
    }
    
}
