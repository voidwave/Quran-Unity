//بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;
namespace QuranApp
{
    class Main : MonoBehaviour
    {
        int MAXPAGE = 620;
        public int CurrentPageNumber = 1;
        public Image CurrentPage, NextPage;
        private Rect rect;
        private Vector2 pivot;
        void Start()
        {
            pivot = CurrentPage.sprite.pivot;
            rect = CurrentPage.sprite.rect;
            Debug.Log("بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ");
            //Initilize Quran Text from XML
            Data.Init();
            Debug.Log($"Quran Sura Count: {Data.QuranWithTashkeel.Count}");
            //SelectSuraToPrint();
            //SearchForText();
            ////Download test
            ////QDownloader.DownloadAll(QDownloader.QuranArabicURL, 60, 63, QDownloader.QuranSaveToPath);
            //Console.ReadLine();

        }
        public void LoadNextPage()
        {
            if (CurrentPageNumber < MAXPAGE)
                CurrentPageNumber++;
            LoadPage(CurrentPageNumber);
        }
        public void LoadPreviousPage()
        {
            if (CurrentPageNumber > 1)
                CurrentPageNumber--;
            LoadPage(CurrentPageNumber);
        }
        private string path = System.IO.Directory.GetCurrentDirectory() + "/Quran_Arabic_Pages_2/";
        public void LoadPage(int page)
        {
            string PageName = page.ToString("0000");
            //DestroyImmediate(CurrentPage.sprite.texture, true);
            CurrentPage.sprite = Sprite.Create(LoadPNG(path + PageName + ".jpg"), rect, pivot);
            Resources.UnloadUnusedAssets();
            //GC.Collect();
        }

        public static Texture2D LoadPNG(string filePath)
        {
            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            }
            return tex;
        }
        private static void SearchForText()
        {
            string inputText = "";
            Console.WriteLine("Please Type Search Text:");
            inputText = Console.ReadLine();
            List<Ayah> results = ReturnAyatWithText(inputText);
            Console.WriteLine($"Number of Results Found: {results.Count}");

            if (results.Count == 0)
                return;

            //print ayat
            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine("__________________________________________");
                PrintAyah(results[i].suraIndex, results[i].ayahIndex);
            }
        }

        private static void SelectSuraToPrint()
        {
            //Select a Sura
            int SelectedSura = 1;
            Console.WriteLine("Please Select A Sura Number:");
            SelectedSura = int.Parse(Console.ReadLine());

            //Print some data about the Sura
            PrintSura(SelectedSura);
        }

        private static void PrintSura(int SelectedSura)
        {
            Console.WriteLine($"Quran Sura {Data.QuranWithTashkeel[SelectedSura - 1].Name} Ayat Count: {Data.QuranWithTashkeel[SelectedSura - 1].ayatCount}");

            for (int i = 0; i < Data.QuranWithTashkeel[SelectedSura - 1].Ayat.Count; i++)
                Console.WriteLine($"{Data.QuranWithTashkeel[SelectedSura - 1].Ayat[i].suraIndex}:{Data.QuranWithTashkeel[SelectedSura - 1].Ayat[i].ayahIndex} : {Data.QuranWithTashkeel[SelectedSura - 1].Ayat[i].Text}");
        }

        private static void PrintAyah(int suraIndex, int ayahIndex)
        {
            Console.WriteLine($"{suraIndex}:{ayahIndex} : {Data.QuranWithTashkeel[suraIndex - 1].Ayat[ayahIndex - 1].Text}");
        }

        private static List<Ayah> ReturnAyatWithText(string text)
        {
            List<Ayah> results = new List<Ayah>();

            for (int s = 0; s < 114; s++)
                for (int a = 0; a < Data.QuranWithoutTashkeel[s].Ayat.Count; a++)
                    if (Data.QuranWithoutTashkeel[s].Ayat[a].Text.Contains(text))
                        results.Add(Data.QuranWithTashkeel[s].Ayat[a]);

            return results;
        }


    }
}
