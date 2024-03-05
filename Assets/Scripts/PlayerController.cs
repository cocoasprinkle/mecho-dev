using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Variables that have [SerializeField] proceeding them can be edited or manipulated in the Inspector, while keeping them private. Headers are used to organise these variables in the Inspector
    [Header("References")]
    [SerializeField] Animator anim;
    [SerializeField] CinemachineFreeLook freeLook;

    [Header("Movement Settings")]
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float rotSpeed = 15f;
    [SerializeField] float smoothTime = 0.2f;
    [SerializeField] float coyoteDuration = 0.1f;
    [SerializeField] float inclineLimit = 80f;
    [SerializeField] bool normalizeVectors = true;
    [SerializeField] LayerMask groundLayer;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpMaxHeight = 2f;
    [SerializeField] float gravMod = 3f;

    [Header("Monitor Vars")]
    [SerializeField] bool isOnGround;
    [SerializeField] float jumpCount;
    [SerializeField] bool performedJump = false;
    [SerializeField] bool hasDoubleJumped = false;
    [SerializeField] bool holdingJump = false;
    [SerializeField] bool performedDive = false;
    [SerializeField] bool canDive = true;
    [SerializeField] float jumpVel;
    [SerializeField] float diveForwardForce;
    [SerializeField] float diveUpwardForce;
    [SerializeField] float diveUpwardVel;
    [SerializeField] public float curSpeed;
    [SerializeField] float xInput;
    [SerializeField] float yInput;
    [SerializeField] AnimatorStateInfo curClipName;

    [Header("Sounds")]
    [SerializeField] AudioClip[] jumpSounds;
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip runSound;
    private AudioClip noAudio;

    private Rigidbody rb;
    private AudioSource audSource;
    private AnimatorClipInfo curAnimInfo;
    float velocity;
    float diveDuration = 0.5f;
    UITimer timer;

    // Used for when floats need to be assigned a null value
    const float ZeroF = 0f;

    // Variables relating to player movement and input
    Vector3 movement;

    // Locates the transform values of the camera in the scene
    Transform mainCam;

    // Declares a list of timers, along with timers in that list
    List<Timer> timers;
    CountdownTimer jumpTimer;
    CountdownTimer diveTimer;
    CountdownTimer coyoteTime;

    // Awake is called when the script is activated
    void Awake()
    {
        // Gathers component and transform references needed for variables
        rb = GetComponent<Rigidbody>();
        audSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        mainCam = Camera.main.transform;
        freeLook.Follow = transform;
        freeLook.LookAt = transform;
        timer = GameObject.Find("Time").GetComponent<UITimer>();

        // Sets up the position and direction for the free-look camera, relative to the player
        freeLook.OnTargetObjectWarped(transform, transform.position - freeLook.transform.position - Vector3.forward);

        // Extra timer declaration
        jumpTimer = new CountdownTimer(jumpDuration);
        diveTimer = new CountdownTimer(diveDuration);
        coyoteTime = new CountdownTimer(coyoteDuration);
        timers = new List<Timer>(2) { jumpTimer, coyoteTime };
        audSource.clip = noAudio;
        //Stops the cursor from leaving the game window and makes it invisible
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {      
        HandleAnimator();
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        curClipName = anim.GetCurrentAnimatorStateInfo(0);
    }

    // FixedUpdate is called at a fixed rate
    void FixedUpdate()
    {
        HandleJump();
        HandleMovement();
        HandleTimers();
        HandleFootsteps();

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // HandleHorizontalMovement changes the direction and velocity of the player when turning
    void HandleHorizontalMovement(Vector3 adjustedDirection)
    {
        Vector3 velocity = adjustedDirection * maxSpeed * Time.fixedDeltaTime;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    // HandleMovement is largely responsible for calling movement voids when the adjustedDirection variable, declared in the void, has a magnitude greater than 0
    void HandleMovement()
    {
        if (normalizeVectors)
        {
            movement = new Vector3(xInput, 0f, yInput).normalized;
        }
        else
        {
            movement = new Vector3(xInput, 0f, yInput);
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
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        Debug.Log("this was called lmao");
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.75f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;
            Debug.Log(adjustedVelocity);
            if (adjustedVelocity.y < 0)
            {
                Debug.Log("ERMMM ADJUSTED!");
                return adjustedVelocity; 
            }
        }
        Debug.Log("not ADJUSTED!");
        return velocity;
    }
    
    // HandleJump handles all jump-related functions, controlling the jump's velocity overtime, stopping the player from jumping in midair, manipulating timers and changing the Rigidbody's velocity to match the jump velocity on the y axis
    void HandleJump()
    {
        if (!jumpTimer.IsRunning && isOnGround)
        {
            jumpVel = ZeroF;
            jumpCount = 0;
            jumpTimer.Stop();
            hasDoubleJumped = false;
        }
        if (Input.GetButton("Jump"))
        {
            holdingJump = true;
        }
        else
        {
            holdingJump = false;
        }

        if (holdingJump && isOnGround && !jumpTimer.IsRunning || holdingJump && !jumpTimer.IsRunning && jumpCount < 2)
        {
            performedJump = true;
        }
        else
        {
            performedJump = false;
        }

        if (performedJump)
        {
            jumpCount = jumpCount + 1;
            jumpTimer.Start();
            anim.SetTrigger("JumpTrig");
        }
        if (jumpTimer.IsRunning && jumpVel < (jumpForce) || jumpTimer.IsRunning && jumpCount == 0)
        {
            audSource.loop = false;
            AudioClip jumpClip = jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Length)];
            audSource.clip = jumpClip;
            audSource.Stop();
            audSource.PlayOneShot(jumpClip);
        }
        else if (!holdingJump && jumpTimer.IsRunning)
        {
            jumpTimer.Stop();
        }

        if (!holdingJump && !jumpTimer.IsRunning && !hasDoubleJumped && !isOnGround)
        {
            jumpCount = 1;
            hasDoubleJumped = true;
        }

        if (jumpTimer.IsRunning)
        {
            float launchPoint = 0.9f;
            if (jumpTimer.Progress > launchPoint)
            {
                // Calculates the decreasing jump velocity using square roots of the maximum height multiplied by 2 and returning the absolute value of the y value of gravity in the project
                jumpVel = Mathf.Sqrt(2 * jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
            }
            else
            {
                jumpVel += (1 - jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
            }
        }
        else
        {
            jumpVel += Physics.gravity.y * gravMod * Time.fixedDeltaTime;
        }
        rb.velocity = new Vector3(rb.velocity.x, jumpVel, rb.velocity.z);
    }

    // HandleTimers ensures that timers tick relative to the deltaTime of the scene, as well as any functions triggered by the end of specific timers
    void HandleTimers()
    {
        foreach (var timer in timers)
        {
            timer.Tick(Time.deltaTime);
        }
    }

    // HandleRot rotates the player in the direction of movement
    void HandleRot(Vector3 adjustedDirection)
    {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
    }

    // SmoothSpeed eases the rotation of HandleRot using the SmoothDamp function in Mathf and angle functions in Quaternion
    float SmoothSpeed(float value)
    {
        var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;
        var adjustedMovement = adjustedDirection * (maxSpeed * Time.deltaTime);
        if (adjustedMovement.magnitude > 0.1f)
        {
            curSpeed = Mathf.SmoothDamp(curSpeed, adjustedMovement.magnitude, ref velocity, smoothTime);
        }
        else
        {
            curSpeed = 0f;
        }
        return curSpeed;
    }

    // HandleAnimator is responsible for setting the values of Animator booleans and floats to be relative to corresponding variables in the script
    void HandleAnimator()
    {
        anim.SetFloat("Speed", curSpeed);
        anim.SetBool("onGround", isOnGround);
        // anim.SetBool("HoldingRoll", rolling);
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Time End"))
        {
            timer.timerActive = false;
        }
    }

    // OnCollisionStay serves as the script's "ground check" by using the Capsule Collider's bounds along with collision-related variables and Vector3 functions
    void OnCollisionStay(Collision collision)
    {
        var bottom = GetComponent<CapsuleCollider>().bounds.center;
        bottom.y -= GetComponent<CapsuleCollider>().bounds.extents.y;
        float minDist = float.PositiveInfinity;
        float angle = 180f;
        // Find closest point to bottom.
        for (int i = 0; i < collision.contactCount; i++)
        {
            var contact = collision.GetContact(i);
            var tempDist = Vector3.Distance(contact.point, bottom);
            if (tempDist < minDist)
            {
                minDist = tempDist;
                // Check how close the contact normal is to our up vector.
                angle = Vector3.Angle(transform.up, contact.normal);
            }
        }
        // Check if the angle is too steep.
        if (angle <= inclineLimit)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }

    // OnCollsionExit's purpose is to set the "isOnGround" variable to false if the player is not colliding with any objects
    void OnCollisionExit(Collision collision)
    {
        isOnGround = false;    
    }

    void HandleFootsteps()
    {
        if (isOnGround && !jumpTimer.IsRunning)
        {
            audSource.loop = true;
            audSource.clip = runSound;
        }

        if (audSource.clip == walkSound && !audSource.isPlaying || audSource.clip == runSound && !audSource.isPlaying)
        {
            audSource.Play();
        }
        if (curSpeed < 1 || !isOnGround && !jumpTimer.IsRunning)
        {
            audSource.Stop();
            audSource.clip = noAudio;
        }
    }

// This abstract class handles all timer logic, allowing for timers to be invoked and controlled by the rest of the script (courtesy of "git-amend", a YouTuber whose tutorials have been very helpful)
public abstract class Timer
{
    protected float initialTime;
    protected float Time { get; set; }
    public bool IsRunning { get; protected set; }

    public float Progress => Time / initialTime;

    public Action OnTimerStart = delegate { };
    public Action OnTimerStop = delegate { };

    protected Timer(float value)
    {
        initialTime = value;
        IsRunning = false;
    }

    public void Start()
    {
        Time = initialTime;
        if (!IsRunning)
        {
            IsRunning = true;
            OnTimerStart.Invoke();
        }
    }

    public void Stop()
    {
        if (IsRunning)
        {
            IsRunning = false;
            OnTimerStop.Invoke();
        }
    }

    public void Resume() => IsRunning = true;
    public void Pause() => IsRunning = false;

    public abstract void Tick(float deltaTime);
}

// This public class handles the logic of countdown timers specifically
public class CountdownTimer : Timer
{
    public CountdownTimer(float value) : base(value) { }

    public override void Tick(float deltaTime)
    {
        if (IsRunning && Time > 0)
        {
            Time -= deltaTime;
        }

        if (IsRunning && Time <= 0)
        {
            Stop();
        }
    }

    public bool IsFinished => Time <= 0;

    public void Reset() => Time = initialTime;

    public void Reset(float newTime)
    {
        initialTime = newTime;
        Reset();
    }
}

// This public class handles the logic of stopwatch timers specifically
public class StopwatchTimer : Timer
{
    public StopwatchTimer() : base(0) { }

    public override void Tick(float deltaTime)
    {
        if (IsRunning)
        {
            Time += deltaTime;
        }
    }

    public void Reset() => Time = 0;

    public float GetTime() => Time;
}
}