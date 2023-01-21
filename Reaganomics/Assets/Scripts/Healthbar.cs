using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public Character _char;
    public Transform _green;
    public bool mp = false;
    public float hp;
    public float maxHP;

    void OnEnable ()
    {
        _char = transform.parent.GetComponent<Character>();
        _green = transform.GetChild(1);
        UpdateValues();
        transform.GetChild(0).gameObject.SetActive(true);
        _green.gameObject.SetActive(true);
    }

    void OnDisable ()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        _green.gameObject.SetActive(false);
    }

    void Start ()
    {
        _char = transform.parent.GetComponent<Character>();
        _green = transform.GetChild(1);
        hp = (mp) ? _char.MP : _char.HP;
        maxHP = (mp) ? _char.maxMP: _char.maxHP;
    }

    void UpdateValues ()
    {
        hp = (mp) ? _char.MP : _char.HP;
        maxHP = (mp) ? _char.maxMP: _char.maxHP;
        _green.localScale = new Vector3(Mathf.Clamp(hp / maxHP * 1.375f, 0f, 1.375f), 0.1875f, 1f);
        transform.localPosition = new Vector3(0f, (mp) ? -.65f : -.3f, -0.5f);
        _green.localPosition = new Vector3(-1 * (1.375f - _green.transform.localScale.x) / 2, 0, -0.5f);
    }

    void Update()
    {
        UpdateValues();
    }
}
