using UnityEngine;

public class SoalTrigger2 : MonoBehaviour
{
    public SoalManagerLv3 soalManagerLv3; // drag SoalManagerLv1 di Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            soalManagerLv3.TampilkanSoalRandom(null);  // tampilkan soal
            Time.timeScale = 0f;                     // pause game
            Destroy(gameObject);                     // hancurkan trigger
        }
    }
}
