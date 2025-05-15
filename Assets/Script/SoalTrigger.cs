using UnityEngine;

public class SoalTrigger : MonoBehaviour
{
    public GameObject panelSoal; // drag panel soal ke sini

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // pastikan player pakai tag "Player"
        {
            panelSoal.SetActive(true);   // munculkan panel soal
            Time.timeScale = 0f;         // pause game
            Destroy(gameObject);         // hilangkan trigger biar gak muncul dua kali
        }
    }
}
