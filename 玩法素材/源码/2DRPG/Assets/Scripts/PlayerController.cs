using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private int directionNumber = 2;
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb;
    public float moveSpeed = 1f;
    public ContactFilter2D contactFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float collisionOffset = 0.02f;

    bool canMove = true;
    private PlayerAttack playerAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAttack = GetComponentInChildren<PlayerAttack>();
    }

    private void FixedUpdate()
    {
        if(direction != Vector2.zero)
        {
            bool isSuccess = TryMove(direction);
            if (!isSuccess)
            {
                if(TryMove(new Vector2(direction.x, 0)) == false)
                {
                    TryMove(new Vector2(0, direction.y));
                }
                
            }
        }
        AnimationControll();
    }

    bool TryMove(Vector2 dirTemp)
    {
        if (canMove == false) return false;
        if (dirTemp == Vector2.zero) return false;

        int count = rb.Cast(dirTemp, contactFilter, castCollisions, moveSpeed * Time.fixedDeltaTime+collisionOffset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + dirTemp * Time.fixedDeltaTime * moveSpeed);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue inputValue)
    {
        direction = inputValue.Get<Vector2>();
        if (direction.x != 0)
        {
            directionNumber = 1;
        }
        else if (direction.y > 0)
        {
            directionNumber = 0;
        }
        else if (direction.y < 0)
        {
            directionNumber = 2;
        }
    }
    void AnimationControll()
    {
        if (direction != Vector2.zero)
        {
            anim.SetBool("IsMoving", true);
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        anim.SetInteger("Direction", directionNumber);
    }

    void OnAttack()
    {
        anim.SetTrigger("Attack");
    }
    void OnAttackStart()
    {
        canMove = false;
        if (spriteRenderer.flipX)
        {
            playerAttack.AttackLeft();
        }
        else
        {
            playerAttack.AttackRight();
        }
    }
    void OnAttackEnd()
    {
        canMove = true;
        playerAttack.StopAttack();
    }
}
