using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour {
    [HideInInspector] public bool control; // Variable for pauses/cutscenes (turn on/off) movement

    [Header("Movement")]
    public float speed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    [Tooltip("How much player speed is reduced in the air")] public float airSpeedMod = 0.5f;
    [Tooltip("How much player acceleration is reduced in the air")] public float airAccelerationMod = 0.5f;
    public float maxFallingSpeed = 15;
    public float knockbackPower = 10;
    private float direction = 1f;
    private float velocity;


    [Header("Jump")]
    public float jumpVelocity = 5;
    public float jumpBufferTime = 0.1f;
    private float lastGroundedTime = float.NegativeInfinity;
    private bool isGrounded = false;
    private float coyoteTime = 0.07f;


    [Tooltip("Origin for Raycast to check ground")]
    public Transform groundOrigin1;
    public Transform groundOrigin2;


    [Header("Components")]
    public Rigidbody2D rb;
    public InputHandler input;
    public Animator animator;

    [Header("Sounds & Effects")]
    public GameObject jumpSound;
    public GameObject landSound;

    [Header("Camera")]
    public GameObject cameraPrefab;
    private CameraFollow myCameraFollow;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputHandler>();
        control = true;
    }

    public override void OnNetworkSpawn() {
        enabled = IsOwner;
    }

    private void FixedUpdate() {
        if (!control) {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        GroundCheck();
        Move();
        Jump();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallingSpeed));
        
    }

    private void Move() {
        velocity = rb.linearVelocity.x;
        if (input.movement.x != 0) {
            direction = Mathf.Sign(input.movement.x);
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
            if (velocity * direction < speed * (isGrounded ? 1 : airSpeedMod)) {
                velocity = Mathf.MoveTowards(velocity, speed * direction * (isGrounded ? 1 : airSpeedMod),
                                            acceleration * Time.fixedDeltaTime * (isGrounded ? 1 : airAccelerationMod));
            }
            animator.SetBool("move", true);
        }
        else {
            velocity = Mathf.MoveTowards(velocity, 0, deceleration * Time.fixedDeltaTime);
            animator.SetBool("move", false);
        }
        rb.linearVelocity = new Vector2(velocity, rb.linearVelocity.y);
    }


    private void Jump() {
        if (isGrounded && (Time.fixedTime - input.jumpPressTime) < jumpBufferTime) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
            input.jumpStartTime = Time.fixedTime;
            lastGroundedTime = float.NegativeInfinity;
            isGrounded = false;
            Destroy(Instantiate(jumpSound), 1f);
        }
    }

    private void GroundCheck() {
        RaycastHit2D hit1 = Physics2D.Raycast(groundOrigin1.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        RaycastHit2D hit2 = Physics2D.Raycast(groundOrigin2.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit1 || hit2) lastGroundedTime = Time.fixedTime;
        isGrounded = (Time.fixedTime - lastGroundedTime) < coyoteTime;
    }

    public void Knockback() {
        direction = Mathf.Sign(input.movement.x);
        rb.linearVelocity = new Vector2(knockbackPower * -direction, jumpVelocity * 0.5f);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.GetMask("Ground")) {
            Destroy(Instantiate(landSound), 1f);
        }
    }
}
