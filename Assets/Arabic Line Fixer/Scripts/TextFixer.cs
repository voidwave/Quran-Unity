using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;

public class TextFixer : MonoBehaviour
{

    //The UI text 
    Text myText;


    //used in cutting the lines 
    int startIndex;
    int endIndex;
    int length;
    int j;
    //*************************

    [TextArea]
    public string EnterText;

    public string[] FixedText = new string[30];
    string[] Holder = new string[5];
    string TextHolder;


    void Start()
    {
        myText = GetComponent<Text>();

        Holder = EnterText.Split('\n');
        //StartCoroutine(FixText());


    }
    public IEnumerator Fix()
    {
        Holder = EnterText.Split('\n');
        myText.text="";
        yield return new WaitForSeconds(1);
        StartCoroutine(FixText());
    }
    IEnumerator FixText()
    {
        
        for (int i = 0; i < Holder.Length; i++)
        {
            myText.text = ArabicFixer.Fix(Holder[i], true, false);
            Canvas.ForceUpdateCanvases();
            for (int k = 0; k < FixedText.Length; k++)
            {
                FixedText[k] = "";
            }

            for (int k = 0; k < myText.cachedTextGenerator.lines.Count; k++)
            {
                startIndex = myText.cachedTextGenerator.lines[k].startCharIdx;
                endIndex = (k == myText.cachedTextGenerator.lines.Count - 1) ? myText.text.Length
                   : myText.cachedTextGenerator.lines[k + 1].startCharIdx;
                length = endIndex - startIndex;
                Debug.Log(myText.text.Substring(startIndex, length));
                FixedText[k] = myText.text.Substring(startIndex, length);
            }
            myText.text = "";
            for (int k = FixedText.Length - 1; k >= 0; k--)
            {
                if (FixedText[k] != "" && FixedText[k] != "\n" && FixedText[k] != null)
                {

                    TextHolder += FixedText[k] + "\n";
                }
            }
        }
        myText.text = TextHolder;

        yield return new WaitForEndOfFrame();


    }



}
