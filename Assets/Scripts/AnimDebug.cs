using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDebug : MonoBehaviour
{
    public Animator anim;
    public float speed;
    public bool isOnGround;
    public bool holdingRoll;
    private float xInput;
    private float yInput;
    private Rigidbody rb;
    public float jumpForce;

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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("HoldingRoll", true);
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("HoldingRoll", false);
        }
    }
}
