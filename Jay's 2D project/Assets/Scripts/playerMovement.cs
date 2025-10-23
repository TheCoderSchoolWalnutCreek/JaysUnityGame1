using UnityEngine;
//Move This to a Basic Player Script
[RequireComponent(typeof(Rigidbody2D))]
public class playerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;

    [Space(5)]
    [Header("Jump Variables")]
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private Transform feet, leftFoot, rightFoot;
    [SerializeField] private float feetScale = .5f;
    [SerializeField] private LayerMask groundLayer;
    [Range(0, 3)]
    [SerializeField] private int jumpAmount = 1;
    //Move This to a Basic Player Script
    [SerializeField] private float padForce = 20;
    private int jumpsLeft;
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = .5f;
    [SerializeField] private float jumpBuffer = .25f;
    [Header("Gravity")]
    [SerializeField] private float gravityMultiplier = 2.5f;
    private float lastGroundTime;
    private float lastJumpPress;

    private Rigidbody2D rb;
    private float moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      jumpsLeft = jumpAmount; 
    }

    // Update is called once per frame
    void Update()
    {
        //Move Logic
        MoveLogic();

        //jump logic
        JumpLogic();

        //SafteyNet
        if (transform.position.y < -6)
        {
            transform.position = new Vector2(0, 5);
        }

        //Gravity
        rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
    }
    //Move This to a Basic Player Script
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("jumpPad"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up*padForce, ForceMode2D.Impulse);
        }
    }
    #region Move Logic
    private void MoveLogic()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }
    #endregion

    #region Jump Logic
    bool GroundLogic()
    {
        if (Physics2D.OverlapCircle(feet.position, feetScale, groundLayer))
            return true;
        if (Physics2D.OverlapCircle(leftFoot.position, feetScale, groundLayer))
            return true;
        if (Physics2D.OverlapCircle(rightFoot.position, feetScale, groundLayer))
            return true;
        return false;
    }
    private void JumpLogic()
    {
        if (GroundLogic())
        {
            /*
            if (lastGroundTime <= 0)//TO DO: replace variable
            {
                jumpsLeft = jumpAmount;
            }
            */
            lastGroundTime = coyoteTime;
        }
        else
        {
            lastGroundTime -= Time.deltaTime;
            if (lastGroundTime <= 0)
            {
                jumpsLeft = 0;
            }
        }
        
        if (lastJumpPress > 0)
            lastJumpPress -= Time.deltaTime;

        if (JumpInput())
        {
            lastJumpPress = jumpBuffer;
            if (jumpsLeft > 0)
            {
                Jump(); 
            }
        }
        else if(lastJumpPress > 0 && GroundLogic())
        {
            Jump();
        }
    }
    bool JumpInput()
    {
        if (Input.GetKeyDown("space") || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            return true;
        }
        return false;
    }
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        jumpsLeft--;
    }
    #endregion
}