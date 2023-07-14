using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using TMPro;

public class ButtonChangeTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public TextMeshProUGUI theText;
    public Material material;
    public Color enterColor;
    public Color enterOutline;
    public Color exitColor;
    public Color exitOutline;
    public AudioClip click1;
    //public AudioClip click2;
    public AudioSource audioSource;

    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = enterColor;
        material.SetColor(ShaderUtilities.ID_OutlineColor, enterOutline);
        audioSource.PlayOneShot(click1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = exitColor;
        material.SetColor(ShaderUtilities.ID_OutlineColor, exitOutline);
    }
}