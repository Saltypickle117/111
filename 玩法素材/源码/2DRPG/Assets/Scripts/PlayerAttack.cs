using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private BoxCollider2D boxCollider2D;

    private float xPosition;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        xPosition = transform.position.x;
    }

    public void AttackRight()
    {
        transform.localPosition = new Vector3(xPosition, 0, 0);
        boxCollider2D.enabled = true;
    }
    public void AttackLeft()
    {
        transform.localPosition = new Vector3(-xPosition, 0, 0);
        boxCollider2D.enabled = true;
    }
    public void StopAttack()
    {
        boxCollider2D.enabled = false;
    }

}
