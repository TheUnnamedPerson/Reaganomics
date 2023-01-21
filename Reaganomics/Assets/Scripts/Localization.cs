using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Localization
{
    public static AttackLocalization[] Attacks = {
        new AttackLocalization("Swing", "Deals Melee Damage Using Your Melee Weapon"),
        new AttackLocalization("Gun", "Shoots an Enemy using Your Gun"),
        new AttackLocalization("HOMERUN!", "Deals a Large Amount of Damage with a High Chance to Miss"),
        new AttackLocalization("Shootout", "Does Many Attacks that do Little Damage"),
        new AttackLocalization("Weeb shit", "Try to Dual Wield Sords, High Damage but Low Hit Rate"),
        new AttackLocalization("Keyboard Spam", "Inflicts a Random Debuff on a Random Number of Enemies of Your Choice"),
        new AttackLocalization("Check Again", "Reflects Next Attack for 5 Turns"),
        new AttackLocalization("Short Term Memory", "Reflects Next Attack for 5 Turns"),
        new AttackLocalization("Slideswipe", "Slides and Swipes at the Enemy Potentialy Dealing Large Damage for a Higher Miss Chance"),
        new AttackLocalization("The Stanley Walker Effect™", "Does the Last Attack that an Ally Did"),
        new AttackLocalization("Top Deck", "Hits Enemy With Many Attacks that Do Different Things Depeending on Equiped Card"),
    };
    public static EffectLocalization[] Effects = {
        new EffectLocalization("Empty", "This Shouldnt Be Here"),
        new EffectLocalization("Empty", "This Shouldnt Be Here"),
        new EffectLocalization("POW Change", "Increased POW"),
        new EffectLocalization("MAG Change", "Increased POW"),
        new EffectLocalization("DEF Change", "Increased POW"),
        new EffectLocalization("SPEED Change", "Increased POW"),
        new EffectLocalization("LUCK Change", "Increased POW"),
        new EffectLocalization("RES Change", "Increased POW"),
        new EffectLocalization("INT Change", "Increased POW"),
        new EffectLocalization("CHA Change", "Increased POW"),
        new EffectLocalization("PER Change", "Increased POW"),
        new EffectLocalization("BOLD Change", "Increased POW"),
        new EffectLocalization("KIND Change", "Increased POW"),
        new EffectLocalization("FRUGAL Change", "Increased POW"),
        new EffectLocalization("Regeneration", "Regenerates HP Each Turn"),
        new EffectLocalization("Recovery", "Recovers MP Each Turn"),
        new EffectLocalization("Madness", "-50% Damage, Random Attack is Chosen, Random Target from Either Side Is Chosen"),
        new EffectLocalization("Enraged", "Increased POW"),
        new EffectLocalization("Addiction", "Increased POW"),
        new EffectLocalization("Reflection", "Increased POW"),
        new EffectLocalization("Poison", "Deals % Damage Every Turn (Can't Kill)"),
        new EffectLocalization("ON FIRE!", "Deals Constant Damage Every Turn (Can Kill)"),
    };

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
        if (number < 1) return string.Empty;            
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900); 
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);            
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new ArgumentOutOfRangeException("something bad happened");
    }
}

public class AttackLocalization
{
    public string Name = "";
    public string Description = "";
    public AttackLocalization ()
    {
        Name = "";
        Description = "";
    }
    public AttackLocalization (string _name, string _desc)
    {
        Name = _name;
        Description = _desc;
    }
}

public class EffectLocalization
{
    public string Name = "";
    public string Description = "";
    public EffectLocalization ()
    {
        Name = "";
        Description = "";
    }
    public EffectLocalization (string _name, string _desc)
    {
        Name = _name;
        Description = _desc;
    }
}
