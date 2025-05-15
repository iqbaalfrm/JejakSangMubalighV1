using UnityEngine;

public class ShowPopupOnClick : MonoBehaviour
{
    public GameObject popupImage;

    // Fungsi untuk menampilkan popup
    public void ShowPopup()
    {
        if (popupImage != null)
            popupImage.SetActive(true);
    }

    // Fungsi untuk menyembunyikan popup
    public void ClosePopup()
    {
        if (popupImage != null)
            popupImage.SetActive(false);
        else
            Debug.LogWarning("popupImage belum di-assign di Inspector!");
    }
}
