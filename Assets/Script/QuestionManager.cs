using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public GameObject popupPanel;
    public Text questionText;
    public Button[] answerButtons;

    void Start()
    {
        // Set soal dan jawaban
        questionText.text = "Siapa pendiri Kerajaan Samudra Pasai?";
        string[] options = { "Sultan Malik al Saleh", "Gajah Mada", "Hayam Wuruk" };
        int correctIndex = 0;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int idx = i;
            answerButtons[i].GetComponentInChildren<Text>().text = options[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => Answer(idx == correctIndex));
        }
    }

    public void Answer(bool isCorrect)
    {
        if (isCorrect)
        {
            popupPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            Debug.Log("Salah! Coba lagi.");
        }
    }

    void Update()
    {
        // Kosongkan atau hapus jika tidak dipakai
    }
}
