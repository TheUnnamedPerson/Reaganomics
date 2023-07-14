using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public GameObject textPanel;
    public RectTransform rectTransform;
    public GameObject portrait;
    void Start()
    {
        textPanel = transform.GetChild(0).gameObject;
        textBox = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        rectTransform = textBox.GetComponent<RectTransform>();
        portrait = transform.GetChild(0).GetChild(0).gameObject;
    }

    public void resetPanel ()
    {
        portrait.SetActive(true);
        rectTransform.SetLeft(700);
    }

    public void HidePortrait ()
    {
        portrait.SetActive(false);
        rectTransform.SetLeft(150);
    }

    public void UpdateText (Dialogue d)
    {
        resetPanel();
        if (textPanel == null) textPanel = transform.GetChild(0).gameObject;
        if (textBox == null) textBox = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        string textToDisplay = d.Text[Random.Range(0,d.Text.Length)];
        if (textToDisplay.Contains("%NoPic%"))
        {
            textToDisplay = textToDisplay.Replace("%NoPic%", "");
            HidePortrait();
        }
        textBox.text = textToDisplay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
