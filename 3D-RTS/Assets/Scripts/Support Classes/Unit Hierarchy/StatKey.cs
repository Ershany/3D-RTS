using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatKey {

    public static class Warrior
    {
        public static float HEALTH = 20;
        public static int MANA = 6;
        public static int STRENGTH = 8;
        public static int INTELLIGENCE = 2;
        public static int DEXTERITY = 4;
    }

    public static class Archer
    {
        public static float HEALTH = 15;
        public static int MANA = 6;
        public static int STRENGTH = 5;
        public static int INTELLIGENCE = 4;
        public static int DEXTERITY = 8;
    }

    public static class Mage
    {
        public static float HEALTH = 10;
        public static int MANA = 12;
        public static int STRENGTH = 4;
        public static int INTELLIGENCE = 8;
        public static int DEXTERITY = 5;
    }


    public static void GetStats(string c, ref float hp, ref int mp, ref int str, ref int intel, ref int dex)
    {

        switch (c)
        {
            case "Warrior":
                hp = Warrior.HEALTH;
                mp = Warrior.MANA;
                str = Warrior.STRENGTH;
                intel = Warrior.INTELLIGENCE;
                dex = Warrior.DEXTERITY;
                break;
            case "Archer":
                hp = Archer.HEALTH;
                mp = Archer.MANA;
                str = Archer.STRENGTH;
                intel = Archer.INTELLIGENCE;
                dex = Archer.DEXTERITY;
                break;
            case "Mage":
                hp = Mage.HEALTH;
                mp = Mage.MANA;
                str = Mage.STRENGTH;
                intel = Mage.INTELLIGENCE;
                dex = Mage.DEXTERITY;
                break;
        }
    }
}
