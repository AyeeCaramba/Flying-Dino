using UnityEngine;
<<<<<<< Updated upstream
using UnityEngine.Networking;
=======
>>>>>>> Stashed changes
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(GravityController))]
[RequireComponent(typeof(OrbitalRotation))]
<<<<<<< Updated upstream
public class PlayerController : NetworkBehaviour
=======
public class PlayerController : MonoBehaviour
>>>>>>> Stashed changes
{

    #region Variables

    #region Modifiers

    public float targetSpeed;

    public float speedChange;

    public float jumpForce;

    public float fallModifier;

    #endregion

    #region Input

    Vector2 moveVector;
    string moveVectorXName = "LSH";
    string moveVectorYName = "LSV";

    bool jumpButtonDown;
    bool jumpButtonUp;
    bool jumpButton;
    string jumpButtonName = "Jump";

    bool attackButtonDown;
    bool attackButtonUp;
    bool attackButton;
    string attackButtonName = "Attack";

    #endregion

    #region Checks

    int localPlayerNumber;
    int playerNumber;

    public LayerMask groundLayer;

    bool facingRight;

    public float groundDistance = 0.1f;

    #endregion

    #region Classes

    Rigidbody2D rBody;

<<<<<<< Updated upstream
    GravityController gravController;

    OrbitalRotation orbitController;

=======
>>>>>>> Stashed changes
    new Collider2D collider;

    #endregion

    #region Properties

    bool isGrounded
    {
        get
        {
            Debug.DrawLine(transform.position - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .95f)), transform.position - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .95f)) - (transform.up * groundDistance), Color.red, 0.05f);

            if(Physics2D.Linecast(transform.position - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .95f)), transform.position - transform.TransformDirection(new Vector3(0, collider.bounds.extents.y * .95f)) - (transform.up * groundDistance), groundLayer))
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

<<<<<<< Updated upstream
        gravController = GetComponent<GravityController>();

        orbitController = GetComponent<OrbitalRotation>();
=======
        PlayerManager.instance.localPlayers.Add(this);
        PlayerManager.instance.players.Add(this);

        playerNumber = PlayerManager.instance.players.Count;
        localPlayerNumber = PlayerManager.instance.localPlayers.Count;
>>>>>>> Stashed changes
    }

    // Use this for initialization
    void Start ()
    {
<<<<<<< Updated upstream
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
=======
	    
	}
>>>>>>> Stashed changes
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
        ApplyMovement();
        Jump();
	}

    void OnTriggerEnter2D(Collider2D col)
    {

    }

    #endregion

    void GetInput()
    {
        moveVector = new Vector2(Input.GetAxis(moveVectorXName + localPlayerNumber), Input.GetAxis(moveVectorYName + localPlayerNumber));

        jumpButtonDown = Input.GetButtonDown(jumpButtonName + localPlayerNumber);
        jumpButtonUp = Input.GetButtonUp(jumpButtonName + localPlayerNumber);
        jumpButton = Input.GetButton(jumpButtonName + localPlayerNumber);

        attackButtonDown = Input.GetButtonDown(attackButtonName + localPlayerNumber);
        attackButtonUp = Input.GetButtonUp(attackButtonName + localPlayerNumber);
        attackButton = Input.GetButton(attackButtonName + localPlayerNumber);
    }

    void ApplyMovement()
    {
        Vector2 targetVelocity = new Vector2(moveVector.x, 0);

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
        if(isGrounded && jumpButtonDown)
        {
            rBody.velocity += new Vector2(transform.up.x, transform.up.y) * jumpForce;

            Debug.Log((Vector2)transform.TransformDirection(Vector2.up) * jumpForce);
        }
    }

    #endregion

}
