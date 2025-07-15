using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTrigger : MonoBehaviour
{
    public SoalManagerLv1 soalManagerLv1;// drag SoalManager disini
    public string nextLevelName;     // isi nama scene berikutnya di Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🚩 Finish Trigger Tersentuh! Skor: " + soalManagerLv1.GetSkor());

            if (soalManagerLv1.GetSkor() >= 80)
            {
                Debug.Log("✅ Skor cukup! Pindah ke level: " + nextLevelName);
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                Debug.Log("❌ Skor kurang, ulang level.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
