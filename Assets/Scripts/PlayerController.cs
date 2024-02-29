using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator anim;
    [SerializeField] CinemachineFreeLook freeLook;

    [Header("Settings")]
    [SerializeField] float speed = 5f;
    [SerializeField] float rotSpeed = 15f;
    [SerializeField] float smoothTime = 0.2f;
    [SerializeField] float jumpHeight = 7f;

    [Header("Ground Check Vars")]
    [SerializeField] float groundDistance = 0.08f;
    [SerializeField] LayerMask groundLayers;
    
    public bool isOnGround;
    private Rigidbody rb;

    const float ZeroF = 0f;

    float curSpeed;
    float velocity;
    float xInput;
    float yInput;

    Vector3 movement;

    Transform mainCam;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        mainCam = Camera.main.transform;
        freeLook.Follow = transform;
        freeLook.LookAt = transform;
        freeLook.OnTargetObjectWarped(transform, transform.position - freeLook.transform.position - Vector3.forward);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {      
        HandleAnimator();
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        movement = new Vector3(xInput, 0f, yInput);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("HoldingRoll", true);
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("HoldingRoll", false);
        }
    }

    void FixedUpdate()
    {
        //HandleJump();
        HandleMovement();
    }

    void HandleHorizontalMovement(Vector3 adjustedDirection)
    {
        Vector3 velocity = adjustedDirection * speed * Time.fixedDeltaTime;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            anim.SetTrigger("JumpTrig");
            isOnGround = false;
            anim.SetBool("onGround", false);
            Debug.Log("JUMPING");
        }
        var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;
        if (adjustedDirection.magnitude > ZeroF)
        {
            HandleRot(adjustedDirection);
            HandleHorizontalMovement(adjustedDirection);
            SmoothSpeed(adjustedDirection.magnitude);
        }
        else
        {
            SmoothSpeed(ZeroF);
        }
        anim.SetFloat("Speed", rb.velocity.magnitude);
    }
    

    void HandleRot(Vector3 adjustedDirection)
    {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
    }
    float SmoothSpeed(float value)
    {
        var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;
        var adjustedMovement = adjustedDirection * (speed * Time.deltaTime);
        curSpeed = Mathf.SmoothDamp(curSpeed, adjustedMovement.magnitude, ref velocity, smoothTime);
        return curSpeed;
    }
    void HandleAnimator()
    {
        anim.SetFloat("Speed", curSpeed);
    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            anim.SetBool("onGround",true);
        }
    }
}
