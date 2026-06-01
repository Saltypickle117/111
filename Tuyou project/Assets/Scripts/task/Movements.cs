using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movements : MonoBehaviour
{
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private ContactFilter2D contactFilter;
    [SerializeField] private Animator animator;
    [SerializeField] private float attackAnimDuration = 0.3f;
    [SerializeField] private float moveInterval = 0.15f;
    [SerializeField] private float initialMoveDelay = 0.3f;
    [SerializeField] private int maxSteps = 11;

    private Rigidbody2D rb;
    private readonly List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private float moveTimer;
    private bool isMoving;
    private int stepsTaken;

    private static readonly int AttackHash = Animator.StringToHash("player attack");
    private static readonly int HoldingHash = Animator.StringToHash("player_holding state");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
        contactFilter.useTriggers = true;
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        Vector2 moveDirection = Vector2.zero;
        float flipDirection = 0f;

        if (kb.wKey.isPressed)
            moveDirection = Vector2.up;
        else if (kb.sKey.isPressed)
            moveDirection = Vector2.down;
        else if (kb.aKey.isPressed)
        {
            moveDirection = Vector2.left;
            flipDirection = -1f;
        }
        else if (kb.dKey.isPressed)
        {
            moveDirection = Vector2.right;
            flipDirection = 1f;
        }

        if (moveDirection == Vector2.zero || stepsTaken >= maxSteps)
        {
            isMoving = false;
            moveTimer = 0f;
            return;
        }

        bool justPressed = kb.wKey.wasPressedThisFrame || kb.sKey.wasPressedThisFrame
            || kb.aKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame;

        if (justPressed)
        {
            isMoving = true;
            moveTimer = initialMoveDelay;
            TryMove(moveDirection, flipDirection);
        }
        else if (isMoving)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0f)
            {
                moveTimer = moveInterval;
                TryMove(moveDirection, flipDirection);
            }
        }
    }

    void TryMove(Vector2 moveDirection, float flipDirection)
    {
        castCollisions.Clear();
        if (rb != null)
        {
            int count = rb.Cast(moveDirection, contactFilter, castCollisions, gridSize);
            if (count > 0)
            {
                Collider2D hit = null;
                for (int i = 0; i < count; i++)
                {
                    if (!castCollisions[i].collider.isTrigger)
                    {
                        hit = castCollisions[i].collider;
                        break;
                    }
                }
                if (hit != null)
                {
                    Box box = hit.GetComponent<Box>();
                    if (box != null)
                        StartCoroutine(PushBox(box, moveDirection, flipDirection));
                    return;
                }
            }

            rb.MovePosition(rb.position + moveDirection * gridSize);
            stepsTaken++;
        }
        else
        {
            transform.position += (Vector3)(moveDirection * gridSize);
            stepsTaken++;
        }

        if (flipDirection != 0f)
            Flip(flipDirection);
    }

    IEnumerator PushBox(Box box, Vector2 moveDirection, float flipDirection)
    {
        Rigidbody2D boxRb = box.GetComponent<Rigidbody2D>();
        if (boxRb != null)
        {
            castCollisions.Clear();
            int boxHitCount = boxRb.Cast(moveDirection, contactFilter, castCollisions, gridSize);
            for (int i = 0; i < boxHitCount; i++)
            {
                if (castCollisions[i].collider.GetComponent<Box>() == null)
                {
                    yield break;
                }
            }
        }

        if (animator != null)
            animator.Play(AttackHash);

        if (boxRb != null)
            boxRb.MovePosition(boxRb.position + moveDirection * gridSize);

        stepsTaken++;

        if (flipDirection != 0f)
            Flip(flipDirection);

        yield return new WaitForSeconds(attackAnimDuration);

        if (animator != null)
            animator.Play(HoldingHash);
    }

    public void ConsumeExtraStep()
    {
        stepsTaken++;
    }

    void Flip(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    void OnGUI()
    {
        if (SceneManager.GetActiveScene().name != "task1") return;
        int remaining = Mathf.Max(0, maxSteps - stepsTaken);
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 300, 40), "剩余步数：" + remaining, style);
    }
}
