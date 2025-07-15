using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class JSONQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string correctAnswer;
    }

    [System.Serializable]
    public class QuestionList
    {
        public List<Question> questions;
    }

    public TextMeshProUGUI questionTextUI;
    public TMP_InputField answerInput;
    public Button submitButton;
    public TextMeshProUGUI feedbackText;

    private QuestionList questionList;
    private int currentIndex = 0;

    void Start()
    {
        LoadQuestions();
        submitButton.onClick.AddListener(CheckAnswer);
        ShowQuestion();
    }

    void LoadQuestions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "questions.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            string wrappedJson = "{\"questions\":" + json + "}";
            questionList = JsonUtility.FromJson<QuestionList>(wrappedJson);
        }
        else
        {
            Debug.LogError("File questions.json tidak ditemukan!");
        }
    }

    void ShowQuestion()
    {
        if (currentIndex < questionList.questions.Count)
        {
            questionTextUI.text = questionList.questions[currentIndex].questionText;
            answerInput.text = "";
            feedbackText.text = "";
        }
        else
        {
            questionTextUI.text = "Kuis selesai!";
            answerInput.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
        }
    }

    void CheckAnswer()
    {
        string input = answerInput.text.ToLower().Trim().Replace(" ", "");
        string correct = questionList.questions[currentIndex].correctAnswer.ToLower().Trim().Replace(" ", "");

        if (input == correct)
        {
            feedbackText.text = "Benar! ✅";
            feedbackText.color = Color.green;
            currentIndex++;
            Invoke(nameof(ShowQuestion), 1.5f);
        }
        else
        {
            feedbackText.text = "Salah, coba lagi.";
            feedbackText.color = Color.red;
        }
    }
}
