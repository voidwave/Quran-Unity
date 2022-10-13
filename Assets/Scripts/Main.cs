//بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using ArabicSupport;

namespace QuranApp
{
    public class Main : MonoBehaviour
    {
        private Camera mainCam;
        private int MAXPAGE = 620;
        public int CurrentPageNumber = 1;
        public Image CurrentPage, AnimatedPage, ProgressBar;
        public RectTransform CurrentPageRectTransform;
        public Toggle invertToggleUI;
        public Material NormalMat, InvertMat;
        public GameObject SettingsWindow, PreviousButtonGO, NextButtonGO, Downloader, DownloadButton;
        public Transform NavParent;
        private Rect rect;
        private Vector2 pivot;
        public Text DownloadText;//, DebugText;
        public Color Green, Red;
        public UnityEngine.UI.Dropdown NavDropDown;//, DownloadLocation;

        void Start()
        {

            string basmalah = "بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ";
            mainCam = GetComponent<Camera>();
            //Debug.Log(ArabicFixer.Fix(basmalah));
            //Debug.Log(ArabicFixer.Fix(basmalah, true, true));
            Application.targetFrameRate = 60;
            //DebugText.text = ArabicFixer.Fix("بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ", false, false);

            SettingsWindow.SetActive(false);
            path = Application.persistentDataPath + "/Quran/750/jpg_90/";
            //DebugText.text = path;
            //Data.PathToQuranWithoutTashkeel = Application.persistentDataPath + "/Text/quran-simple-clean.xml";
            //Data.PathToQuranWithTashkeel = Application.persistentDataPath + "/Text/quran-simple.xml";
            pivot = CurrentPage.sprite.pivot;
            rect = CurrentPage.sprite.rect;
            //pageInitialPosition = new Vector3(0,-960,0);
            //Initilize Quran Text from XML
            //Data.Init();
            //Debug.Log($"Quran Sura Count: {Data.QuranWithTashkeel.Count}");
            //SelectSuraToPrint();
            //SearchForText();
            ////Download test
            ////QDownloader.DownloadAll(QDownloader.QuranArabicURL, 60, 63, QDownloader.QuranSaveToPath);
            //Console.ReadLine();
            // PlayerPrefs.SetInt("QuranDownloaded", 0);
            // PlayerPrefs.SetInt("QuranExtracted", 0);

            CheckQuranFiles();

        }

        //private bool fileDownloaded, fileExtracted = false;
        private void CheckQuranFiles()
        {
            Debug.Log("Checking Files...");

            if (PlayerPrefs.GetInt("QuranDownloaded") != 114)
            {
                Downloader.SetActive(true);
                DownloadText.text = "(ﺖﻳﺎﺑﺎﺠﻴﻣ 160 ﻢﺠﺣ) ﻊﻗﻮﻤﻟﺍ ﻦﻣ نﺁﺮﻘﻟﺍ تﺎﻔﻠﻣ ﻞﻳﺰﻨﺗ ﻰﻟﺇ ﻖﻴﺒﻄﺘﻟﺍ جﺎﺘﺤﻳ";
                DownloadButton.SetActive(true);
                DownloadButton.transform.GetChild(0).GetComponent<Text>().text = "ﻖﻓﺍﻮﻣ";
                DownloadButton.GetComponent<Image>().color = Green;

                return;
            }

            if (PlayerPrefs.GetInt("QuranExtracted") != 114)
            {
                StartCoroutine(ExtractQuranFiles());
                Debug.Log("extracting Quran files...");
                return;
            }

            //Quran Loaded
            Debug.Log("Quran finished loading");
            Downloader.SetActive(false);
            //yield return new WaitForSeconds(0.01f);

            //Set a var to Ready, to avoid checking files again
            //PlayerPrefs.SetInt("Quran_Ready", 114);
            BuildSuraButtons.InitilizeUI(this, NavParent);
            InitilizePrefs();

        }

        private bool isDownloading = false;

        public void DownloadButtonClicked()
        {
            if (isDownloading)
                StopDownloading();
            else
                StartDownloading();
        }
        private void StartDownloading()
        {

            Downloader.SetActive(true);
            StartCoroutine(DownloadQuranFiles());
            isDownloading = true;
            DownloadButton.SetActive(true);
            DownloadButton.transform.GetChild(0).GetComponent<Text>().text = "ﻞﻳﺰﻨﺘﻟﺍ ءﺎﻐﻟﺍ";
            DownloadButton.GetComponent<Image>().color = Red;
            Debug.Log("Quran downloading...");
        }

