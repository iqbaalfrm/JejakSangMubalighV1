using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class MultipleChoiceQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] choices;
        public string correctAnswer;
    }

    [System.Serializable]
    public class QuestionList
    {
        public List<Question> questions;
    }

    public GameObject panelSoal;
    public TextMeshProUGUI textSoal;
    public Button buttonA, buttonB, buttonC;
    public TextMeshProUGUI textA, textB, textC;
    public TextMeshProUGUI textFeedback;

    private QuestionList questionList;
    private Question currentQuestion;

    private int totalScore = 0;
    private int questionsAnswered = 0;
    public int maxQuestions = 5;

    void Start()
    {
        LoadQuestions();
        panelSoal.SetActive(false);
    }

    void LoadQuestions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "questions.json");
        Debug.Log("📥 Loading questions from: " + path);

        if (!File.Exists(path))
        {
            Debug.LogError("🚫 File questions.json tidak ditemukan di StreamingAssets!");
            return;
        }

        string json = File.ReadAllText(path);
        Debug.Log("✅ JSON loaded:\n" + json);

        string wrappedJson = "{\"questions\":" + json + "}";

        questionList = JsonUtility.FromJson<QuestionList>(wrappedJson);

        if (questionList == null || questionList.questions == null)
        {
            Debug.LogError("🚫 Gagal parsing JSON jadi QuestionList!");
        }
        else
        {
            Debug.Log("✅ Jumlah soal terbaca: " + questionList.questions.Count);
        }
    }

    public void ShowRandomQuestion()
    {
        if (questionList == null || questionList.questions.Count == 0)
        {
            Debug.LogError("🚫 Tidak ada soal yang bisa ditampilkan!");
            return;
        }

        Time.timeScale = 0f;
        panelSoal.SetActive(true);
        textFeedback.text = "";

        int randomIndex = Random.Range(0, questionList.questions.Count);
        currentQuestion = questionList.questions[randomIndex];

        Debug.Log("🧠 Soal: " + currentQuestion.questionText);
        Debug.Log("🅰️ " + currentQuestion.choices[0]);
        Debug.Log("🅱️ " + currentQuestion.choices[1]);
        Debug.Log("🇨 " + currentQuestion.choices[2]);
        Debug.Log("✔️ Jawaban benar: " + currentQuestion.correctAnswer);

        textSoal.text = currentQuestion.questionText;

        textA.text = currentQuestion.choices[0];
        textB.text = currentQuestion.choices[1];
        textC.text = currentQuestion.choices[2];

        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();

        buttonA.onClick.AddListener(() => Answer(currentQuestion.choices[0]));
        buttonB.onClick.AddListener(() => Answer(currentQuestion.choices[1]));
        buttonC.onClick.AddListener(() => Answer(currentQuestion.choices[2]));
    }

    void Answer(string selected)
    {
        questionsAnswered++;

        if (selected.Trim().ToLower() == currentQuestion.correctAnswer.Trim().ToLower())
        {
            totalScore += 20;
            textFeedback.text = "Benar! ✅";
            textFeedback.color = Color.green;
            Debug.Log("✅ Jawaban benar! Skor saat ini: " + totalScore);
        }
        else
        {
            textFeedback.text = "Salah ❌";
            textFeedback.color = Color.red;
            Debug.Log("❌ Jawaban salah.");
        }

        Invoke(nameof(NextStep), 1.5f);
    }

    void NextStep()
    {
        panelSoal.SetActive(false);
        Time.timeScale = 1f;

        if (questionsAnswered >= maxQuestions)
        {
            if (totalScore >= 80)
            {
                Debug.Log("🎉 Level selesai! Skor kamu: " + totalScore);
                // TODO: Load next scene
            }
            else
            {
                Debug.Log("🔁 Ulangi level. Skor kamu: " + totalScore);
                totalScore = 0;
                questionsAnswered = 0;
                // TODO: Reset scene
            }
        }
    }
}
