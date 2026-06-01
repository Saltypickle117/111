using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RotatingHole : MonoBehaviour
{
    private bool consumed;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (consumed) return;
        Movements player = other.GetComponent<Movements>();
        if (player != null)
        {
            player.ConsumeExtraStep();
            consumed = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Movements>() != null)
            consumed = false;
    }
}
