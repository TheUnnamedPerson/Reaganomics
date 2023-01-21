using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DialogueManager : MonoBehaviour
{
    public string currentDirectory;
    public (int, string, string)[] startingDialogues;
    public NPCDialogue[] npcDialogues;
    public Dictionary<string, Dictionary<string, Dialogue>> activeDialogueAssets;
    public List<string> textAssetNames;
    public TextBox dialoguePanel;
    public ChoicesPanel choicesPanel;
    public GameManager gameManager;
    public string currentFile;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = transform.parent.GetComponent<GameManager>();
        dialoguePanel = transform.GetChild(0).GetChild(0).GetComponent<TextBox>();
        choicesPanel = transform.GetChild(0).GetChild(1).GetComponent<ChoicesPanel>();
    }

    public void InitializeDialogue (SceneDialogueData[] dialogueData)
    {
        Object[] dialogueObjs = Object.FindObjectsOfType(typeof(NPCDialogue), true);
        List<NPCDialogue> npcDialogueList = new List<NPCDialogue>();
        foreach (Object obj in dialogueObjs) npcDialogueList.Add((NPCDialogue)obj);
        foreach (NPCDialogue npcDialogue in npcDialogueList) npcDialogue.dialogueManager = this;
        npcDialogueList.Sort();
        npcDialogues = npcDialogueList.ToArray();

        

        foreach(SceneDialogueData tup in dialogueData)
        {
            npcDialogues[tup.ID].npcName = tup.Name;
            string[] location = tup.StartingOption.Split('.');
            npcDialogues[tup.ID].currentDialogue = activeDialogueAssets[location[0]][location[1]];
            npcDialogues[tup.ID].dialogueManager = this;
        }
    }

    public void LoadDialogue ()
    {
        npcDialogues = new NPCDialogue[1];
        activeDialogueAssets = new Dictionary<string, Dictionary<string, Dialogue>>();
        textAssetNames = new List<string>();

        object[] loadedAssets = Resources.LoadAll("Data/Dialogue/" + currentDirectory + "/", typeof(TextAsset));
        List<TextAsset> tAs = new List<TextAsset>();
        Dictionary<string, TextAsset> tAD = new Dictionary<string, TextAsset>();
        foreach (object obj in loadedAssets)
        {
            textAssetNames.Add(((TextAsset)obj).name);
            tAD.Add(((TextAsset)obj).name, ((TextAsset)obj));
            activeDialogueAssets.Add(((TextAsset)obj).name, new Dictionary<string, Dialogue>());
        }
        foreach (string key in activeDialogueAssets.Keys)
        {
            DialogueDataArray dData = JsonUtility.FromJson<DialogueDataArray>(tAD[key].text);
            foreach (DialogueData dialogueData in dData.Dialogue)
            {
                Dialogue dial = new Dialogue();
                dial.Name = dialogueData.Name;
                dial.Text = dialogueData.Text;
                dial.File = key;
                dial.Choices = dialogueData.Choices;
                UnityEvent dialogueEffect = new UnityEvent();
                foreach (EffectData effectData in dialogueData.Effects)
                {
                    if (effectData.Method == "%None")
                    {
                        dialogueEffect.AddListener(Nothing);
                    }
                    else if (effectData.Method == "%End")
                    {
                        dialogueEffect.AddListener(Nothing);
                    }
                    else
                    {
                        string[] target = effectData.TargetObject.Split('.');
                        var obj = GameObject.Find(target[0]).GetComponent(target[1]);
                        System.Type scriptType = obj.GetType();
                        System.Reflection.MethodInfo info = scriptType.GetMethod(effectData.Method);
                        dialogueEffect.AddListener(() => info.Invoke(obj, new Object[0]));
                    }
                }
                dial.Effects = dialogueEffect;
                activeDialogueAssets[key].Add(dial.Name, dial);
            }
        }
    }

    public void Nothing ()
    {
        return;
    }

    public void EndDialogue ()
    {
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        gameManager.player.inPrompt = false;
        return;
    }
    
    public void PlayDialogue (Dialogue d)
    {
        currentFile = d.File;
        dialoguePanel.transform.GetChild(0).gameObject.SetActive(true);
        dialoguePanel.UpdateText(d);
        StartCoroutine(DisplayChoices(d));
    }

    public void PlayDialogue (string d)
    {
        if (d == "%End")
        {
            EndDialogue();
            return;
        }
        string[] location = d.Split('.');
        if (location[0] == "Self") location[0] = currentFile;
        Dialogue _d = activeDialogueAssets[location[0]][location[1]];
        dialoguePanel.transform.GetChild(0).gameObject.SetActive(true);
        dialoguePanel.UpdateText(_d);
        StartCoroutine(DisplayChoices(_d));
    }

    public IEnumerator DisplayChoices (Dialogue d)
    {
        yield return new WaitForSeconds(0.25f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        if (d.Choices[0].Text == "%None")
        {
            PlayDialogue(d.Choices[0].Result);
            yield break;
        }
        //print("Updating choices");
        StartCoroutine(choicesPanel.UpdateChoices(d));
    }
}
