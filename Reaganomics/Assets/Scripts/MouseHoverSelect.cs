using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHoverSelect : MonoBehaviour
{
    public ChoicesPanel choicesPanel;
    
    void Start ()
    {
        choicesPanel = GameObject.FindObjectOfType<ChoicesPanel>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Pointer Entered");
        choicesPanel.optionSelected = transform.GetSiblingIndex();
    }

}
