using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FizzBuzzController : MonoBehaviour
{
    public TextMeshProUGUI RandomNumberText, UserInputText, FeedbackText, ScoreText, DivisibleBy3Text, DivisibleBy5Text;
    public Image panel;
    public AudioClip rightSoundClip;
    public AudioClip wrongSoundClip;
    private AudioSource audioSource;
    private int randomNumber, score;
    private string userInput = "";
    private bool canInput = true;
    private Coroutine changeColorCoroutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from this GameObject. Please add one.");
            return;
        }

        if (RandomNumberText == null || UserInputText == null || FeedbackText == null || ScoreText == null || DivisibleBy3Text == null || DivisibleBy5Text == null || panel == null)
        {
            Debug.LogError("One or more TextMeshProUGUI or Image components are not assigned. Please assign them in the inspector.");
            return;
        }

        GenerateNewRandomNumber();
        UpdateUI();
    }

    private void GenerateRandomNumber()
    {
        randomNumber = Random.Range(1, 1000);
    }

    private void UpdateUI()
    {
        RandomNumberText.text = randomNumber.ToString();
        DivisibleBy3Text.text = (randomNumber % 3 == 0) ? "Teilbar durch 3" : "";
        DivisibleBy5Text.text = (randomNumber % 5 == 0) ? "Teilbar durch 5" : "";
    }

    private void UpdateUserInputText(string input)
    {
        userInput = input;
        UserInputText.text = userInput;
    }

    private void CheckUserInput()
    {
        if (string.IsNullOrEmpty(userInput))
        {
            FeedbackText.text = "Bitte geben Sie eine Antwort ein.";
            return;
        }

        string correctAnswer = (randomNumber % 3 == 0 && randomNumber % 5 == 0) ? "FizzBuzz" :
                               (randomNumber % 3 == 0) ? "Fizz" :
                               (randomNumber % 5 == 0) ? "Buzz" : "Keine Zahl";

        bool isCorrect = userInput == correctAnswer || (userInput == "nicht teilbar" && correctAnswer == "Keine Zahl");

        if (isCorrect)
        {
            FeedbackText.text = "Richtig!";
            score++;
            UpdateScoreText();
            if (score >= 10)
            {
                SceneManager.LoadScene("EndScene");
                return;
            }
            if (changeColorCoroutine != null)
            {
                StopCoroutine(changeColorCoroutine);
            }
            changeColorCoroutine = StartCoroutine(ChangePanelColorAfterDelay(Color.green, rightSoundClip));
        }
        else
        {
            FeedbackText.text = "Falsch! Die richtige Antwort ist: " + correctAnswer;
            if (changeColorCoroutine != null)
            {
                StopCoroutine(changeColorCoroutine);
            }
            changeColorCoroutine = StartCoroutine(ChangePanelColorAfterDelay(Color.red, wrongSoundClip));
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null. Please assign the clip in the inspector.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is null. Please check the AudioSource component.");
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

    private IEnumerator ChangePanelColorAfterDelay(Color color, AudioClip clip)
    {
        yield return new WaitForSeconds(1f);
        panel.color = color;
        PlaySound(clip);
    }

    private void UpdateScoreText()
    {
        ScoreText.text = "Punktzahl: " + score;
    }

    private void GenerateNewRandomNumber()
    {
        GenerateRandomNumber();
        UpdateUI();
        userInput = "";
        UserInputText.text = "";
        FeedbackText.text = "";
        canInput = true;
    }

    private void Update()
    {
        if (canInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                UpdateUserInputText("Fizz");
                CheckUserInput();
                canInput = false;
                StartCoroutine(GenerateNewNumberAfterDelay());
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                UpdateUserInputText("Buzz");
                CheckUserInput();
                canInput = false;
                StartCoroutine(GenerateNewNumberAfterDelay());
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UpdateUserInputText("FizzBuzz");
                CheckUserInput();
                canInput = false;
                StartCoroutine(GenerateNewNumberAfterDelay());
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                UpdateUserInputText("nicht teilbar");
                CheckUserInput();
                canInput = false;
                StartCoroutine(GenerateNewNumberAfterDelay());
            }
        }
    }

    private IEnumerator GenerateNewNumberAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        GenerateNewRandomNumber();
        panel.color = Color.grey;
    }
}