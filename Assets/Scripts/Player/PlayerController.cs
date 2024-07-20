using System.Collections;
using System.Collections.Generic;
//using UnityEngine.EventSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	Rigidbody rb;
	Camera cam;
	float moveSpeed;
	//float airborneMoveSpeed;
	float currentMoveSpeed;
	float sprintSpeed;
	float jumpForce;
	//float individualGravityAcceleration;
	float mouseX;
	float mouseY;
	[SerializeField]float sensitivity;
	float kbX;
	float kbZ;
	bool sprint;
    bool isDashing;
	bool jump;
    public float acceleration;
	public bool cursorLock;
    public GameObject PauseMenu;
    Vector3 velocity;
    [SerializeField]private AudioClip dashClip, jumpClip;
    private AudioSource aSource;
//    [SerializeField]private float stamina;
//    [SerializeField]private float maxStamina;
//    private float staminaPercentage;
//    public Image DashMeter;
    public StaminaController staminaController;
    bool isJumping;
    Vector3 targetCameraRotation;
    private LayerMask playermask;
	
// Start is called before the first frame update
// MAKE SURE TO CORRECTLY SET MASS OF PLAYER'S RIGIDBODY
// MASS = 50 
    void Start()
    {
    	//Initializing variables
    	rb = gameObject.GetComponent<Rigidbody>();
    	cam = GetComponentInChildren<Camera>();
        staminaController = gameObject.GetComponent<StaminaController>();
        moveSpeed = 5;
        //airborneMoveSpeed = 4;
        sprintSpeed = 25;
        jumpForce = 150;
        //sensitivity = 200;
        cursorLock = true;
        Cursor.lockState = CursorLockMode.Locked;
        mouseIn();
        kbIn();
        SprintIn();
        jumpIn();
        isDashing = false;
        isJumping = false;
        aSource = gameObject.GetComponent<AudioSource>();
        //stamina = maxStamina;
        //staminaPercentage = stamina/maxStamina;
        //DashMeter = GameObject.Find("DashMeter").GetComponent<Image>();
        targetCameraRotation = Vector3.zero;
        playermask = ~(LayerMask.GetMask("Player"));
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
        Dash();
    }
    
//FixedUpdate is called once every .02 seconds
    void FixedUpdate()
    {
    	//Executing physics
        //rotate();
        //Dash();
        move();
        jumpExec();
        //regenStamina();
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
        Vector3 playerFeetPos = transform.position;
        playerFeetPos.y = playerFeetPos.y - gameObject.GetComponent<CapsuleCollider>().bounds.extents.y;
        print("isGrounded was run" + playerFeetPos);
        return Physics.CheckSphere(playerFeetPos, .25f, playermask);
    	
        //return Physics.Raycast(transform.position, Vector3.down, gameObject.GetComponent<CapsuleCollider>().bounds.extents.y + .1f);
    }
    
    /*public void SpeedCap()
    {
    	//if(isGrounded())
    	//{
        //    if(sprint)
            {
            currentMoveSpeed = sprintSpeed;
            }
            else
            {
			currentMoveSpeed = moveSpeed;
            }    	
    	//}
    	//else
    	//{
    	//	currentMoveSpeed = airborneMoveSpeed;
    	//}
        //print(currentMoveSpeed);
    }*/

    public void Dash()
    {
        if(Input.GetButtonDown("Sprint") && !isDashing && staminaController.CanDash())
        {
            Vector3 targetVelocity = transform.TransformDirection(new Vector3((kbX * sprintSpeed), 0 , (kbZ * sprintSpeed)));
            if(targetVelocity != Vector3.zero)
            {
                staminaController.UseStamina();
                isDashing = true;
                rb.velocity = targetVelocity;
                Invoke(nameof(resetDash), 0.25f);
                aSource.PlayOneShot(dashClip);
            }
            else
            {
                staminaController.UseStamina();
                targetVelocity = gameObject.transform.forward * sprintSpeed;
                isDashing = true;
                rb.velocity = targetVelocity;
                Invoke(nameof(resetDash), 0.25f);
                aSource.PlayOneShot(dashClip);
            }
        }
    }

    private void resetDash()
    {
        rb.velocity = rb.velocity.normalized * moveSpeed;
        isDashing = false;
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }

    private void resetJump()
    {
        isJumping = false;
    }

    /*private void regenStamina()
    {
        if(!isDashing)
        {
            stamina += 1.0f * Time.fixedDeltaTime;
            if(stamina > 3.0f)
            {
                stamina = 3.0f;
            }
        }
        staminaPercentage = stamina/maxStamina;
        DashMeter.fillAmount = staminaPercentage;
    }*/

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

    public void setSensitivity(float sense)
    {
        print(sense);
        sensitivity = sense;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Restart Button Pressed");
    }

//Physics execution functions
    public void rotate()
    {
    	Vector3 playerRotation = new Vector3(0f, mouseX, 0f) * sensitivity;
    	Vector3 cameraRotation = new Vector3(mouseY, 0f, 0f) * sensitivity;
    	
        

    	gameObject.transform.localEulerAngles += playerRotation * Time.deltaTime;

        //if (cam.transform.rotation.eulerAngles.z != 180)
        //{
        //    targetCameraRotation += (-cameraRotation) * Time.deltaTime;
        //    cam.transform.localEulerAngles = targetCameraRotation;
        //}
        //else if (cam.transform.rotation.eulerAngles.x + (-cameraRotation.x) > 270)
        //{
        //    cam.transform.rotation = Quaternion.Euler(new Vector3(270.0f, gameObject.transform.rotation.eulerAngles.y, 0f));
        //}
        //else
        //{
        //    cam.transform.rotation = Quaternion.Euler(new Vector3(90.0f, gameObject.transform.rotation.eulerAngles.y , 0f));
        //}

        targetCameraRotation += (-cameraRotation) * Time.deltaTime;

        if(targetCameraRotation.x > 90)
        {
            targetCameraRotation.x = 90;
        }
        else if(targetCameraRotation.x < -90)
        {
            targetCameraRotation.x = -90;
        }
        cam.transform.localEulerAngles = targetCameraRotation;
        //print(targetCameraRotation);
    }
    
    public void move()
    {	if(!isDashing)
        {
            Vector3 targetVelocity = transform.TransformDirection(new Vector3((kbX * moveSpeed), rb.velocity.y , (kbZ * moveSpeed)));
            //print(currentMoveSpeed);
            //print(movementVector);
            Vector3 deltaV = targetVelocity - rb.velocity;
            Vector3 accel = deltaV/Time.fixedDeltaTime;
        

            if(accel.sqrMagnitude > acceleration * acceleration)
            {
                accel.Normalize();
                accel *= acceleration;
            }

    	    rb.velocity += accel * Time.fixedDeltaTime;
        }
    	
    }
    
    public void jumpExec()
    {
    	if(jump && !isJumping && isGrounded())
    	{
    		Vector3 jumpVector = new Vector3(0f, jumpForce, 0f);
            rb.AddForce(jumpVector, ForceMode.Impulse);
            aSource.PlayOneShot(jumpClip);
            isJumping = true;
            Invoke(nameof(resetJump), .25f);
    	}
    }
    
}
