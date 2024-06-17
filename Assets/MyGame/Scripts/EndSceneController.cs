using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EndSceneController : MonoBehaviour
{
    public TextMeshProUGUI restartButtonText;

    private void Start()
    {
        if (restartButtonText == null)
        {
            Debug.LogError("restartButtonText is not assigned. Please assign it in the inspector.");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("FizzBuzzMainSzene");
    }
}