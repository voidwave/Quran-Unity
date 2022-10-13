using UnityEngine;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using ArabicSupport;
public class QuranXmlLoader : MonoBehaviour
{
    Text TextUI;
    public int SuraNumber = 18;
    void Start()
    {
        TextUI = GetComponent<Text>();
        // XmlDocument quran = new XmlDocument();
        // quran.Load("quran-simple.xml");

        Quran quran = DeserializeToObject<Quran>(@"Assets/Text/quran-simple-min.xml");
        
        TextUI.text = "";
        int ayaCount = quran.Sura[SuraNumber].Aya.Count;
        for (int i = 0; i < ayaCount; i++)
            TextUI.text += i + " - " + ArabicFixer.Fix(quran.Sura[SuraNumber].Aya[i].Text, false, false) + '\n';


    }
    public T DeserializeToObject<T>(string filepath) where T : class
    {
        System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

        using (StreamReader sr = new StreamReader(filepath))
        {
            return (T)ser.Deserialize(sr);
        }
    }
}



[XmlRoot(ElementName = "aya")]
public class Aya
{
    [XmlAttribute(AttributeName = "index")]
    public string Index { get; set; }
    [XmlAttribute(AttributeName = "text")]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "sura")]
public class Sura
{
    [XmlElement(ElementName = "aya")]
    public List<Aya> Aya { get; set; }
    [XmlAttribute(AttributeName = "index")]
    public string Index { get; set; }
    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }
}

[XmlRoot(ElementName = "quran")]
public class Quran
{
    [XmlElement(ElementName = "sura")]
    public List<Sura> Sura { get; set; }
}