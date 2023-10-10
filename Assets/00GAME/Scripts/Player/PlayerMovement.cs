using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //how much time the player can hang in the air before jumping
    private float coyoteCounter; //how much time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;


    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D _rigi;
    private Animator _anim;
    private BoxCollider2D _boxCollider;
    private float wallJumpCooldown;
    private float _horizontalInput;

    public static int numberOfCoins;
    [SerializeField] public Text coinsText;

    [Header("Character")]
    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;

    private int selectedOption = 0;

    private void Awake()
    {
        //references for rigidbody and animator
        _rigi = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        //numberOfCoins = PlayerPrefs.GetInt("NumberOfCoins", 0);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }
        UpdateCharacter(selectedOption);
    }
    void Update()
    {
        coinsText.text = "" + numberOfCoins.ToString();
        _horizontalInput = Input.GetAxis("Horizontal");
        _rigi.velocity = new Vector2(_horizontalInput * _speed, _rigi.velocity.y);

        //flip player when moving left-right
        if(_horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if(_horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //set animator parameters
        _anim.SetBool("run", _horizontalInput != 0);
        _anim.SetBool("grounded", isGrounded());

        //jump
        if (Input.GetKeyDown(KeyCode.Space))     
            Jump();


        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && _rigi.velocity.y > 0)
            _rigi.velocity = new Vector2(_rigi.velocity.x, _rigi.velocity.y / 2);
        if (onWall())
        {
            _rigi.gravityScale = 0;
            _rigi.velocity = Vector2.zero;
        }
        else
        {
            _rigi.gravityScale = 7;
            _rigi.velocity = new Vector2(_horizontalInput * _speed, _rigi.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; // reset coyote counter when on the ground
                jumpCounter = extraJumps;
            }
            else
                coyoteCounter -= Time.deltaTime; //start decreasing coyote counter when not on the ground
        }
        
    }

    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

    private void Jump()
    {
        if (coyoteCounter < 0 && !onWall() && jumpCounter <= 0) return;
        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                _rigi.velocity = new Vector2(_rigi.velocity.x, _jumpPower);
            else
            {
                //if not on the ground and coyoe counter bigger than 0 do a normal jump
                if (coyoteCounter > 0)
                    _rigi.velocity = new Vector2(_rigi.velocity.x, _jumpPower);
                else
                {
                    if(jumpCounter > 0)
                    {
                        _rigi.velocity = new Vector2(_rigi.velocity.x, _jumpPower);
                        _jumpPower--;
                    }    
                }
            }
            //reset coyote counter to 0 to avoid double jump
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        _rigi.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall() 
    { 
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return _horizontalInput == 0 && isGrounded() && !onWall();
    }
}
