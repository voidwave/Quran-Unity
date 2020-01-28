//بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ

using UnityEngine;
using UnityEngine.UI;
using System;
//using System.IO;
using System.Collections.Generic;
namespace QuranApp
{
    public class Main : MonoBehaviour
    {
        private int MAXPAGE = 620;
        public int CurrentPageNumber = 1;
        public Image CurrentPage, NextPage;
        public RectTransform CurrentPageRectTransform;
        public Toggle invertToggleUI;
        public Material NormalMat, InvertMat;
        public GameObject SettingsWindow;
        public Transform NavParent;
        private Rect rect;
        private Vector2 pivot;
        public Text PageNumberText, SuraNameText;
        //public DropDown RotationDropDown;

        void Start()
        {
            Debug.Log("بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ");
            SettingsWindow.SetActive(false);
            path = Application.persistentDataPath + "/Quran_Arabic_Pages_2/";
            //Data.PathToQuranWithoutTashkeel = Application.persistentDataPath + "/Text/quran-simple-clean.xml";
            //Data.PathToQuranWithTashkeel = Application.persistentDataPath + "/Text/quran-simple.xml";

            pivot = CurrentPage.sprite.pivot;
            rect = CurrentPage.sprite.rect;

            //Initilize Quran Text from XML
            //Data.Init();
            //Debug.Log($"Quran Sura Count: {Data.QuranWithTashkeel.Count}");
            //SelectSuraToPrint();
            //SearchForText();
            ////Download test
            ////QDownloader.DownloadAll(QDownloader.QuranArabicURL, 60, 63, QDownloader.QuranSaveToPath);
            //Console.ReadLine();
            BuildSuraButtons.InitilizeUI(this, NavParent);
            InitilizePrefs();

        }

