using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnLetsDieButtonClick()
    {
        SceneManager.LoadScene(1);
    }
}
