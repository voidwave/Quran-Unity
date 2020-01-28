//بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ

using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace QuranApp
{
    public static class BuildSuraButtons
    {
        //public GameObject SuraButtonPrefab;

        // Start is called before the first frame update
        public static void InitilizeUI(Main main, Transform NavParent)
        {
            int startSura = 93;
            //Debug.Log(int.Parse("A1", System.Globalization.NumberStyles.HexNumber));
            //Debug.Log(int.Parse("F1", System.Globalization.NumberStyles.HexNumber));
            GameObject SuraButtonPrefab = Resources.Load("Sura_") as GameObject;

            for (int i = 0; i < 114; i++)
            {
                GameObject sura = GameObject.Instantiate(SuraButtonPrefab, Vector3.zero, Quaternion.identity, NavParent);
                sura.name = "Sura_" + (i + 1);

                //fix for jumps in unicode font
                if (i == 35)
                    startSura = 161;
                if (i == 47)
                    startSura++;
                if (i == 56)
                    startSura++;

                //Title Name
                uint utf32 = uint.Parse(startSura.ToString("X"), System.Globalization.NumberStyles.HexNumber);
                string s = Encoding.Unicode.GetString(BitConverter.GetBytes(utf32));
                sura.transform.GetChild(0).GetComponent<Text>().text = s;
                startSura++;


                int suraNumber = i + 1;
                //Debug.Log(suraNumber);
                sura.transform.GetChild(1).GetComponent<Text>().text = suraNumber.ToString();


                //Assign Button
                sura.GetComponent<Button>().onClick.AddListener(delegate () { main.GotoSura(suraNumber); });
                sura.GetComponent<Button>().onClick.AddListener(delegate () { main.SettingsToggle(); });
                

            }
        }



    }
}