        private bool touching = false;
        private Vector2 startTouchPos, endTouchPos;
        private float touchTime = 0;
        [SerializeField]
        private float triggerTimeStart = 0.1f;
        [SerializeField]
        private float triggerTimeEnd = 0.4f;
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                touchTime += Time.deltaTime;
                if (!touching)
                {
                    touching = true;
                    startTouchPos = Input.mousePosition;
                    //Debug.Log($"Start Pos: {startTouchPos}");
                }

            }
            else
            {
                if (touching)
                {
                    endTouchPos = Input.mousePosition;
                    touching = false;
                    //Debug.Log($"End Pos: {endTouchPos}");

                    if (touchTime > triggerTimeStart && touchTime < triggerTimeEnd)
                        SwipePage();

                }
                touchTime = 0;
            }
        }
        private void SwipePage()
        {
            //from left to right
            if (startTouchPos.x + 350 < endTouchPos.x)
            {
                LoadNextPage();
            }
            //from right to left
            if (startTouchPos.x > endTouchPos.x + 350)
            {
                LoadPreviousPage();
            }
        }
        private void InitilizePrefs()
        {
            //setting some settings for first time run
            int firstTime = PlayerPrefs.GetInt("FirstTime");
            if (firstTime != 114)
            {
                PlayerPrefs.SetInt("Invert", 1);
                PlayerPrefs.SetInt("FirstTime", 114);
            }

            //load user settings
            InvertToggle(PlayerPrefs.GetInt("Invert"));
            ToggleRotation(PlayerPrefs.GetInt("Orientation"));
            //RotationDropDown.value = PlayerPrefs.GetInt("Orientation");

            //adjust settings state in ui
            invertToggleUI.isOn = PlayerPrefs.GetInt("Invert") == 1 ? true : false;

            //load last page viewed
            CurrentPageNumber = PlayerPrefs.GetInt("CurrentPage");
            if (CurrentPageNumber < 1 || CurrentPageNumber > MAXPAGE)
                CurrentPageNumber = 1;
            LoadPage(CurrentPageNumber);
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

        private string path;// = Application.persistentDataPath + "/Quran_Arabic_Pages_2/";
        public void LoadPage(int page)
        {
            if (page > 2 && page < 614)
                PageNumberText.text = (page - 2).ToString();
            else
                PageNumberText.text = "-";

            string PageName = page.ToString("0000");

            //load from file
            //CurrentPage.sprite = Sprite.Create(LoadPNG(path + PageName + ".jpg"), rect, pivot);

            //load from resources
            CurrentPage.sprite = Sprite.Create(Resources.Load<Texture2D>("Quran_Arabic_Pages_2/" + PageName), rect, pivot);
            Resources.UnloadUnusedAssets();
            PlayerPrefs.SetInt("CurrentPage", CurrentPageNumber);

            Vector3 tempPos = CurrentPageRectTransform.position;
            tempPos.y -= 2000;
            CurrentPageRectTransform.position = tempPos;

            //update sura name text
            for (int i = 0; i < 114; i++)
            {
                if (CurrentPageNumber > 613 || CurrentPageNumber < 4)
                {
                    SuraNameText.text = "";
                    break;
                }

                if (CurrentPageNumber == SuraPageNumbers[i])
                {
                    SuraNameText.text = BuildSuraButtons.SuraNames[i];
                    break;
                }
                if (CurrentPageNumber > SuraPageNumbers[i] && CurrentPageNumber < SuraPageNumbers[i + 1])
                {
                    SuraNameText.text = BuildSuraButtons.SuraNames[i];
                    break;
                }
            }
        }

        public void SettingsToggle()
        {
            SettingsWindow.SetActive(!SettingsWindow.activeSelf);
        }
        public void InvertToggle(bool toggle)
        {
            if (toggle)
            {
                CurrentPage.material = InvertMat;
                PlayerPrefs.SetInt("Invert", 1);
            }
            else
            {
                CurrentPage.material = NormalMat;
                PlayerPrefs.SetInt("Invert", 0);
            }
        }
        public void InvertToggle(int toggle)
        {
            if (toggle == 1)
                CurrentPage.material = InvertMat;
            else
                CurrentPage.material = NormalMat;
        }

        public void ToggleRotation(int orientation)
        {
            switch (orientation)
            {
                case 0:
                    Screen.orientation = ScreenOrientation.AutoRotation;
                    break;
                case 1:
                    Screen.orientation = ScreenOrientation.Portrait;
                    break;
                case 2:
                    Screen.orientation = ScreenOrientation.Landscape;
                    break;
            }

            PlayerPrefs.SetInt("Orientation", orientation);
            //RotationDropDown.value = orientation;
        }
        private int[] SuraPageNumbers = {
            004,005,053,080,109,131,154,180,190,211,
            224,238,252,258,264,270,285,296,308,315,
            325,334,345,353,362,369,379,388,399,407,
            414,418,421,431,437,443,448,455,461,470,
            480,486,492,498,501,505,509,514,518,521,
            523,526,529,531,534,537,540,545,548,552,
            554,556,557,559,561,563,565,567,570,572,
            574,576,579,581,583,585,587,589,590,592,
            593,594,595,597,598,599,600,600,601,603,
            603,604,605,605,606,606,607,607,608,608,
            609,609,610,610,610,611,611,611,611,612,
            612,612,613,613};
        public void GotoSura(int SuraNumber)
        {
            //Debug.Log(SuraNumber);
            SuraNumber--;
            CurrentPageNumber = SuraPageNumbers[SuraNumber];
            LoadPage(CurrentPageNumber);
            SuraNameText.text = BuildSuraButtons.SuraNames[SuraNumber];

        }
        // public static Texture2D LoadPNG(string filePath)
        // {
        //     Texture2D tex = null;
        //     byte[] fileData;

        //     if (File.Exists(filePath))
        //     {
        //         fileData = File.ReadAllBytes(filePath);
        //         tex = new Texture2D(2, 2);
        //         tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        //     }
        //     return tex;
        // }
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