        private void StopDownloading()
        {
            StopAllCoroutines();
            isDownloading = false;
            Downloader.SetActive(true);
            DownloadText.text = "(ﺖﻳﺎﺑﺎﺠﻴﻣ 160 ﻢﺠﺣ) ﻊﻗﻮﻤﻟﺍ ﻦﻣ نﺁﺮﻘﻟﺍ تﺎﻔﻠﻣ ﻞﻳﺰﻨﺗ ﻰﻟﺇ ﻖﻴﺒﻄﺘﻟﺍ جﺎﺘﺤﻳ";
            DownloadButton.SetActive(true);
            DownloadButton.transform.GetChild(0).GetComponent<Text>().text = "ﻖﻓﺍﻮﻣ";
            DownloadButton.GetComponent<Image>().color = Green;
            Debug.Log("Quran stopped downloading...");
        }
        System.Collections.IEnumerator DownloadQuranFiles()
        {

            Debug.Log("DownloadQuranFiles()...");
            Downloader.SetActive(true);

            string downloadLink = "http://voidwave.com/AlQuran.zip";

            // if (DownloadLocation.value == 0)
            //     downloadLink = "https://images.qurancomplex.gov.sa/publications/a_70_nastaleeq/750.zip";
            // if (DownloadLocation.value == 1)
            //     downloadLink = "http://voidwave.com/AlQuran.zip";

            WWW Qlink = new WWW(downloadLink);

            while (!Qlink.isDone)
            {
                //Downloading
                DownloadText.text = string.Format("ﺖﻳﺎﺑﺎﺠﻴﻣ {1:0.0}/160   ﻒﺤﺼﻤﻟﺍ ﻞﻳﺰﻨﺗ {0:P1}", Qlink.progress, Qlink.progress * 168);
                ProgressBar.fillAmount = Qlink.progress;
                yield return new WaitForSeconds(0.1f);
            }

            yield return Qlink;

            if (!string.IsNullOrEmpty(Qlink.error))
            {
                CheckQuranFiles();

            }
            else
            {
                //saving zip to path
                string savePath = string.Format("{0}/Quran.zip", Application.persistentDataPath);
                System.IO.File.WriteAllBytes(savePath, Qlink.bytes);
                //Quran compressed file downloaded
                PlayerPrefs.SetInt("QuranDownloaded", 114);
                CheckQuranFiles();
            }

        }

        System.Collections.IEnumerator ExtractQuranFiles()
        {
            Debug.Log("ExtractQuranFiles()...");
            DownloadText.text = "ةﺮﻴﺧﻷﺍ تﺎﺴﻤﻠﻟﺍ";
            DownloadButton.SetActive(false);
            yield return new WaitForSeconds(.1f);

            //Unzip file
            string zipPath = string.Format("{0}/Quran.zip", Application.persistentDataPath);
            var dir = new DirectoryInfo(Application.persistentDataPath + "/Quran/");
            if (dir.Exists)
                dir.Delete(true);

            ZipFile.ExtractToDirectory(zipPath, Application.persistentDataPath + "/Quran/");
            //System.IO.Compression.
            PlayerPrefs.SetInt("QuranExtracted", 114);
            File.Delete(zipPath);
            CheckQuranFiles();
        }
        private bool touching = false;
        private Vector2 startTouchPos, endTouchPos;
        private float touchTime = 0;
        [SerializeField]
        private float triggerTimeStart = 0.1f;
        [SerializeField]
        private float triggerTimeEnd = 0.4f;
        public Vector3 pageInitialPosition = Vector3.zero;
        void Update()
        {
            SwipeInput();
            ScreenRotationEvent();
            PageAnimation();
        }
        private float pageAnimating = 0;
        private bool animateRight = false;
        public float animationSpeed = 1;
        public bool animationDone = true;
        public float widthMultiplier = 3;
        private void PageAnimation()
        {
            if (animationDone)
                return;

            if (pageAnimating <= 0)
            {
                AnimatedPage.sprite = CurrentPage.sprite;
                //AnimatedPage.sprite = CurrentPage.sprite;
                //AnimatedPage.sprite = CurrentPage.sprite;
                AnimatedPage.transform.localPosition = pageInitialPosition;
                animationDone = true;
                return;
            }

            if (animateRight && AnimatedPage.transform.localPosition.x < 1340)//Screen.width * widthMultiplier)
                AnimatedPage.transform.localPosition = Vector3.Lerp(AnimatedPage.transform.localPosition, AnimatedPage.transform.localPosition + Vector3.right * 1340, Time.deltaTime * animationSpeed);
            else if (!animateRight && AnimatedPage.transform.localPosition.x > -1340)
                AnimatedPage.transform.localPosition = Vector3.Lerp(AnimatedPage.transform.localPosition, AnimatedPage.transform.localPosition + Vector3.left * 1340, Time.deltaTime * animationSpeed);

            pageAnimating -= Time.deltaTime;
        }

