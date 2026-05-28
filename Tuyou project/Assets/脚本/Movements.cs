using UnityEngine;
using UnityEngine.InputSystem;

public class Movements : MonoBehaviour
{
    public float gridSize = 1f;

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        Vector3 move = Vector3.zero;

        if (kb.wKey.wasPressedThisFrame)
            move = new Vector3(0f, gridSize, 0f);
        else if (kb.sKey.wasPressedThisFrame)
            move = new Vector3(0f, -gridSize, 0f);
        else if (kb.aKey.wasPressedThisFrame)
        {
            move = new Vector3(-gridSize, 0f, 0f);
            Flip(-1f);
        }
        else if (kb.dKey.wasPressedThisFrame)
        {
            move = new Vector3(gridSize, 0f, 0f);
            Flip(1f);
        }

        transform.position += move;
    }

    void Flip(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}
