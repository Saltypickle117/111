using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class DoorTransition : MonoBehaviour
{
    [SerializeField] private string sceneName = "plot1";

    private bool triggered;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.TryGetComponent<Movements>(out _))
        {
            triggered = true;
            SceneManager.LoadScene(sceneName);
        }
    }
}