        //private ScreenOrientationState CurrentScreenOrientation;
        private void ScreenRotationEvent()
        {
            // if (ScreenState == ScreenOrientationState.AutoRotation)
            // {

            if ((Input.deviceOrientation == DeviceOrientation.LandscapeLeft) && (Screen.orientation != ScreenOrientation.LandscapeLeft))
            {
                SettingsOff();
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                return;
            }

            if ((Input.deviceOrientation == DeviceOrientation.LandscapeRight) && (Screen.orientation != ScreenOrientation.LandscapeRight))
            {
                SettingsOff();
                Screen.orientation = ScreenOrientation.LandscapeRight;
                return;
            }

            if ((Input.deviceOrientation == DeviceOrientation.Portrait) && (Screen.orientation != ScreenOrientation.Portrait))
            {
                SettingsOff();
                Screen.orientation = ScreenOrientation.Portrait;
                return;
            }

            if ((Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) && (Screen.orientation != ScreenOrientation.PortraitUpsideDown))
            {
                SettingsOff();
                Screen.orientation = ScreenOrientation.PortraitUpsideDown;
                return;
            }
            //}


        }

        // private void SwipeInput()
        // {
        //     if (!UseSwipe || SettingsWindow.activeSelf)
        //         return;

        //     //swipe input 
        //     if (Input.GetMouseButton(0))
        //     {
        //         touchTime += Time.deltaTime;
        //         if (!touching)
        //         {
        //             touching = true;
        //             startTouchPos = Input.mousePosition;
        //             //Debug.Log($"Start Pos: {startTouchPos}");
        //         }

        //     }
        //     else
        //     {
        //         if (touching)
        //         {
        //             endTouchPos = Input.mousePosition;
        //             touching = false;
        //             //Debug.Log($"End Pos: {endTouchPos}");

        //             if (touchTime > triggerTimeStart && touchTime < triggerTimeEnd)
        //                 SwipePage();

        //         }
        //         touchTime = 0;
        //     }
        // }
        private void SwipeInput()
        {
            if (SettingsWindow.activeSelf)
                return;

            if (!animationDone)
                return;

            //swipe input 
            if (Input.GetMouseButton(0))
            {
                //touchTime += Time.deltaTime;
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

                    if (startTouchPos.y < 200 || endTouchPos.y < 200)
                        return;

                    if (CurrentPageRectTransform.localPosition.x > 200)
                        LoadNextPage();
                    if (CurrentPageRectTransform.localPosition.x < -200)
                        LoadPreviousPage();
                    
                }
                //touchTime = 0;
            }
        }
        //public float swipePercentDistance = 0.25f;
        // private void SwipePage()
        // {
        //     if (!animationDone)
        //         return;

        //     if (startTouchPos.y < 200 || endTouchPos.y < 200)
        //         return;
        //     //from left to right
        //     if (startTouchPos.x + swipePercentDistance * Screen.width < endTouchPos.x)
        //     {
        //         LoadNextPage();
        //     }
        //     //from right to left
        //     if (startTouchPos.x > endTouchPos.x + swipePercentDistance * Screen.width)
        //     {
        //         LoadPreviousPage();
        //     }
        // }
        private void InitilizePrefs()
        {

            //setting some settings for first time run
            int firstTime = PlayerPrefs.GetInt("FirstTime");
            if (firstTime != 114)
            {
                PlayerPrefs.SetInt("Invert", 0);
                PlayerPrefs.SetInt("FirstTime", 114);
                PlayerPrefs.SetInt("Nav", 0);
                //PlayerPrefs.SetInt("Orientation", 0);
            }

            //load user settings
            InvertToggle(PlayerPrefs.GetInt("Invert") == 1);
            //SwitchRotation(PlayerPrefs.GetInt("Orientation"));
            SwitchNavigation(PlayerPrefs.GetInt("Nav"));

            //adjust settings state in ui
            invertToggleUI.isOn = PlayerPrefs.GetInt("Invert") == 1 ? true : false;
            //RotationDropDown.value = (PlayerPrefs.GetInt("Orientation"));
            NavDropDown.value = (PlayerPrefs.GetInt("Nav"));

            //load last page viewed
            CurrentPageNumber = PlayerPrefs.GetInt("CurrentPage");
            if (CurrentPageNumber < 1 || CurrentPageNumber > MAXPAGE)
                CurrentPageNumber = 1;
            LoadPage(CurrentPageNumber);
            AnimatedPage.sprite = CurrentPage.sprite;
            AnimatedPage.transform.localPosition = pageInitialPosition;
        }

