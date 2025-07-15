using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MateriData
{
    public string image;
    public string text;
}

public class SlideMateri : MonoBehaviour
{
    public Image materiImage;
    public TextMeshProUGUI materiText;
    public Button nextButton, prevButton;

    private List<MateriData> slides;
    private int index = 0;

    void Start()
    {
        string json = Resources.Load<TextAsset>("MateriData").text;
        slides = new List<MateriData>(JsonHelper.FromJson<MateriData>(json));
        ShowSlide();
    }

    public void Next() { if (index < slides.Count - 1) { index++; ShowSlide(); } }
    public void Prev() { if (index > 0) { index--; ShowSlide(); } }

    void ShowSlide()
    {
        var data = slides[index];
        materiImage.sprite = Resources.Load<Sprite>(data.image);
        materiText.text = data.text;

        prevButton.interactable = index > 0;
        nextButton.interactable = index < slides.Count - 1;
    }
}
