//بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ

using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
namespace QuranApp
{
    public class ClickSura : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DebugPoint()
        {
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out localCursor))
                return;

            //localCursor.y +=3830;
            //localCursor.y -= GetComponent<RectTransform>().position.y;
            Debug.Log("LocalCursor:" + localCursor);
        }
    }
}
