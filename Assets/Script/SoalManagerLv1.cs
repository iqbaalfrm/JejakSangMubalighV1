using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SoalManagerLv1 : MonoBehaviour
{
    [System.Serializable]
    public class Soal
    {
        public string pertanyaan;
        public string[] pilihan;
        public int indexJawabanBenar;
    }

    public TMP_Text teksSoal;
    public TMP_Text teksSkor;
    public TMP_Text[] teksPilihan;
    public Button[] tombolPilihan;
    public TMP_Text feedbackText;
    public GameObject panelSoal;

    private List<Soal> daftarSoal = new List<Soal>();
    private Soal soalAktif;
    private System.Action onSelesaiCallback;

    private int skor = 0;
    private int soalDijawab = 0;
    private const int soalTotal = 5;

    void Start()
    {
        panelSoal.SetActive(false);
        InisialisasiSoal();
    }

    void InisialisasiSoal()
    {
        skor = 0;
        soalDijawab = 0;

        daftarSoal = new List<Soal>
        {
            new Soal { pertanyaan = "Siapa pendiri Kerajaan Samudra Pasai?", pilihan = new string[] { "Malik Al Saleh", "Gajah Mada", "Hayam Wuruk" }, indexJawabanBenar = 0 },
            new Soal { pertanyaan = "Apa mata uang yang digunakan di Samudra Pasai?", pilihan = new string[] { "Dirham", "Rupiah", "Dinar" }, indexJawabanBenar = 0 },
            new Soal { pertanyaan = "Samudra Pasai sekarang berada di provinsi mana?", pilihan = new string[] { "Aceh", "Sumatera Utara", "Sumatera Barat" }, indexJawabanBenar = 0 },
            new Soal { pertanyaan = "Agama resmi kerajaan Samudra Pasai?", pilihan = new string[] { "Hindu", "Islam", "Budha" }, indexJawabanBenar = 1 },
            new Soal { pertanyaan = "Komoditas utama perdagangan Samudra Pasai?", pilihan = new string[] { "Emas", "Kayu", "Rempah" }, indexJawabanBenar = 0 }
        };
    }

    public void TampilkanSoalRandom(System.Action callback)
    {
        if (daftarSoal.Count == 0)
        {
            Debug.LogWarning("❗ Semua soal sudah habis.");
            EvaluasiHasil();
            return;
        }

        Time.timeScale = 0f; 
        panelSoal.SetActive(true);
        onSelesaiCallback = callback;

        int index = Random.Range(0, daftarSoal.Count);
        soalAktif = daftarSoal[index];
        daftarSoal.RemoveAt(index);

        teksSoal.text = soalAktif.pertanyaan;
        feedbackText.text = "";

        for (int i = 0; i < 3; i++)
        {
            teksPilihan[i].text = soalAktif.pilihan[i];
            tombolPilihan[i].interactable = true; // aktifkan lagi tombol

            int pilihanIndex = i;
            tombolPilihan[i].onClick.RemoveAllListeners();
            tombolPilihan[i].onClick.AddListener(() => PeriksaJawaban(pilihanIndex));
        }
    }

    void PeriksaJawaban(int index)
    {
        soalDijawab++;

        if (index == soalAktif.indexJawabanBenar)
        {
            skor += 20;
            feedbackText.text = "✅ Benar!";
            feedbackText.color = Color.green;
        }
        else
        {
            feedbackText.text = "❌ Salah! Jawaban: " + soalAktif.pilihan[soalAktif.indexJawabanBenar];
            feedbackText.color = Color.red;
        }

        teksSkor.text = "Skor: " + skor;

        // Disable semua tombol setelah menjawab
        foreach (Button btn in tombolPilihan)
        {
            btn.interactable = false;
        }

        StartCoroutine(TungguDanTutup(2f));
    }

    System.Collections.IEnumerator TungguDanTutup(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        TutupPanel();
    }

    void TutupPanel()
    {
        panelSoal.SetActive(false);
        Time.timeScale = 1f;

        if (soalDijawab >= soalTotal)
        {
            EvaluasiHasil();
        }
        else
        {
            onSelesaiCallback?.Invoke();
        }
    }

    void EvaluasiHasil()
    {
        if (skor >= 80)
        {
            Debug.Log("🎉 Level selesai! Skor kamu: " + skor);
            // TODO: Load level berikutnya
        }
        else
        {
            Debug.Log("🔁 Skor kamu " + skor + ". Ulang dari awal.");
            InisialisasiSoal();
            TampilkanSoalRandom(null);
        }
    }

    public int GetSkor()
    {
        return skor;
    }
}
