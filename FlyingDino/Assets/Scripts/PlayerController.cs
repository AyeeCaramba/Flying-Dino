using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(GravityController))]
[RequireComponent(typeof(OrbitalRotation))]
public class PlayerController : NetworkBehaviour
{

    #region Variables

    #region Modifiers

    public float targetSpeed;

    public float speedChange;

    public float jumpForce;

    public float fallModifier;

    public float jumpTimer;
    public float minimumJumpTimer;

    #endregion

    #region Input

    Vector2 inputVector;
    string moveVectorXName = "LSH";
    string moveVectorYName = "LSV";

    bool jumpButtonDown;
    bool jumpButtonUp;
    bool jumpButton;
    string jumpButtonName = "Jump";

    [HideInInspector]
    public bool attackButtonDown;
    [HideInInspector]
    public bool attackButtonUp;
    [HideInInspector]
    public bool attackButton;
    string attackButtonName = "Attack";

    bool barkButtonDown;
    string barkButtonName = "Woof";

    #endregion

    #region Checks

    int localPlayerNumber;
    int playerNumber;

    bool startedJump;
    bool jumpFinished;
    bool velocityReset = false;

    float currentJumpTimer;

    public LayerMask groundLayer;

    [HideInInspector]
    public bool facingRight;

    public float groundDistance = 0.1f;

    #endregion

    #region Classes

    Rigidbody2D rBody;

    GravityController gravController;

    OrbitalRotation orbitController;

    Animator animController;

    Gun gun;

    new Collider2D collider;

    #endregion

    #region Properties

    bool isGrounded
    {
        get
        {
            //Debug.DrawLine(transform.position - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .95f)), transform.position - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .95f)) - (transform.up * groundDistance), Color.red, 0.05f);

            Debug.DrawRay(transform.position - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .90f)), -transform.up, Color.black);

