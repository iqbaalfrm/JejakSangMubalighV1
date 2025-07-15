using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DaftarSoal", menuName = "Quiz/DaftarSoal")]
public class DaftarSoal : ScriptableObject
{
    //public Soal[] soalArray;
    public string pertanyaan;
    public string[] pilihanJawaban;
    public int indexJawabanBenar;
}




