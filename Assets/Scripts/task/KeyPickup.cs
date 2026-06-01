using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class KeyPickup : MonoBehaviour
{
    [SerializeField] private Transform doorMoving;
    [SerializeField] private Transform doorTarget;

    private bool collected;

    void Start()
    {
        if (doorMoving == null)
            doorMoving = GameObject.Find("开门_0（1）")?.transform;
        if (doorTarget == null)
            doorTarget = GameObject.Find("开门_0")?.transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (other.TryGetComponent<Movements>(out _))
        {
            collected = true;
            GetComponent<SpriteRenderer>().enabled = false;
            if (doorMoving != null && doorTarget != null)
                doorMoving.position = doorTarget.position;
            Destroy(gameObject);
        }

    }

    void OnGUI()
    {
        if (!collected) return;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 40, 300, 40), "已开门");
    }
}
