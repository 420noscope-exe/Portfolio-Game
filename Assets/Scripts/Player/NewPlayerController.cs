using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody rigidBody;
    private Camera cam;
    private AudioSource aSource;
    private StaminaController staminaController;

    //Steering Configuration
    [Header("Steering Configuration")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float sensitivity;

    //State Based Movement Configuration
    [Header("State Based Movement Configuration")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float jumpForce;

    //IsGrounded() Configuration
    [Header("IsGrounded() Configuration")]
    [SerializeField] private float checkSphereRadius;
    [SerializeField] private float checkSphereYOffset;

    //Input properties
    private float mouseX;
    private float mouseY;
    private float kbX;
    private float kbZ;
    private bool dashButton;
    private bool jumpButton;

    //States
    private bool isDashing;
    private bool isJumping;

    //Steering vectors
    private Vector3 targetCameraRotation;
    private Vector3 targetVelocity;

    //Player LayerMask.  Haha, see what I did there?
    private LayerMask playerMask;

    //Audio
    [SerializeField]private AudioClip dashClip, jumpClip;
    // Start is called before the first frame update
    void Start()
    {
        //defining component references
        rigidBody = gameObject.GetComponent<Rigidbody>();
        cam = gameObject.GetComponentInChildren<Camera>();
        staminaController = gameObject.GetComponent<StaminaController>();
        aSource = gameObject.GetComponent<AudioSource>();

        //initializing states
        isDashing = false;
        isJumping = false;

        //defining LayerMask
        playerMask = ~(LayerMask.GetMask("Player"));
        
        GetInputs(); //Initialize Input Properties

        sensitivity = PlayerPrefs.GetFloat("sensitivity", 100);
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    //FixedUpdate is called once every .02 seconds
    private void FixedUpdate()
    {
        Look();
        Move();
        Dash();
        Jump();
    }

    //Checks inputs and assign values to Input Properties
    private void GetInputs()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        kbX = Input.GetAxisRaw("Horizontal");
        kbZ = Input.GetAxisRaw("Vertical");
        dashButton = Input.GetButton("Sprint");
        jumpButton = Input.GetButton("Jump");
    }

    //Steering functions
    private void Look()
    {
        Vector3 playerRotation = new Vector3(0f, mouseX, 0f) * sensitivity;
        Vector3 cameraRotation = new Vector3(mouseY, 0f, 0f) * sensitivity;

        gameObject.transform.localEulerAngles += playerRotation * Time.deltaTime; //Player rotation can be assigned directly

        targetCameraRotation += (-cameraRotation) * Time.deltaTime; //Camera rotation must be clamped before being assigned

        if(targetCameraRotation.x > 90)
        {
            targetCameraRotation.x = 90;
        }
        else if(targetCameraRotation.x < -90)
        {
            targetCameraRotation.x = -90;
        }
        cam.transform.localEulerAngles = targetCameraRotation;
    }

    private void Move()
    {
        if(!isDashing)
        {
            targetVelocity = transform.TransformDirection(new Vector3((kbX * moveSpeed), rigidBody.velocity.y , (kbZ * moveSpeed)));

            Vector3 deltaV = targetVelocity - rigidBody.velocity;
            Vector3 targetAcceleration = deltaV/Time.fixedDeltaTime;//Velocity/Time = Acceleration;

            if(targetAcceleration.sqrMagnitude > maxAcceleration * maxAcceleration)//Enforces maxAcceleration
            {
                targetAcceleration.Normalize();
                targetAcceleration *= maxAcceleration;
            }

            rigidBody.velocity += targetAcceleration * Time.fixedDeltaTime;
        }
    }

    //State based movement
    private void Dash()
    {
        if(dashButton && !isDashing && staminaController.CanDash())
        {
            targetVelocity = transform.TransformDirection(new Vector3((kbX * dashSpeed), 0 , (kbZ * dashSpeed)));
            if(targetVelocity != Vector3.zero)
            {
                staminaController.UseStamina();
                isDashing = true;
                rigidBody.velocity = targetVelocity;
                Invoke(nameof(ResetDash), dashDuration);
                if(dashClip != null)
                {
                    aSource.PlayOneShot(dashClip);
                }
            }
            else
            {
                staminaController.UseStamina();
                targetVelocity = gameObject.transform.forward * dashSpeed;
                isDashing = true;
                rigidBody.velocity = targetVelocity;
                Invoke(nameof(ResetDash), dashDuration);
                if(dashClip != null)
                {
                    aSource.PlayOneShot(dashClip);
                }
            }
        }
    }

    private void Jump()
    {
    	if(jumpButton && !isJumping && IsGrounded())
    	{
    		Vector3 jumpVector = new Vector3(0f, jumpForce, 0f);
            rigidBody.AddForce(jumpVector, ForceMode.Impulse);
            isJumping = true;
            Invoke(nameof(ResetJump), .25f);
            if(jumpClip != null)
            {
                aSource.PlayOneShot(jumpClip);
            }
    	}
    }

    private bool IsGrounded()
    {
        Vector3 playerFeetPos = transform.position;
        playerFeetPos.y = playerFeetPos.y - gameObject.GetComponent<CapsuleCollider>().bounds.extents.y + checkSphereYOffset;
        print("isGrounded was run" + playerFeetPos);
        return Physics.CheckSphere(playerFeetPos, checkSphereRadius, playerMask);
    }

    //State reset functions
    private void ResetDash()
    {
        if(!isJumping)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * moveSpeed;
        }
        isDashing = false;
    }

    private void ResetJump()
    {
        isJumping = false;
    }

    //public accessors and mutators
    public bool GetIsDashing()
    {
        return isDashing;
    }

    public void SetSensitivity(float sense)
    {
        print(sense);
        sensitivity = sense;
        PlayerPrefs.SetFloat("sensitivity", sense);
        PlayerPrefs.Save();
    }
}
