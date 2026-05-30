using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movements : MonoBehaviour
{
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private ContactFilter2D contactFilter;

    private Rigidbody2D rb;
    private readonly List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        Vector2 moveDirection = Vector2.zero;
        float flipDirection = 0f;

        if (kb.wKey.wasPressedThisFrame)
            moveDirection = Vector2.up;
        else if (kb.sKey.wasPressedThisFrame)
            moveDirection = Vector2.down;
        else if (kb.aKey.wasPressedThisFrame)
        {
            moveDirection = Vector2.left;
            flipDirection = -1f;
        }
        else if (kb.dKey.wasPressedThisFrame)
        {
            moveDirection = Vector2.right;
            flipDirection = 1f;
        }

        if (moveDirection == Vector2.zero) return;

        castCollisions.Clear();
        if (rb != null)
        {
            int count = rb.Cast(moveDirection, contactFilter, castCollisions, gridSize);
            if (count > 0) return;
            rb.MovePosition(rb.position + moveDirection * gridSize);
        }
        else
        {
            transform.position += (Vector3)(moveDirection * gridSize);
        }

        if (flipDirection != 0f)
            Flip(flipDirection);
    }

    void Flip(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}
