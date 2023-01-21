using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attacks
{
    public static Attack[] attacks = {
        new Attack(1, 1,  1, 0.25f,     1.5f,   0,      0f,     Vector3Int.zero, //Strike
            0,    0,    0,  0,  800,    -.2f,  0,       1),

        new Attack(2, 1,  1, 0.05f,     6f,     0,      0f,     Vector3Int.zero, //Gun
            0,    0,    0,  0,  500,    0f,      0,     2),

        new Attack(1, 1,  1, .25f,      2.5f,   0,      0f,     Vector3Int.zero, //Homerun
            0,    0,    0,  0,  800,    0f,      0,     1),

        new Attack(2, 5,  1, .05f,      1f,     0,      0f,     Vector3Int.zero, //Shootout
            0,    0,    0,  0,  500,    0f,     0,      2),

        new Attack(1, 2,  1, 0.1875f,   1.25f,  0,      0f,     Vector3Int.zero, //Weebshit
            0,    0,    0,  0,  500,    0f,     0,      3),

        new Attack(0, 1,  7, 0,         0f,     0f,     0f,     new Vector3Int(-7, 1, 2), //Keyboard spam
            0,    0,    0,  0,  100,  0.1f,     0,      4),

        new Attack(0, 1, -1, 0,         0f,     0f,     0f,     new Vector3Int(19, 1, 5), //Check Again
            0,    0,    0,  0,    0,    0f,     0,      5),

        new Attack(0, 1, -1, 0,         0f,     0f,     0f,     new Vector3Int(-1, 0, 0), //Short Term Memory
            0,    0,    0,  0,    0,    0f,     0,      6),
        
        new Attack(1, 1,  1, 0.375f,   2.5f,  0,      0f,     Vector3Int.zero, //Slideswipe
            0,    0,    0,  0,  600,    0f,     0,      3),
        
        new Attack(0, 0, 0, 0,         0f,     0f,     0f,     new Vector3Int(0, 0, 0), //The Stanley Walker Effect™
            0,    0,    0,  0,    0,    0f,     0,      0),

        new Attack(1, 10,  1, 0.375f,   0.5f,  0,      0f,     Vector3Int.zero, //Slideswipe
            0,    0,    0,  0,  600,    0f,     0,      7),
        
    };
}

public class Attack
{
    public int element = 0;
    public int hitCount = 1; //Number of times it hits each target
    public int areaOfEffect = 0; //1 is single target, 2-5 is 2-5 targets, 7 is random, negative is same but for teammates
    //TotalDamage = (powDamage * POW + powOffset + magDamage * MAG + magOffset + hpSteal * HP) - (Target Defence * (1 - armorPiece))
    public float powDamage = 0; //Multiply Times Pow
    public float powOffset = 0; //Base Pow Damage
    public float magDamage = 0; //Multiply Times Mag
    public float magOffset = 0; //Base Mag Damage
    public Vector3Int StatusEffect = Vector3Int.zero; //Status Effect
    public int mpCost = 0; //MP Cost
    public int hpCost = 0; //HP Cost
    public float mpSteal = 0; //% mp stolen (0.10 = 10%) If negative then it just gives doesnt cost
    public float hpSteal = 0; //% hp stolen (0.10 = 10%) If negative then it just gives doesnt cost
    public float missChance = 0; //% chance to miss divided by perception (10 = 10%)
    public float missOffset = 0; //% to offset (0.10 = 10%)
    public float armorPierce = 0; //% of Defence to Pierce (0.10 = 10%)
    public int animationID = 0; //id 0 is no animation

    public Attack ()
    {

    }
    
    public Attack (int el, int n, int aoe, float pd, float po, float md, float mo, Vector3Int se, int mp, float ms, float hs, int hp, float mc, float mco, float ap, int ai)
    {
        element = el;
        hitCount = n;
        areaOfEffect = aoe;
        powDamage = pd;
        powOffset = po;
        magDamage = md;
        magOffset = mo;
        StatusEffect = se;
        mpCost = mp;
        hpCost = hp;
        mpSteal = ms;
        hpSteal = hs;
        missChance = mc;
        missOffset = mco;
        armorPierce = ap;
        animationID = ai;
    }
    
}
