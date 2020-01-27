//بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ

using System.Xml;
using System.Collections.Generic;
namespace QuranApp
{
    static class Data
    {
        public static string PathToQuranWithoutTashkeel = System.IO.Directory.GetCurrentDirectory() + "/Text/quran-simple-clean.xml";
        public static string PathToQuranWithTashkeel = System.IO.Directory.GetCurrentDirectory() + "/Text/quran-simple.xml";
        public static List<Sura> QuranWithTashkeel, QuranWithoutTashkeel;// = new List<Sura>();
        public static void Init()
        {
            //Generating SuraList from XML
            QuranWithoutTashkeel = ReadXML(PathToQuranWithoutTashkeel);
            QuranWithTashkeel = ReadXML(PathToQuranWithTashkeel);
        }
        public static List<Sura> ReadXML(string path)
        {
            int currentSuraIndex = -1;
            List<Sura> Quran = new List<Sura>();

            XmlReader xmlReader = XmlReader.Create(path);
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "sura"))
                    if (xmlReader.HasAttributes)
                    {
                        currentSuraIndex++;
                        Sura currentSura = new Sura();
                        currentSura.Name = xmlReader.GetAttribute("name");
                        currentSura.index = int.Parse(xmlReader.GetAttribute("index"));
                        Quran.Add(currentSura);
                    }

                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "aya"))
                    if (xmlReader.HasAttributes)
                    {
                        Quran[currentSuraIndex].ayatCount++;
                        Ayah ayah = new Ayah(xmlReader.GetAttribute("text"), currentSuraIndex + 1, Quran[currentSuraIndex].ayatCount);
                        Quran[currentSuraIndex].Ayat.Add(ayah);

                    }


            }

            return Quran;
        }
    }
}
