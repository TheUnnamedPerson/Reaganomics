using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public GameObject textPanel;
    void Start()
    {
        textPanel = transform.GetChild(0).gameObject;
        textBox = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText (Dialogue d)
    {
        if (textPanel == null) textPanel = transform.GetChild(0).gameObject;
        if (textBox == null) textBox = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        textBox.text = d.Text[Random.Range(0,d.Text.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
