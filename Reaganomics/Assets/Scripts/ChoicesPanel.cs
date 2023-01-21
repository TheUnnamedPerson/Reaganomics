using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoicesPanel : MonoBehaviour
{
    public Transform[] Choices;
    public Transform choicePrefab;
    public DialogueManager dialogueManager; 
    public int optionSelected = 0;
    public float countDown = 0.25f;
    public float buffer = 0.05f;
    private float _countDown = 0.25f;
    public Transform selector1;
    public PlayAudio playAudio;
    void Start()
    {
        dialogueManager = GameObject.FindObjectOfType<DialogueManager>();
    }

    public IEnumerator UpdateChoices (Dialogue d)
    {
        //print("Updating choices Started");
        Choices = new Transform[d.Choices.Length];
        foreach (Transform child in transform.GetChild(0).GetChild(0)) {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < Choices.Length; i++)
        {
            Choices[i] = Instantiate(choicePrefab, transform.GetChild(0).GetChild(0));
            Choices[i].GetComponent<TextMeshProUGUI>().text = d.Choices[i].Text;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        bool ChoseOption = false;
        optionSelected = 0;
        int optionChosen = 0;
        yield return null;
        while (!ChoseOption)
        {
            int _os = optionSelected;
            if (Input.GetKeyDown(KeyCode.S)) optionSelected++;
            if (Input.GetKeyDown(KeyCode.W)) optionSelected--;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)) _countDown -= Time.deltaTime;
            else _countDown = countDown; 
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W)) _countDown = countDown; 
            if (_countDown <= 0) { optionSelected += (Input.GetKey(KeyCode.S) ? 1 : 0) + (Input.GetKey(KeyCode.W) ? -1 : 0); _countDown = buffer; }

            if (optionSelected < 0) optionSelected = 4;
            if (optionSelected > 4) optionSelected = 0;

            selector1.position = new Vector3(selector1.position.x, Choices[optionSelected].transform.position.y, Choices[optionSelected].transform.position.z - 0.1f);

            if (_os != optionSelected) playAudio.playAudio(0);

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                playAudio.playAudio(1);
                optionChosen = optionSelected;
                optionSelected = 0;
                ChoseOption = true;
            }
            yield return null;
        }
        transform.GetChild(0).gameObject.SetActive(false);
        dialogueManager.PlayDialogue(d.Choices[optionChosen].Result);
    }

}
