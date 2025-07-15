using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ImageSlideManager : MonoBehaviour
{
    public Sprite[] slideImages;
    public Image displayImage;
    public Button nextButton;
    public Button prevButton;

    public GameObject slidePanel; // referensi panel utama untuk ditutup

    private int index = 0;

    void Start()
    {
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        ShowSlide();
    }

    public void Next()
    {
        if (index < slideImages.Length - 1)
        {
            index++;
            ShowSlide();
        }
    }

    public void Prev()
    {
        if (index > 0)
        {
            index--;
            ShowSlide();
        }
        else
        {
            CloseSlide(); // kalau di slide pertama, tombol "Kembali"
        }
    }

    void ShowSlide()
    {
        displayImage.sprite = slideImages[index];

        nextButton.gameObject.SetActive(true);
        prevButton.gameObject.SetActive(true);

        // NEXT → berubah jadi PLAY di slide terakhir
        if (index == slideImages.Length - 1)
        {
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(GoToLevelSatu);
        }
        else
        {
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(Next);
        }

        // PREV → berubah jadi Kembali di slide pertama
        if (index == 0)
        {
            prevButton.GetComponentInChildren<TextMeshProUGUI>().text = "Kembali";
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(CloseSlide);
        }
        else
        {
            prevButton.GetComponentInChildren<TextMeshProUGUI>().text = "Previous";
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(Prev);
        }
    }

    void GoToLevelSatu()
    {
        SceneManager.LoadScene("LevelSatu");
    }

    void CloseSlide()
    {
        if (slidePanel != null)
        {
            slidePanel.SetActive(false);
        }
    }
}
