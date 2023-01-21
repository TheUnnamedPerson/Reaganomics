using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Character> Party;
    public BattleManager battleManager;
    public DialogueManager dialogueManager;
    public bool battle = false;
    public Scene currentScene;
    public GameObject cube;
    public Texture2D tex;
    public Texture2D tex2;
    public int money = 0;
    public Vector3[] previousPos;
    public Player player;
    public GameObject prev42;
    public Camera MainCam;
    public Camera CloseUpCam;
    public GameObject Closeup;
    public RenderTexture rend;
    public GameObject LoadingScreen;
    public GameObject startingCamera;
    public bool loadedStartingScene = false;
    public PlayAudio musicPlayer;
    public SceneData currentSceneData;
    public bool devMode = false;

    void Awake ()
    {
        DontDestroyOnLoad(gameObject);
        dialogueManager = transform.GetChild(4).GetComponent<DialogueManager>();
        musicPlayer = transform.GetChild(7).GetComponent<PlayAudio>();
        LoadingScreen = transform.GetChild(6).gameObject;
        LoadingScreen.SetActive(false);
        MainCam = transform.GetChild(1).GetComponent<Camera>();
        MainCam.gameObject.SetActive(false);
        CloseUpCam = transform.GetChild(2).GetComponent<Camera>();
        Closeup = transform.GetChild(3).gameObject;
        transform.GetChild(6).gameObject.SetActive(true);
         
        if (devMode)
        {
            MainCam.gameObject.SetActive(true);
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) obj.GetComponent<Enemy>().gameManager = this;

            GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

            Party = new List<Character>();
            
            for (int i = 0; i < objs.Length; i++)
            {
                Party.Add(objs[i].GetComponent<Character>());
                if (objs[i].GetComponent<Player>() != null)
                {
                    objs[i].GetComponent<Player>().gameManager = this;
                    player = objs[i].GetComponent<Player>();
                }
            }
        }
    }

    public void StartGame ()
    {
        StartCoroutine(LaunchGame());
    }



    public IEnumerator StartBattle (Enemy enemy)
    {
        previousPos = new Vector3[Party.Count];
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<Animator>().Play("APPEAR");
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        Camera transitionCamera = transform.GetChild(1).GetChild(0).GetComponent<Camera>();

        transitionCamera.targetTexture = new RenderTexture( Screen.width, Screen.height, 24 );
        rend = transitionCamera.targetTexture;
        transitionCamera.Render();
        print(rend.width.ToString() + " ; " + rend.height.ToString());
        tex = new Texture2D(rend.width, rend.height, TextureFormat.RGBA32, false);
        tex.Apply(false);
        Graphics.CopyTexture(rend, tex);

        cube = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        cube.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", tex);


        transform.position = Vector3.zero;
        currentScene = SceneManager.GetActiveScene();
        battle = true;


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone);
        int _j = 0;
        foreach (Character _char in Party)
        {
            previousPos[_j] = _char.transform.position;
            _j++;
            _char.inBattle = true;
            _char.transform.GetChild(1).gameObject.SetActive(true);
            _char.transform.GetChild(2).gameObject.SetActive(true);
            _char.transform.parent = null;
            SceneManager.MoveGameObjectToScene(_char.gameObject, SceneManager.GetSceneByName("Battle"));
        }
        enemy.transform.parent = null;
        enemy.GetComponent<Character>().inBattle = true;
        enemy.GetComponent<EnemyMovement>().enabled = false;
        SceneManager.MoveGameObjectToScene(enemy.gameObject, SceneManager.GetSceneByName("Battle"));
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Battle"));

        //SceneManager.UnloadSceneAsync(currentScene);
        prev42 = GameObject.FindGameObjectWithTag("42");
        prev42.SetActive(false);

        battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        battleManager.gameManager = this;
        battleManager.Closeup = Closeup;
        battleManager.CloseUpCam = CloseUpCam;
        StartCoroutine(battleManager.StartBattle());

        yield return new WaitUntil(() => battleManager.finishedLoading == true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        transitionCamera.targetTexture = new RenderTexture( Screen.width, Screen.height, 24 );
        rend = transitionCamera.targetTexture;
        transitionCamera.Render();
        //print(rend.width.ToString() + " ; " + rend.height.ToString());
        tex2 = new Texture2D(rend.width, rend.height, TextureFormat.RGBA32, false);
        tex2.Apply(false);
        Graphics.CopyTexture(rend, tex2);
        transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", tex2);
                
        transform.GetChild(0).GetComponent<Animator>().Play("CUBE TRANSITION");

        yield return new WaitForSeconds(1f);
        battleManager.StartStartBattle = true;
    }

    public IEnumerator WonBattle (int xp, int gold)
    {
        foreach (Character _ch in Party)
        {
            _ch.xp += xp;
            if (_ch.xp >= Mathf.Exp(_ch.lvl - 1) * 25)
            {
                //level up stuff
                _ch.lvl++;
            }
        }
        money += gold;


        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<Animator>().Play("APPEAR");
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        Camera transitionCamera = transform.GetChild(1).GetChild(0).GetComponent<Camera>();

        transitionCamera.targetTexture = new RenderTexture( Screen.width, Screen.height, 24 );
        rend = transitionCamera.targetTexture;
        transitionCamera.Render();
        //print(rend.width.ToString() + " ; " + rend.height.ToString());
        tex = new Texture2D(rend.width, rend.height, TextureFormat.RGBA32, false);
        tex.Apply(false);
        Graphics.CopyTexture(rend, tex);

        cube = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        cube.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", tex);


        battle = false;
        SceneManager.SetActiveScene(currentScene);

        int _j = 0;
        foreach (Character _char in Party)
        {
            _char.inBattle = false;
            _char.transform.GetChild(1).gameObject.SetActive(false);
            _char.transform.GetChild(2).gameObject.SetActive(false);
            SceneManager.MoveGameObjectToScene(_char.gameObject, currentScene);
            _char.transform.parent = prev42.transform;
            _char.transform.position = previousPos[_j];
            _j++;
        }
        prev42.SetActive(true);
        battleManager.StopCoroutines();
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Battle"));
        StartCoroutine(CubeEndBattle());
        yield break;
    }

    IEnumerator CubeEndBattle ()
    {
        yield return new WaitForSeconds(.1f);
        Camera transitionCamera = transform.GetChild(1).GetChild(0).GetComponent<Camera>();
        yield return new WaitForEndOfFrame();
        transitionCamera.targetTexture = null;
        rend = new RenderTexture( Screen.width, Screen.height, 24 );
        yield return new WaitForEndOfFrame();
        transitionCamera.targetTexture = rend;
        yield return new WaitForEndOfFrame();
        transitionCamera.Render();
        tex2 = new Texture2D(rend.width, rend.height, TextureFormat.RGBA32, false);
        tex2.Apply(false);
        Graphics.CopyTexture(rend, tex2);
        yield return new WaitForEndOfFrame();
        transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", tex2);
        yield return new WaitForEndOfFrame();
        transform.GetChild(0).GetComponent<Animator>().Play("CUBE TRANSITION");
    }

    public IEnumerator LaunchGame ()
    {
        StartCoroutine(LoadScene(0));
        yield return new WaitUntil(() => loadedStartingScene);
        print("loaded");
        MainCam.gameObject.SetActive(true);
        LoadDataFromScene();
        musicPlayer.playMusic(currentSceneData.Music);
        
        
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) obj.GetComponent<Enemy>().gameManager = this;

        

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        Party = new List<Character>();
        
        for (int i = 0; i < objs.Length; i++)
        {
            Party.Add(objs[i].GetComponent<Character>());
            if (objs[i].GetComponent<Player>() != null)
            {
                objs[i].GetComponent<Player>().gameManager = this;
                player = objs[i].GetComponent<Player>();
            }
        }
    }

    public IEnumerator LoadScene (int id)
    {
        id += 2;
        LoadingScreen.SetActive(true);
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);

        while (sceneLoading.progress < 0.89)
        {
            yield return null;
        }

        yield return new WaitForSeconds(5);

        loadedStartingScene = true;
        
        LoadingScreen.SetActive(false);
        sceneLoading.allowSceneActivation = true;
        SceneManager.UnloadSceneAsync(currentScene);
    }

    public void LoadDataFromScene ()
    {
        TextAsset sceneDataJSON = Resources.Load<TextAsset>("Data/Location/" + SceneManager.GetActiveScene().name);
        currentSceneData = JsonUtility.FromJson<SceneData>(sceneDataJSON.text);
        dialogueManager.currentDirectory = currentSceneData.Name;
        dialogueManager.LoadDialogue();
        dialogueManager.InitializeDialogue(currentSceneData.Dialogue);
    }
}