            List<RaycastHit2D> info = new List<RaycastHit2D>(Physics2D.RaycastAll(transform.position - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .90f)), -transform.up, groundDistance));

            foreach (RaycastHit2D hitInfo in info)
                if (hitInfo.collider != null && hitInfo.transform != transform)
                    return true;

            return false;
        }
    }

    #endregion

    #endregion

    #region Functions

    #region Unity

    void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();

        collider = GetComponent<Collider2D>();

        gravController = GetComponent<GravityController>();

        orbitController = GetComponent<OrbitalRotation>();

        animController = GetComponentInChildren<Animator>();

        gun = GetComponentInChildren<Gun>();
    }

    // Use this for initialization
    void Start()
    {
        PlayerManager.instance.players.Add(this);

        playerNumber = PlayerManager.instance.players.Count;

        if (!isLocalPlayer)
        {
            gravController.enabled = false;
            orbitController.enabled = false;
            enabled = false;
        }
        else
        {
            PlayerManager.instance.localPlayers.Add(this);
            localPlayerNumber = PlayerManager.instance.localPlayers.Count;
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ApplyMovement();
        Jump();
        ApplyAnimation();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.tag + " was tiggered");

        if(other.gameObject.tag == ("Bullet"))
        {
            if(1>0)
            {
                Debug.Log("Ha");
            }
        }
    }

    #endregion

    void GetInput()
    {
        inputVector = new Vector2(Input.GetAxis(moveVectorXName + localPlayerNumber), Input.GetAxis(moveVectorYName + localPlayerNumber));
        if (inputVector.x > .2) facingRight = true;
        else if (inputVector.x < -.2) facingRight = false;

        jumpButtonDown = Input.GetButtonDown(jumpButtonName + localPlayerNumber);
        jumpButtonUp = Input.GetButtonUp(jumpButtonName + localPlayerNumber);
        jumpButton = Input.GetButton(jumpButtonName + localPlayerNumber);

        attackButtonDown = Input.GetButtonDown(attackButtonName + localPlayerNumber);
        attackButtonUp = Input.GetButtonUp(attackButtonName + localPlayerNumber);
        attackButton = Input.GetButton(attackButtonName + localPlayerNumber);

        barkButtonDown = Input.GetButtonDown(barkButtonName + localPlayerNumber);
    }

    void ApplyAnimation()
    {
        Vector2 animationMovementVector = transform.TransformVector(rBody.velocity);

        Debug.Log(animationMovementVector.magnitude);

        if (animationMovementVector.magnitude > 1.2f)
        {
            animController.SetBool("IsIdle", false);
        }
        else
        {
            animController.SetBool("IsIdle", true);
            animController.SetInteger("IdleID", Random.Range(1, 2));
        }

        if(startedJump && !animController.GetBool("Fall"))
        {
            animController.SetBool("IsJump", true);
            if(jumpFinished)
                animController.SetBool("Fall", true);
        }
        else if (animController.GetBool("Fall") && isGrounded)
        {
            animController.SetBool("IsJump", false);
            animController.SetBool("Fall", false);
        }

        animController.SetBool("IsHoldingWeapon", gun.hasGun);

        if (barkButtonDown)
        {
            animController.Play("DinosaurBark", 3, 0f);
            animController.SetBool("IsBarking", true);
            animController.SetBool("BarkingToggle", true);
        }
    }

    void ApplyMovement()
    {
        if (facingRight)
            transform.localScale = new Vector3(1,transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        Vector2 targetVelocity = new Vector2(inputVector.x, 0);

        targetVelocity = targetVelocity * targetSpeed;

        Vector2 velocity = transform.InverseTransformDirection(rBody.velocity);
        Vector2 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -speedChange, speedChange);
        velocityChange.y = 0;

        velocityChange = transform.TransformDirection(velocityChange);
        rBody.velocity += velocityChange;
    }

    void Jump()
    {
        Debug.Log(isGrounded);
        if (isGrounded && !startedJump)
        {
            if (jumpButtonDown)
            {
                startedJump = true;
                currentJumpTimer = 0;
            }
        }
        else if (isGrounded && jumpFinished)
        {
            startedJump = false;
            jumpFinished = false;
            velocityReset = false;
        }

        else if (inputVector.y < 0 && !isGrounded)
        {
            Debug.Log(inputVector);
            rBody.velocity += new Vector2(-transform.up.x, -transform.up.y) * fallModifier;

        }

        if (!isGrounded && jumpFinished)
        {
            Vector3 tempVel = rBody.velocity;
            Vector3 tempPos = transform.position + new Vector3(rBody.velocity.x, rBody.velocity.y, 0);

            if (!velocityReset && checkTempGround(tempPos))
            {
                resetVerticalVelocity();
                velocityReset = true;
            }
        }

        if (startedJump && !jumpFinished && (jumpButton || currentJumpTimer < minimumJumpTimer) && currentJumpTimer < jumpTimer)
        {
            currentJumpTimer += Time.deltaTime;
            rBody.velocity += new Vector2(transform.up.x, transform.up.y) * jumpForce * Time.deltaTime;
        }
        else if (startedJump && !jumpFinished) jumpFinished = true;
    }

    float PercentageRange(float min, float max, float val)
    {
        return (val - min) / (max - min);
    }

    bool checkTempGround(Vector3 pos)
    {
        Debug.DrawRay(pos - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .90f)), -transform.up, Color.red);

        List<RaycastHit2D> info = new List<RaycastHit2D>(Physics2D.RaycastAll(pos - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .90f)), -transform.up, groundDistance));

        foreach (RaycastHit2D hitInfo in info)
            if (hitInfo.collider != null && hitInfo.transform != transform)
                return true;

        return false;
    }

    void resetVerticalVelocity()
    {
        rBody.velocity = transform.TransformVector(rBody.velocity);
        rBody.velocity = new Vector2(rBody.velocity.x, 0);
        rBody.velocity = transform.InverseTransformVector(rBody.velocity);
    }

    #endregion

}