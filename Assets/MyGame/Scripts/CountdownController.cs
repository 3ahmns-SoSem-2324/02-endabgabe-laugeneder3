using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CountdownController : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    private float countdownTime = 20f; 

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString("F0");
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        
        SceneManager.LoadScene("FizzBuzzMainSzene");
    }
}