using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public List<Character> party;
    public List<Character> enemies;
    List<Character> everyone = new List<Character>();
    public int[] turnOrder;
    public BattleOptions battleOptions;
    public int optionChosen;
    public Transform _selector;
    public bool StartStartBattle = false;
    public Transform animationPrefab;
    public Transform hitmarkerPrefab;
    public List<Transform> animations;
    public int targetChosen;
    public Character[] targets;
    public List<Transform> hitmarkers;
    public int[] targetIndexes;
    public PlayAudio playAudio;
    public int animationsPlaying;
    public int turnNumber = 0;
    bool over1 = true;
    bool over2 = true;
    public GameObject battleOver;
    public TMP_Text xp;
    public TMP_Text money;
    public TMP_Text items;
    public GameManager gameManager;
    public bool finishedLoading = false;
    public Vector3 animPos = new Vector3();

    public Camera CloseUpCam;
    public GameObject Closeup;
    public Attack lastAttack = new Attack();
    public int pCI;
    public int eCI;

    public IEnumerator StartBattle() {

        Enemy enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        foreach (Transform _en in enemy.enemies)
        {
            Instantiate(_en, null);
        }

        GameObject __en = enemy.gameObject;

        Destroy(__en);

        yield return new WaitForFixedUpdate();

        playAudio = GetComponentInChildren<PlayAudio>();
        
        GameObject[] objs1 = GameObject.FindGameObjectsWithTag("Player");
        objs1.Reverse();
        GameObject[] objs2 = GameObject.FindGameObjectsWithTag("Enemy");
        objs2.Reverse();

        party = new List<Character>();
        enemies = new List<Character>();
        
        for (int i = 0; i < objs1.Length; i++)
        {
            objs1[i].GetComponentInChildren<SpriteRenderer>().flipX = false;
            party.Add(objs1[i].GetComponent<Character>());
            everyone.Add(objs1[i].GetComponent<Character>());
        }
        for (int i = 0; i < objs2.Length; i++)
        {
            objs2[i].GetComponentInChildren<SpriteRenderer>().flipX = true;
            enemies.Add(objs2[i].GetComponent<Character>());
            everyone.Add(objs2[i].GetComponent<Character>());
        }

        yield return new WaitForEndOfFrame();
        foreach (Character _char in everyone)
        {
            _char.inBattle = true;
            _char.transform.GetChild(1).gameObject.SetActive(true);
            _char.transform.GetChild(2).gameObject.SetActive(true);
        }

        pCI = party.Count;

        switch (party.Count)
        {
            case 0:
                Debug.LogError("Party Has 0");
                break;
            case 1:
                party[0].transform.position = new Vector3(2.5f, -2f + 1.5f, 0);
                break;
            case 2:
                party[0].transform.position = new Vector3(2.5f, -3.5f + 1.5f, 0);
                party[1].transform.position = new Vector3(2.75f, -0.5f + 1.5f, 0);
                break;
            case 3:
                party[0].transform.position = new Vector3(2.5f, -2f + 1.5f, 0);
                party[1].transform.position = new Vector3(4.75f, -0.5f + 1.5f, 0);
                party[2].transform.position = new Vector3(4.75f, -3.5f + 1.5f, 0);
                break;
            case 4:
                party[0].transform.position = new Vector3(2.5f, -2f + 1.5f, 0);
                party[1].transform.position = new Vector3(4.75f, -0.5f + 1.5f, 0);
                party[2].transform.position = new Vector3(4.75f, -3.5f + 1.5f, 0);
                party[3].transform.position = new Vector3(6.75f, -2f + 1.5f, 0);
                break;
            case 5:
                party[0].transform.position = new Vector3(2.5f, -2f + 1.5f, 0);
                party[1].transform.position = new Vector3(3.75f, -0.5f + 1.5f, 0);
                party[2].transform.position = new Vector3(4.5f, -3.5f + 1.5f, 0);
                party[3].transform.position = new Vector3(5.5f, -1f + 1.5f, 0);
                party[4].transform.position = new Vector3(6.75f, -3f + 1.5f, 0);
                break;
        }
        //Debug.Log(party[0].transform.position);

        foreach (Character _enemy in enemies) _enemy.inBattle = true;

        eCI = enemies.Count;

        switch (enemies.Count)
        {
            case 0:
                Debug.LogError("Party Has 0");
                break;
            case 1:
                enemies[0].transform.position = new Vector3(-2.5f, -2f + 1.5f, 0);
                break;
            case 2:
                enemies[0].transform.position = new Vector3(-2.5f, -3.5f + 1.5f, 0);
                enemies[1].transform.position = new Vector3(-2.75f, -0.5f + 1.5f, 0);
                break;
            case 3:
                enemies[0].transform.position = new Vector3(-2.5f, -2f + 1.5f, 0);
                enemies[1].transform.position = new Vector3(-4.75f, -0.5f + 1.5f, 0);
                enemies[2].transform.position = new Vector3(-4.75f, -3.5f + 1.5f, 0);
                break;
            case 4:
                enemies[0].transform.position = new Vector3(-2.5f, -2f + 1.5f, 0);
                enemies[1].transform.position = new Vector3(-4.75f, -0.5f + 1.5f, 0);
                enemies[2].transform.position = new Vector3(-4.75f, -3.5f + 1.5f, 0);
                enemies[3].transform.position = new Vector3(-6.75f, -2f + 1.5f, 0);
                break;
        }

        foreach (Character cahr in everyone) cahr.transform.eulerAngles = Vector3.zero;

        battleOptions.battleManager = this;
        battleOptions.battleLoaded = true;

        DecideTurnOrder();

        

        finishedLoading = true;

        yield return new WaitUntil (() => StartStartBattle);

        

        StartCoroutine(DoTurn(turnNumber));        

        yield return 0;
        
        
    }

    void DecideTurnOrder()
    {
        
        List<int> _tO = new List<int>();
        for (int i = 0; i < everyone.Count; i++)
        {
            if (!everyone[i].dead) _tO.Add(i);
        }
        
        _tO.Sort((x,y) => everyone[x].getSPEED().CompareTo(everyone[y].getSPEED()));

        _tO.Reverse();

        turnOrder = _tO.ToArray();

        List<int> _tO2 = new List<int>();

        foreach (int n in _tO)
        {
            _tO2.Add(n);
        }

        //int speedOffset = 200;

        List<int> intsDone = new List<int>();
        for (int i = 0; i < _tO.Count; i++)
        {
            if (!intsDone.Contains(_tO[i]))
            {
                int repeats = 1;
                for (int ii = i + 1; ii < _tO.Count; ii++)
                {
                    int s = (everyone[_tO[ii]].getSPEED() / everyone[_tO[i]].getSPEED());
                    //if ((speedOffset - everyone[_tO[ii]].getSPEED()) / (speedOffset - everyone[_tO[i]].getSPEED()) >= repeats + 1)
                    if ( 1 / ((s == 0) ? 1 : s) >= repeats + 1)
                    {
                        _tO2.Insert(ii + repeats - 1, _tO[i]);
                        repeats++;
                    }
                }
                intsDone.Add(_tO[i]);
                _tO = new List<int>();
                foreach (int n in _tO2)
                {
                    _tO.Add(n);
                }
            }
        }

        turnOrder = _tO.ToArray();
        
    }

    IEnumerator DoTurn (int index)
    {
        
        Character _char = everyone[turnOrder[index]];
        if (!_char.Player) yield return new WaitUntil(() => StartStartBattle);

        if (!_char.dead)
        {
            _char.tickStatuses();
            if (_char.Player == true)
            {
                battleOptions.gameObject.SetActive(true);
                battleOptions.selectedPlayer = _char;
                battleOptions.UpdateOptions();
                battleOptions.newPos = _char.transform.position + Vector3.left * (0 + (battleOptions.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().size.x / 2f)) + Vector3.up + Vector3.back * 50;
                yield return new WaitUntil(() => optionChosen != 0);
                battleOptions.gameObject.SetActive(false);
                switch (optionChosen)
                {
                    case -1:
                    break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        yield return StartCoroutine(doAttack(_char.Attacks[optionChosen - 1], _char, false));
                        break;
                }
            }
            else
            {
                optionChosen = UnityEngine.Random.Range(0,_char.Attacks.Length);
                yield return StartCoroutine(doAttack(_char.Attacks[optionChosen], _char, true));
            }
        }

        optionChosen = 0;

        over1 = true;
        over2 = true;

        foreach (Character c in enemies) if (!c.dead) over1 = false;
        foreach (Character c in party) if (!c.dead) over2 = false;

        if (over1 || over2)
        {
            StartCoroutine(finishBattle());
        }
        else if (index < turnOrder.Length - 1) StartCoroutine(DoTurn(index + 1));
        else
        {
            DecideTurnOrder();
            StartCoroutine(DoTurn(0));
        }
    }

    IEnumerator awaitFinishBattle ()
    {
        yield return new WaitUntil(() => enemies.Count == 0 || party.Count == 0);
        StartCoroutine(finishBattle());
    }

    IEnumerator finishBattle ()
    {
        battleOver.SetActive(true);
        int _xp = 0;
        int _gold = 0;
        foreach (Character _char in everyone)
        {
            if (!_char.Player) _xp += _char.xp;
            if (!_char.Player) _gold += _char.money;
        }
        xp.text = _xp.ToString();
        money.text = _gold.ToString();
        items.text = "None";

        yield return new WaitUntil(() => Input.GetKey(KeyCode.Return));
        StartCoroutine(gameManager.WonBattle(_xp, _gold));
    }

    IEnumerator doAttack (int attackID, Character _char, bool isEnemy)
    {
        Vector3 pos = _char.transform.position;
        Attack attack = Attacks.attacks[attackID];
        if (attackID != 9 && _char.Player) lastAttack = attack;
        else if (attackID == 9) attack = lastAttack;

        int n = 0;
        bool side = false;
        if (attack.areaOfEffect == 7) attack.areaOfEffect = UnityEngine.Random.Range(1,5);
        else if (attack.areaOfEffect == -7) attack.areaOfEffect = UnityEngine.Random.Range(1,5) * -1;

        if (attack.areaOfEffect >= 1 && attack.areaOfEffect <= 5)
        {
            n = attack.areaOfEffect;
            side = false;
        }
        else if (attack.areaOfEffect <= -1 && attack.areaOfEffect >= -5)
        {
            n = -1 * attack.areaOfEffect;
            side = true;
        }

        if (attack.StatusEffect.x == -7) attack.StatusEffect.x = UnityEngine.Random.Range(1,3);

        if (!isEnemy) targets = new Character[Math.Min(n, (side) ? party.Count : enemies.Count)];
        else targets = new Character[Math.Min(n, (!side) ? party.Count : enemies.Count)];
        if (!isEnemy) targetIndexes = new int[Math.Min(n, (side) ? party.Count : enemies.Count)];
        else targetIndexes = new int[Math.Min(n, (!side) ? party.Count : enemies.Count)];
        for (int i = 0; i < targetIndexes.Length; i++) targetIndexes[i] = -1;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!isEnemy) {
                yield return StartCoroutine(SelectTarget(side));
                targets[i] = (side) ? party[targetChosen] : enemies[targetChosen];
            }
            else
            {
                if (!side)
                {
                    targetChosen = UnityEngine.Random.Range(0, party.Count);
                }
                else targetChosen = UnityEngine.Random.Range(0, enemies.Count);

                targets[i] = (!side) ? party[targetChosen] : enemies[targetChosen];
            }
            
            targetIndexes[i] = targetChosen;
        }

        animations = new List<Transform>();

        int Damage = (int)(attack.powDamage * _char.getPOW() + attack.powOffset) + (int)(attack.magDamage * _char.getMAG() + attack.magOffset);

        for (int ii = 0; ii < attack.hitCount; ii++)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (!targets[i].dead)
                {
                    bool missed = false;
                    bool hitOnce = false;
                    int trueDamage = 0;

                    animations.Add(Instantiate(animationPrefab));
                    hitmarkers.Add(Instantiate(hitmarkerPrefab));

                    float r = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (r > ((attack.missChance / _char.getPER()) / 100) + attack.missOffset)
                    {
                        trueDamage = Damage - (int)(targets[i].getDEF() * (1 - attack.armorPierce)) + (int)(targets[i].maxHP * attack.hpSteal);
                        hitOnce = true;
                    }
                    else missed = true;

                    if (trueDamage <= 0) trueDamage = 0;
                    
                    if (targets[i].reflectIndex == -1 || (targets[i].StatusEffects[targets[i].reflectIndex].z == 0))
                    {
                        if (!targets[i].dead)
                        {
                            StartCoroutine(PlayAnimation(attack.animationID, animations.Last(), _char, targets[i], hitmarkers.Last(), trueDamage, missed, ii, attack.StatusEffect));
                        }
                    }
                    else
                    {
                        animations.Last().position = targets[i].transform.position + Vector3.back * 22f + Vector3.left * 3f + Vector3.up * .75f;
                        animations.Last().GetComponentInChildren<Animator>().Play("attack_05");
                        yield return new WaitForSeconds(23f / 60f);
                        animations.Last().GetComponentInChildren<PlayAudio>().playAudio(4);
                        animations.Add(Instantiate(animationPrefab));
                        StartCoroutine(PlayAnimation(attack.animationID, animations.Last(), _char, _char, hitmarkers.Last(), trueDamage, missed, ii, attack.StatusEffect));
                        targets[i].StatusEffects[targets[i].reflectIndex] = new Vector3Int(targets[i].StatusEffects[targets[i].reflectIndex].x, targets[i].StatusEffects[targets[i].reflectIndex].y - 1, targets[i].StatusEffects[targets[i].reflectIndex].z);
                        targets[i].updateSatuses();
                    }

                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
        
       yield return new WaitUntil(() => animationsPlaying <= 0);

        foreach (Transform _t in animations) Destroy(_t.gameObject);
        foreach (Transform _t in hitmarkers) Destroy(_t.gameObject);

        animations = new List<Transform>();
        hitmarkers = new List<Transform>();
        animPos = new Vector3();
        _char.transform.position = pos;
        _char.transform.localEulerAngles = Vector3.zero;

    }

    IEnumerator PlayAnimation (int id, Transform animation, Character attacker, Character target, Transform hitmarker, int trueDamage, bool missed, int extra, Vector3Int effect)
    {
        animationsPlaying++;
        hitmarker.GetComponentInChildren<TMP_Text>().text = "";
        //Vector3 r = new Vector3(UnityEngine.Random.Range(-.25f, .25f), UnityEngine.Random.Range(-.25f, .25f), 0);
        switch (id)
        {
            case 1:
                animation.position = target.transform.position + Vector3.back * 22f + Vector3.left * .025f + Vector3.up * .75f;
                animation.GetComponentInChildren<Animator>().Play("attack_01");
                doDamage(target, trueDamage, effect, hitmarker, missed, animation);
                yield return new WaitForSeconds(0.84f);
                break;
            case 2:
                animation.position = target.transform.position + Vector3.back * 22f + Vector3.right * 3f + Vector3.up * .75f;
                animation.GetComponentInChildren<Animator>().Play("attack_02");
                yield return new WaitForSeconds(0.5f);
                animation.GetComponentInChildren<PlayAudio>().playAudio(1);
                doDamage(target, trueDamage, effect, hitmarker, missed, animation);
                yield return new WaitForSeconds(1);
                break;
            case 3:
                if (extra == 0)
                {
                    animPos = attacker.transform.position;
                    attacker.GetComponentInChildren<Animator>().Play("pattack_03");
                }
                yield return new WaitForSeconds(25f/60f);
                animation.position = target.transform.position + Vector3.back * 22f + Vector3.left * .025f + Vector3.up * .5f;
                if (extra == 1)
                {
                    animation.GetComponentInChildren<Animator>().Play("attack_03_02");
                }
                else
                {
                    attacker.transform.position = new Vector3(-6, 0, animPos.z);
                    animation.GetComponentInChildren<Animator>().Play("attack_03");
                }
                animation.GetComponentInChildren<PlayAudio>().playAudio(2);
                doDamage(target, trueDamage, effect, hitmarker, missed, animation);
                if (extra == 0)
                {
                    yield return new WaitForSeconds(30f/60f);
                    attacker.transform.position = animPos;
                }
                yield return new WaitForSeconds(1);
                break;
            case 4:
                animation.position = target.transform.position + Vector3.back * 22f + Vector3.right * 3f + Vector3.up * .75f;
                animation.GetComponentInChildren<Animator>().Play("attack_04");
                yield return new WaitForSeconds(0.5f);
                animation.GetComponentInChildren<PlayAudio>().playAudio(3);
                doDamage(target, trueDamage, effect, hitmarker, missed, animation, false);
                yield return new WaitForSeconds(3);
                break;
            case 5:
                animation.position = target.transform.position + Vector3.back * 22f + Vector3.left * 3f + Vector3.up * .75f;
                animation.GetComponentInChildren<Animator>().Play("attack_05");
                yield return new WaitForSeconds(23f / 60f);
                animation.GetComponentInChildren<PlayAudio>().playAudio(4);
                doDamage(target, trueDamage, effect, hitmarker, missed, animation, false);
                yield return new WaitForSeconds(100f/60f);
                break;
            case 7:
                if (extra == 1)
                {
                    CloseUpCam.transform.position = attacker.transform.GetChild(0).position + Vector3.back * 25f + Vector3.up * .25f;
                    Closeup.GetComponentInChildren<Animator>().Play("Closeup");
                }
                yield return new WaitForSeconds(75f/60f);
                animation.position = Vector3.up * (UnityEngine.Random.Range(-4f, 4f));
                animation.GetComponentInChildren<Animator>().Play("attack_07");
                yield return new WaitForSeconds(2f / 60f);
                animation.GetChild(0).GetComponentInChildren<SpriteMask>().sprite = animation.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                animation.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(.75f,.75f,.75f,1f);
                //animation.GetComponentInChildren<PlayAudio>().playAudio(2);
                yield return new WaitUntil(() => animation.GetChild(0).position.x <= target.transform.position.x);
                if (!target.dead) doDamage(target, trueDamage, effect, hitmarker, missed, animation);
                yield return new WaitUntil(() => (animation.GetChild(0).position.x <= -10) || !(animation.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_07")));
                Destroy(animation.GetChild(0).gameObject);
                yield return new WaitForSeconds(1f);
                break;
        }
        animationsPlaying--;
        
    }

    void doDamage (Character target, int damage, Vector3Int effect, Transform hitmarker, bool missed, Transform animation, bool doDamage = true)
    {
        if (target.HP - damage <= 0)
        {
            //targets[i].GetComponentInChildren<Animator>().Play("Death");
            target.dead = true;
            target.transform.GetChild(1).gameObject.SetActive(false);
            target.transform.GetChild(2).gameObject.SetActive(false);
            enemies.Remove(target);
            party.Remove(target);
        }
        if (doDamage)
        {
            if (!missed) animation.GetComponentInChildren<PlayAudio>().playAudio(0);
            if (!target.dead) { if (!missed) target.GetComponentInChildren<Animator>().Play("Hurt"); }
            else if (!missed) target.GetComponentInChildren<Animator>().Play("Death");
            hitmarker.transform.position = target.transform.position + new Vector3(UnityEngine.Random.Range(-.25f, .25f), UnityEngine.Random.Range(-.25f, .25f), -1);
            hitmarker.GetComponentInChildren<TMP_Text>().text = damage.ToString();
            if (missed) hitmarker.GetComponentInChildren<TMP_Text>().text = "Missed!";
            hitmarker.GetComponentInChildren<Animator>().Play("marker02");   
        }
        else
        {
            if (missed) hitmarker.transform.position = target.transform.position + new Vector3(UnityEngine.Random.Range(-.25f, .25f), UnityEngine.Random.Range(-.25f, .25f), -1);
            if (missed) hitmarker.GetComponentInChildren<TMP_Text>().text = "Missed!";
        }

        target.HP -= damage;


        if (effect != Vector3Int.zero && !missed)
        {
            target.addStatusEffect(effect);
            hitmarkers.Add(Instantiate(hitmarkerPrefab));
            hitmarkers.Last().transform.position = target.transform.position + new Vector3(UnityEngine.Random.Range(-.05f, .05f), UnityEngine.Random.Range(-.05f, .05f), -1);
            hitmarkers.Last().GetComponentInChildren<TMP_Text>().text = Localization.Effects[effect.x].Name + " " + Localization.ToRoman(effect.y);
            hitmarkers.Last().GetComponentInChildren<Animator>().Play("marker02");
        }
    }

    IEnumerator SelectTarget (bool playerSide = false)
    {
        _selector.gameObject.SetActive(true);
        bool chose = false;
        if (!playerSide)
        {
            while (!chose)
            {
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Return)));
                if (Input.GetKeyDown(KeyCode.Return) && !targetIndexes.Contains(targetChosen))
                {
                    playAudio.playAudio(1);
                    chose = true;
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    playAudio.playAudio(0);
                    targetChosen++;
                }
                else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A))
                {
                    playAudio.playAudio(0);
                    targetChosen--;
                }
                if (targetChosen < 0) targetChosen = enemies.Count - 1;
                if (targetChosen > enemies.Count - 1) targetChosen = 0;
                _selector.position = new Vector3 (0,2,-25) + enemies[targetChosen].transform.position;
            }
        }
        else if (playerSide)
        {
            while (!chose)
            {
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Return)));
                if (Input.GetKeyDown(KeyCode.Return) && !targetIndexes.Contains(targetChosen))
                {
                    playAudio.playAudio(1);
                    chose = true;
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    playAudio.playAudio(0);
                    targetChosen++;
                }
                else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A))
                {
                    playAudio.playAudio(0);
                    targetChosen--;
                }
                if (targetChosen < 0) targetChosen = party.Count - 1;
                if (targetChosen > party.Count - 1) targetChosen = 0;
                _selector.position = new Vector3 (0,2,-25) + party[targetChosen].transform.position;
            }
        }
        _selector.gameObject.SetActive(false);
    }

    public void StopCoroutines ()
    {
        StopAllCoroutines();
    }

}
