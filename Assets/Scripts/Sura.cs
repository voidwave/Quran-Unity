//بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ

using System.Collections.Generic;
namespace QuranApp
{
    public class Sura
    {
        public string Name = "";
        public int index, ayatCount = 0;
        public List<Ayah> Ayat = new List<Ayah>();

    }

    public struct Ayah
    {
        public Ayah(string text, int s, int i)
        {
            Text = text;
            suraIndex = s;
            ayahIndex = i;
        }
        public string Text;
        public int suraIndex, ayahIndex;
    }

}