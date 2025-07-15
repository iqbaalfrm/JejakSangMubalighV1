using UnityEngine;

public class SoalTrigger : MonoBehaviour
{
    public SoalManagerLv1 soalManager; // drag SoalManagerLv1 di Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            soalManager.TampilkanSoalRandom(null);  // tampilkan soal
            Time.timeScale = 0f;                     // pause game
            Destroy(gameObject);                     // hancurkan trigger
        }
    }
}
