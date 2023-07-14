using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleOptions : MonoBehaviour
{
    
    public Character selectedPlayer;
    public bool battleLoaded = false;
    public SpriteRenderer background;
    public PlayAudio playAudio;
    public BattleManager battleManager;
    public RectTransform container;
    public TMP_Text _name;
    public TMP_Text[] options = new TMP_Text[5];
    public Transform selector1;
    public int optionSelected = 0;
    public float countDown = 0.25f;
    public float buffer = 0.05f;
    private float _countDown = 0.25f;
    public int menu = 0;
    public Vector3 newPos;

    void Start()
    {
        playAudio = GetComponent<PlayAudio>();
    }

    void OnDisable ()
    {
        selectedPlayer = null;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.position = new Vector3(50,50,50);
        newPos = new Vector3(50,50,50);
    }

    void OnEnable ()
    {
        StartCoroutine(onEnabled());
    }

    IEnumerator onEnabled ()
    {
        yield return new WaitUntil(() => selectedPlayer != null);
        UpdateOptions();
        yield return new WaitForEndOfFrame();
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => newPos != new Vector3(50,50,50));
        transform.position = newPos;
    }

    void Update()
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

        _name.gameObject.GetComponent<LayoutElement>().minWidth = _name.GetRenderedValues(true).x;

        foreach (TMP_Text o in options)
        {
            o.gameObject.GetComponent<LayoutElement>().minWidth = (o.GetRenderedValues(true).x >= 1.8f) ? o.GetRenderedValues(true).x : 1.8f;
            o.transform.GetChild(0).transform.position = new Vector3(o.transform.position.x - (o.transform.GetComponent<RectTransform>().sizeDelta.x / 2), o.transform.position.y, o.transform.position.z);
        }

        background.size = new Vector2(container.sizeDelta.x + 0.7f, background.size.y);

        selector1.position = new Vector3(options[optionSelected].transform.GetChild(0).transform.position.x - 0.3f, options[optionSelected].transform.position.y, options[optionSelected].transform.position.z - 0.1f);

        if (_os != optionSelected) playAudio.playAudio(0);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            playAudio.playAudio(1);
            OptionPressed();
        }
    }
    public void OptionPressed ()
    {
        switch (menu)
        {
            case 0:
                menu = optionSelected + 1;
                optionSelected = 0;
                UpdateOptions();
                break;
            case 1:
                if (optionSelected == 4) menu = 0;
                else battleManager.optionChosen = optionSelected + 1;
                optionSelected = 0;
                UpdateOptions();
                break;
            case 2:
                if (optionSelected == 4) menu = 0;
                else if (optionSelected == 1) battleManager.optionChosen = -1;
                else battleManager.optionChosen = optionSelected + 1;
                optionSelected = 0;
                UpdateOptions();
                break;
            case 3:
                if (optionSelected == 4) menu = 0;
                else battleManager.optionChosen = optionSelected + 1;
                optionSelected = 0;
                UpdateOptions();
                break;
            case 4:
                if (optionSelected == 4) menu = 0;
                else battleManager.optionChosen = optionSelected + 1;
                optionSelected = 0;
                UpdateOptions();
                break;
        }
    }

    public void UpdateOptions ()
    {
        _name.text = selectedPlayer.Name;
        
        switch (menu)
        {
            case 0:
                options[0].text = "Fight";
                options[1].text = "Act";
                options[2].text = "Item";
                options[3].text = "Party";
                options[4].text = "Flee";
                break;
            case 1:
                options[0].text = Localization.Attacks[selectedPlayer.Attacks[0]].Name;
                options[1].text = Localization.Attacks[selectedPlayer.Attacks[1]].Name;
                options[2].text = Localization.Attacks[selectedPlayer.Attacks[2]].Name;
                options[3].text = Localization.Attacks[selectedPlayer.Attacks[3]].Name;
                options[4].text = "Back";
                break;
            case 2:
                options[0].text = "Nothing";
                options[1].text = "N/a";
                options[2].text = "N/a";
                options[3].text = "N/a";
                options[4].text = "Back";
                break;
            case 3:
                options[0].text = "Fight";
                options[1].text = "Act";
                options[2].text = "Item";
                options[3].text = "Party";
                options[4].text = "Flee";
                break;
            case 4:
                options[0].text = "Fight";
                options[1].text = "Act";
                options[2].text = "Item";
                options[3].text = "Party";
                options[4].text = "Flee";
                break;
        }
    }
}
