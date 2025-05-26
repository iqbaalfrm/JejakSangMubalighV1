using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    // Drag panel GameObject (yang berisi tombol Tutup dan Mulai) ke sini lewat Inspector
    public GameObject panelSol;

    // Dipanggil saat tombol "Mulai" diklik
    public void OnClickMulai()
    {
        // Tutup panel terlebih dahulu
        if (panelSol != null)
        {
            panelSol.SetActive(false);
        }

        // Load scene bernama "levelsatu"
        SceneManager.LoadScene("levelsatu");
    }

    // Dipanggil saat tombol "Tutup" diklik
    public void OnClickTutup()
    {
        if (panelSol != null)
        {
            panelSol.SetActive(false);
        }
    }
}
