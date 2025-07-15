using UnityEngine;

public class Papanlvl1 : MonoBehaviour
{
    public SoalManagerLv1 soalManagerLv1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎯 Player menyentuh papan soal.");
            
            // Munculkan soal lewat SoalManager
            soalManagerLv1.TampilkanSoalRandom(null);

            // Nonaktifkan trigger supaya gak dipanggil lagi
            gameObject.SetActive(false);
        }
    }
}
