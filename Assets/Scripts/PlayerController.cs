using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool melee;
    public bool shoot;
    public float climbSpeed;
    public float jumpForce;
    private float moveInput;
    private float climbInput;
    public float aimSpeed;
    private float timeBetweenShots;
    public float startTimeBetweenShots;
    public Slider slider;
    public float chargeSpeed;
    public Slider healtbar;

    public Destructable destructible;
    private Rigidbody2D rigidbody;
    private CircleCollider2D collider;
    private bool facingRight = true;
    private bool isClimbing;
    private float aimInput;

    public Transform attackPoint;
    public Transform aimPoint;
    public float meleeRange;
    public int damage;
    public LayerMask enemyLayer;
    public GameObject projectile;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public float distance;
    public LayerMask whatIsGround;
    public LayerMask passableGround;
    public LayerMask whatIsLadder;

    private int extraJumps;
    public int extraJumpValue;

    public int playerNumber;
    GameMaster gameMaster;
    public bool selected;
    public bool hasMoved;
    public GameObject playerCamera;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        extraJumps = extraJumpValue;
        gameMaster = FindObjectOfType<GameMaster>();
        healtbar.maxValue = GetComponentInChildren<Destructable>().health;
    }

    private void OnMouseDown()
    {
        if(gameMaster.playerTurn == playerNumber && hasMoved == false && gameMaster.playerSelected == false)
            {
                if (gameMaster.selectedUnit != null)
                {
                    gameMaster.selectedUnit.selected = false;
                }
                selected = true;
                hasMoved = true;
                gameMaster.selectedUnit = this;
                playerCamera.active = !playerCamera.active;
                gameMaster.mainCamera.active = !gameMaster.mainCamera.active;
                gameMaster.playerSelected = true;
            }
        
    }

    private void FixedUpdate()
    {
        if(playerNumber != gameMaster.playerTurn || gameMaster.timeLeft <= 0 || selected == false)
        {
            return;
        }
        Move();
        Climb();
        Drop();
    }

    private void Update()
    {
        
        if (playerNumber != gameMaster.playerTurn || gameMaster.timeLeft <= 0 || selected == false)
        {
            Health();
            return;
        }
        Jump();
        Health();
        Aim();
        Shoot();
        Melee();
    }

    void Move()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(moveInput * speed, rigidbody.velocity.y);

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    void Jump()
    {
        if (isGrounded == true)
        {
            extraJumps = extraJumpValue;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rigidbody.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)
        {
            rigidbody.velocity = Vector2.up * jumpForce;
        }
    }

    void Shoot()
    {
        if (shoot && timeBetweenShots <= 0)
        {
                if (Input.GetKey(KeyCode.X))
                {
                    slider.value += Time.deltaTime * chargeSpeed;
                }

                if (Input.GetKeyUp(KeyCode.X))
                {
                    var ball = Instantiate(projectile, attackPoint.position, attackPoint.rotation);
                    ball.GetComponent<Projectile>().speed = slider.value;
                    timeBetweenShots = startTimeBetweenShots;
                }         
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
            slider.value = 0;
        }

    }

    void Melee()
    {
        if(melee && timeBetweenShots <= 0)
        {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, meleeRange, enemyLayer);
                    foreach (Collider2D enemy in hitEnemies)
                    {
                        enemy.GetComponent<Destructable>().health -= damage;
                        timeBetweenShots = startTimeBetweenShots;
                    }
            }       
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void Climb()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);

        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isClimbing = true;
            }
        }
        else
        {
            isClimbing = false;
        }
        if (isClimbing == true)
        {
            climbInput = Input.GetAxis("Vertical");
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, climbInput * climbSpeed);
            rigidbody.gravityScale = 0;
        }
        else
        {
            rigidbody.gravityScale = 5;
        }
    }

    void Drop()
    {
        RaycastHit2D downInfo = Physics2D.Raycast(transform.position, Vector2.down, distance, passableGround);

        if (downInfo.collider != null && isGrounded == true)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                collider.enabled = false;
            }
        }
        else
        {
            collider.enabled = true;
        }
    }

    void Aim()
    {
            aimInput = Input.GetAxis("Vertical");

            aimPoint.Rotate(0.0f, 0.0f, aimInput * aimSpeed);
    }

    void Health()
    {
        healtbar.value = destructible.health;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
    }
}
