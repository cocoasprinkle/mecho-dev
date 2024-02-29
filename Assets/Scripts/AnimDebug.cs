using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDebug : MonoBehaviour
{
    public Animator anim;
    public float speed;
    public bool isOnGround;
    public bool holdingRoll;
    public float xInput;
    public float yInput;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("onGround", true);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        if (yInput >= 0)
        {
            speed = yInput * 5;
        }
        if (yInput < 0)
        {
            speed = yInput * -5;
        }
        anim.SetFloat("Speed", speed);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
            anim.SetTrigger("JumpTrig");
            isOnGround = false;
            anim.SetBool("onGround", false);
            Debug.Log("JUMPING");
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("HoldingRoll", true);
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("HoldingRoll", false);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            anim.SetBool("onGround", true);
        }
    }
}
