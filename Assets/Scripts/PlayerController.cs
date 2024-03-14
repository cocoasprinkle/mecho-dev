using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;

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
    [SerializeField] float groundStick = -6f;
    [SerializeField] float coyoteDuration = 0.25f;
    [SerializeField] float inclineLimit = 80f;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpMaxHeight = 2f;
    [SerializeField] float gravMod = 3f;

    [Header("Monitor Vars")]
    [SerializeField] public bool canInput = false;
    [SerializeField] public bool isOnGround;
    [SerializeField] float jumpCount = 2;
    [SerializeField] bool performedJump = false;
    [SerializeField] bool holdingJump = false;
    [SerializeField] float jumpVel;
    [SerializeField] float diveForwardForce;
    [SerializeField] float diveUpwardForce;
    [SerializeField] float diveUpwardVel;
    [SerializeField] public float curSpeed;
    [SerializeField] float xInput;
    [SerializeField] float yInput;
    [SerializeField] AnimatorStateInfo curClipName;
    [SerializeField] public bool noInput;
    [SerializeField] public bool inCoyote;

    [Header("Sounds")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    private AudioClip noAudio;
    public AudioSource raceAudSource;
    public AudioClip raceStart;
    public AudioClip raceEnd;

    [Header("Particles")]
    [SerializeField] ParticleSystem dustParticle;
    [SerializeField] ParticleSystem jumpParticle;
    [SerializeField] ParticleSystem landParticle;
    [SerializeField] bool hasPlayedLandParticle = false;

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
        raceAudSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        // Sets up the position and direction for the free-look camera, relative to the player
        freeLook.OnTargetObjectWarped(transform, transform.position - freeLook.transform.position - Vector3.forward);

        // Extra timer declaration
        jumpTimer = new CountdownTimer(jumpDuration);
        diveTimer = new CountdownTimer(diveDuration);
        coyoteTime = new CountdownTimer(coyoteDuration);
        timers = new List<Timer>(2) { jumpTimer, coyoteTime };
        audSource.clip = noAudio;
        //Makes the cursor invisible

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        StartCoroutine(StartPauseInputs());
    }

    IEnumerator StartPauseInputs()
    {
        canInput = false;
        yield return new WaitForSeconds(1);
        canInput = true;
        raceAudSource.PlayOneShot(raceStart, 0.45f);
    }

    // Update is called once per frame
    void Update()
    {      
        HandleAnimator();
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        movement = new Vector3(xInput, 0f, yInput);
        curClipName = anim.GetCurrentAnimatorStateInfo(0);
    }

    // FixedUpdate is called at a fixed rate
    void FixedUpdate()
    {
        hasPlayedLandParticle = landParticle.isPlaying || isOnGround || coyoteTime.IsRunning;
        if (canInput)
        {
            HandleJump();
            HandleMovement();
            HandleTimers();
        }
        HandleParticles();
        CheckInput();
        if (!coyoteTime.IsRunning && inCoyote)
        {
            isOnGround = false;
            hasPlayedLandParticle = false;
            inCoyote = false;
            if (!jumpTimer.IsRunning && jumpCount == 0)
            {
                jumpCount = 1;
            }
            
        }
    }

    public void HandleMovement()
    {
        // Rotate movement direction to match camera rotation
        var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;

        if (adjustedDirection.magnitude > ZeroF)
        {
            HandleRotation(adjustedDirection);
            HandleHorizontalMovement(adjustedDirection);
            SmoothSpeed(adjustedDirection.magnitude);
        }
        else
        {
            SmoothSpeed(ZeroF);

            // Reset horizontal velocity for a snappy stop
            rb.velocity = new Vector3(ZeroF, rb.velocity.y, ZeroF);
        }
    }

    void HandleHorizontalMovement(Vector3 adjustedDirection)
    {
        // Move the player
        Vector3 velocity = adjustedDirection * (maxSpeed * Time.fixedDeltaTime);
        velocity = Vector3.ClampMagnitude(velocity, 22.5f);
        if (isOnGround && !Input.GetButton("Jump"))
        {
            rb.velocity = new Vector3(velocity.x, groundStick, velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
    }

    void HandleRotation(Vector3 adjustedDirection)
    {
        // Adjust rotation to match movement direction
        var targetRotation = Quaternion.LookRotation(adjustedDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    void SmoothSpeed(float value)
    {
        var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;
        if (adjustedDirection.magnitude > 0.2f)
        {
            curSpeed = Mathf.SmoothDamp(curSpeed, value, ref velocity, smoothTime);
            curSpeed = Mathf.Clamp(curSpeed, 0, 1);
        }
        else
        {
            curSpeed = 0;
        }
    }

    // HandleJump handles all jump-related functions, controlling the jump's velocity overtime, stopping the player from jumping in midair, manipulating timers and changing the Rigidbody's velocity to match the jump velocity on the y axis
    void HandleJump()
    {
        if (!jumpTimer.IsRunning && isOnGround)
        {
            jumpVel = ZeroF;
            jumpCount = 0;
            jumpTimer.Stop();
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

        if (performedJump && jumpVel <= 1f)
        {
            if (jumpVel >= -14f)
            {
                jumpCount = jumpCount + 1;
            }
            else
            {
                jumpCount = 2;
            }
            
            jumpTimer.Start();
            anim.SetTrigger("JumpTrig");
            audSource.loop = false;
            audSource.clip = jumpSound;
            audSource.Stop();
            audSource.PlayOneShot(jumpSound);
        }
        else if (!holdingJump && jumpTimer.IsRunning)
        {
            jumpTimer.Stop();
        }

        if (jumpTimer.IsRunning)
        {
            float launchPoint = 0.5f;
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
            raceAudSource.PlayOneShot(raceEnd);
            Destroy(collision.gameObject);
        }
    }

    // OnCollisionStay serves as the script's "ground check" by using the Box Collider's bounds along with collision-related variables and Vector3 functions
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
            if (!hasPlayedLandParticle)
            {
                landParticle.Play();
                audSource.loop = false;
                audSource.PlayOneShot(landSound);
            }
        }
        else
        {
            isOnGround = false;
        }
    }

    // OnCollsionExit's purpose is to set the "isOnGround" variable to false if the player is not colliding with any objects
    void OnCollisionExit(Collision collision)
    {
        //coyoteTime.Start();
        //inCoyote = true;
        isOnGround = false;
        hasPlayedLandParticle = false;
        inCoyote = false;
        /*if (!jumpTimer.IsRunning && jumpCount == 0)
        {
            jumpCount = 1;
        }*/
    }

    void CheckInput()
    {
        if (xInput < 0.02f && xInput > -0.02f && yInput < 0.02f && yInput > -0.02f)
        {
            noInput = true;
        }
        else
        {
            if (canInput)
            {
                noInput = false;
            }
            else
            {
                noInput = true;
            }            
        }
    }
    
    void HandleParticles()
    {
        if (!noInput && isOnGround)
        {
            dustParticle.Play();
        }
        else if (noInput && isOnGround || !isOnGround)
        {
            dustParticle.Stop();
        }

        if (performedJump && isOnGround)
        {
            jumpParticle.Play();
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