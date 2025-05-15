using UnityEngine;

public class SignTrigger : MonoBehaviour
{
    public GameObject questionPopup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player menyentuh trigger!");

            questionPopup.SetActive(true);
            Time.timeScale = 0.0001f; // pause tapi UI masih bisa di-click
        }
    }
}
