using UnityEngine;

public class SlimeController : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        anim.SetTrigger("Die");

        Destroy(this.gameObject, 3f);
    }
}
