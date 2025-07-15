using UnityEngine;

public class SoalTrigger1 : MonoBehaviour
{
    public SoalManagerLv2 soalManagerLv2; // drag SoalManagerLv1 di Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            soalManagerLv2.TampilkanSoalRandom(null);  // tampilkan soal
            Time.timeScale = 0f;                     // pause game
            Destroy(gameObject);                     // hancurkan trigger
        }
    }
}
