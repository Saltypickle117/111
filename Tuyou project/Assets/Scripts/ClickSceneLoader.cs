using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class ClickSceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName = "task1";

    void OnMouseDown()
    {
        SceneManager.LoadScene(sceneName);
    }
}
