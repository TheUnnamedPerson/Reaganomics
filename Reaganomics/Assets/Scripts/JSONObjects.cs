using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public string Name;
    public string[] Text;
    public ChoiceData[] Choices;
    public UnityEvent Effects;
    public string File;
}

[System.Serializable]
public class DialogueData
{
    public string Name;
    public string[] Text;
    public ChoiceData[] Choices;
    public EffectData[] Effects;
}
[System.Serializable]
public class DialogueDataArray
{
    public DialogueData[] Dialogue;
}
[System.Serializable]
public class SceneDialogueDataArray
{
    public SceneDialogueData[] Dialogue;
}
[System.Serializable]
public class ChoiceDataArray
{
    public ChoiceData[] Choices;
}
[System.Serializable]
public class EffectDataArray
{
    public EffectData[] Effects;
}

[System.Serializable]
public class ChoiceData
{
    public string Text;
    public string Result;
}

[System.Serializable]
public class EffectData
{
    public string Method;
    public string TargetObject;
}

[System.Serializable]
public class SceneData
{
    public int ID;
    public string Name;
    public string Description;
    public int Music;
    public SceneDialogueData[] Dialogue;
}

[System.Serializable]
public class SceneDialogueData
{
    public int ID;
    public string Name;
    public string StartingOption;
}

