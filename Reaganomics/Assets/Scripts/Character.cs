using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string Name = "";

    [Header("Combat Stats")]
    public int maxHP = 20;
    public int maxMP = 20;
    public int HP = 20;
    public int MP = 20;
    public int POW = 20;
    public int MAG = 20;
    public int DEF = 20;
    public int SPEED = 20;
    public int LUCK = 20;

    [Header("Other Stats")]
    public int RES = 20;
    public int INT = 20;
    public int CHA = 20;
    public int PER = 20;
    public int BOLD = 20;
    public int KIND = 20;
    public int FRUGAL = 20;

    public int[] Elements = new int[3];
    public Vector2 WeaponTypes;
    public int xp = 0;
    public int lvl = 1;
    public int money = 0;
    public int reflectIndex = -1;

    public Healthbar healthbar;
    public Healthbar manabar;

    public Dictionary<string, string> Weapon1 = new Dictionary<string, string>
    {
        {"Name", ""},
        {"Description", ""},
        {"Stat ID", ""},
        {"Magnitude", ""},
        {"Type", ""}
    };

    public Dictionary<string, string> Weapon2 = new Dictionary<string, string>
    {
        {"Name", ""},
        {"Description", ""},
        {"Stat ID", ""},
        {"Magnitude", ""},
        {"Type", ""}
    };

    public bool inBattle = false;

    public bool Player = false;
    public bool partyLeader = false;

    public List<Vector3Int> StatusEffects; //x is id, y is magnitude, z is length
    public Dictionary<string, string> Helmet = new Dictionary<string, string>
    {
        {"Name", ""},
        {"Description", ""},
        {"Stat ID", ""},
        {"Magnitude", ""},
        {"Type", ""}
    };

    public Dictionary<string, string> Armor = new Dictionary<string, string>
    {
        {"Name", ""},
        {"Description", ""},
        {"Stat ID", ""},
        {"Magnitude", ""},
        {"Type", ""}
    };

    public Dictionary<string, string> Boots = new Dictionary<string, string>
    {
        {"Name", ""},
        {"Description", ""},
        {"Stat ID", ""},
        {"Magnitude", ""},
        {"Type", ""}
    };

    public Dictionary<string, string>[] Accessories = 
    {
        new Dictionary<string, string>
        {
            {"Name", ""},
            {"Description", ""},
            {"Stat ID", ""},
            {"Magnitude", ""},
            {"Type", ""}
        },
        new Dictionary<string, string>
        {
            {"Name", ""},
            {"Description", ""},
            {"Stat ID", ""},
            {"Magnitude", ""},
            {"Type", ""}
        },
        new Dictionary<string, string>
        {
            {"Name", ""},
            {"Description", ""},
            {"Stat ID", ""},
            {"Magnitude", ""},
            {"Type", ""}
        },
    };

    public int[] Attacks = new int[4];

    public bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = transform.GetChild(1).GetComponent<Healthbar>();
        healthbar.gameObject.SetActive(false);
        manabar = transform.GetChild(2).GetComponent<Healthbar>();
        manabar.gameObject.SetActive(false);
        //if (gameObject.tag == "Player") GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    public int getPOW ()
    {
        int pow = POW;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 2) pow += status.y;
        }
        if (pow <= 0) pow = 1;
        return pow;
    }

    public int getMAG ()
    {
        int mag = MAG;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 3) mag += status.y;
        }
        if (mag <= 0) mag = 1;
        return mag;
    }

    public int getDEF ()
    {
        int def = DEF;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 4) def += status.y;
        }
        if (def <= 0) def = 1;
        return def;
    }

    public int getSPEED ()
    {
        int speed = SPEED;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 5) speed += status.y;
        }
        if (speed <= 0) speed = 1;
        return speed;
    }

    public int getLUCK ()
    {
        int luck = LUCK;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 6) luck += status.y;
        }
        if (luck <= 0) luck = 1;
        return luck;
    }

    public int getRES ()
    {
        int res = RES;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 7) res += status.y;
        }
        if (res <= 0) res = 1;
        return res;
    }

    public int getINT ()
    {
        int _int = INT;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 8) _int += status.y;
        }
        if (_int <= 0) _int = 1;
        return _int;
    }

    public int getCHA ()
    {
        int cha = CHA;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 9) cha += status.y;
        }
        if (cha <= 0) cha = 1;
        return cha;
    }

    public int getPER ()
    {
        int per = PER;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 10) per += status.y;
        }
        if (per <= 0) per = 1;
        return per;
    }

    public int getBOLD ()
    {
        int bold = BOLD;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 11) bold += status.y;
        }
        if (bold <= 0) bold = 1;
        return bold;
    }

    public int getKIND ()
    {
        int kind = KIND;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 12) kind += status.y;
        }
        if (kind <= 0) kind = 1;
        return kind;
    }

    public int getFRUGAL ()
    {
        int frugal = FRUGAL;
        foreach (Vector3Int status in StatusEffects)
        {
            if (status.x == 13) frugal += status.y;
        }
        if (frugal <= 0) frugal = 1;
        return frugal;
    }

    public void addStatusEffect (Vector3Int effect)
    {
        if (effect == Vector3Int.zero) return;
        bool present = false;
        for (int i = 0; i < StatusEffects.Count; i++)
        {
            if (StatusEffects[i].x == effect.x)
            {
                present = true;
                if (i == 19) reflectIndex = i;
                if (StatusEffects[i].y <= effect.y)
                {
                    StatusEffects[i] = new Vector3Int(effect.x, effect.y, (effect.z > StatusEffects[i].z) ? effect.z : StatusEffects[i].z);
                }
            }
        }
        if (!present)
        {
            StatusEffects.Add(effect);
            if (effect.x == 19) reflectIndex = StatusEffects.Count - 1;
        }
    }

    public void tickStatuses (int ticks = 1)
    {
        for (int ii = 1; ii <= ticks; ii++)
        {
            List<Vector3Int> statusEffects = StatusEffects;
            StatusEffects = new List<Vector3Int>();
            for (int i = 0; i < statusEffects.Count; i++)
            {
                if (statusEffects[i].z >= 1) {
                    statusEffects[i] = new Vector3Int(statusEffects[i].x, statusEffects[i].y, statusEffects[i].z - 1);
                }
                if (statusEffects[i].z != 0)
                {
                    StatusEffects.Add(statusEffects[i]);
                    if (statusEffects[i].x == 19) reflectIndex = StatusEffects.Count - 1;
                }
                else
                {
                    if (statusEffects[i].x == 19) reflectIndex = -1;
                }
            }
        }
    }

    public void updateSatuses ()
    {
        List<Vector3Int> statusEffects = StatusEffects;
        StatusEffects = new List<Vector3Int>();
        for (int i = 0; i < statusEffects.Count; i++)
        {
            if (statusEffects[i].z > 1) {
                statusEffects[i] = new Vector3Int(statusEffects[i].x, statusEffects[i].y, statusEffects[i].z);
            }
            if (statusEffects[i].y != 0 && statusEffects[i].z != 0)
            {
                StatusEffects.Add(statusEffects[i]);
                if (statusEffects[i].x == 19) reflectIndex = StatusEffects.Count - 1;
            }
            else
            {
                if (statusEffects[i].x == 19) reflectIndex = -1;
            }
        }
    }

}
