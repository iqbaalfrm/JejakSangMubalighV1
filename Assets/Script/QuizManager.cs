using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public GameObject panelSoal;

    void Start()
    {
        panelSoal.SetActive(false);
    }

    public void JawabanBenar()
    {
        Debug.Log("Jawaban benar!");
        panelSoal.SetActive(false);
        Time.timeScale = 1f;
    }

    public void JawabanSalah()
    {
        Debug.Log("Jawaban salah!");
    }

    public void TampilkanSoal()
    {
        panelSoal.SetActive(true);
        Time.timeScale = 0.0001f;
    }
}