        public void LoadNextPage()
        {

            AnimatedPage.sprite = CurrentPage.sprite;
            AnimatedPage.transform.localPosition = pageInitialPosition;

            if (CurrentPageNumber < MAXPAGE)
                CurrentPageNumber++;
            LoadPage(CurrentPageNumber, 1);



            animationDone = false;
            animateRight = true;
            pageAnimating = 0.5f;
        }
        public void LoadPreviousPage()
        {
            AnimatedPage.sprite = CurrentPage.sprite;
            AnimatedPage.transform.localPosition = pageInitialPosition;


            if (CurrentPageNumber > 1)
                CurrentPageNumber--;
            LoadPage(CurrentPageNumber, -1);


            animationDone = false;
            animateRight = false;
            pageAnimating = 0.5f;
        }

        private string path;// = Application.persistentDataPath + "/Quran_Arabic_Pages_2/";
        public void LoadPage(int page, int nextPage = 0)
        {
            // if (page > 2 && page < 614)
            //     PageNumberText.text = (page - 2).ToString();
            // else
            //     PageNumberText.text = "-";

            string PageName = page.ToString("0000");

            //load from file
            //CurrentPage.sprite = Sprite.Create(LoadPNG(path + PageName + ".jpg"), rect, pivot);

            //load from folder/resources
            //Resources.Load<Texture2D>("Quran_Arabic_Pages_2/" + PageName);
            CurrentPage.sprite = Sprite.Create(LoadPNG(path + PageName + ".jpg"), rect, pivot);
            if (nextPage > 0)
                AnimatedPage.transform.GetChild(0).GetComponent<Image>().sprite = CurrentPage.sprite;
            else if (nextPage < 0)
                AnimatedPage.transform.GetChild(1).GetComponent<Image>().sprite = CurrentPage.sprite;

            Resources.UnloadUnusedAssets();
            PlayerPrefs.SetInt("CurrentPage", CurrentPageNumber);

            Vector3 tempPos = CurrentPageRectTransform.position;
            //tempPos.y = -960;
            CurrentPageRectTransform.position = tempPos;

        }

        public void SettingsToggle()
        {
            SettingsWindow.SetActive(!SettingsWindow.activeSelf);
            CurrentPage.enabled = (!SettingsWindow.activeSelf);
        }
        private void SettingsOff()
        {
            SettingsWindow.SetActive(false);
            CurrentPage.enabled = (true);
        }
        public void InvertToggle(bool toggle)
        {
            if (toggle)
            {
                mainCam.backgroundColor = Color.black;
                CurrentPage.material = InvertMat;
                AnimatedPage.material = InvertMat;
                AnimatedPage.transform.GetChild(0).GetComponent<Image>().material = InvertMat;
                AnimatedPage.transform.GetChild(1).GetComponent<Image>().material = InvertMat;
                PlayerPrefs.SetInt("Invert", 1);
            }
            else
            {
                mainCam.backgroundColor = Color.white;
                CurrentPage.material = NormalMat;
                AnimatedPage.material = NormalMat;
                AnimatedPage.transform.GetChild(0).GetComponent<Image>().material = NormalMat;
                AnimatedPage.transform.GetChild(1).GetComponent<Image>().material = NormalMat;
                PlayerPrefs.SetInt("Invert", 0);
            }
        }

        public void SwitchRotation(int orientation)
        {
            //SettingsWindow.SetActive(false);
            PlayerPrefs.SetInt("Orientation", orientation);
            //ScreenState = (ScreenOrientationState)orientation;
            switch (orientation)
            {
                case 0:
                    Screen.orientation = ScreenOrientation.AutoRotation;
                    break;
                case 1:
                    Screen.orientation = ScreenOrientation.Portrait;
                    break;
                case 2:
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                    break;
            }

            //RotationDropDown.value = orientation;
        }



        private bool UseSwipe = true;
        public void SwitchNavigation(int nav)
        {
            PlayerPrefs.SetInt("Nav", nav);

            switch (nav)
            {
                case 0:
                    PreviousButtonGO.SetActive(true);
                    NextButtonGO.SetActive(true);
                    UseSwipe = true;
                    break;
                case 1:

                    PreviousButtonGO.SetActive(true);
                    NextButtonGO.SetActive(true);
                    UseSwipe = false;

                    break;
                case 2:
                    PreviousButtonGO.SetActive(false);
                    NextButtonGO.SetActive(false);
                    UseSwipe = true;
                    break;
            }

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
            //SuraNameText.text = BuildSuraButtons.SuraNames[SuraNumber];
            AnimatedPage.sprite = CurrentPage.sprite;
            AnimatedPage.transform.localPosition = pageInitialPosition;

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
