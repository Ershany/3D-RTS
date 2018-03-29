using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatKey
{

    public static class Warrior
    {
        public static float HEALTH = 20;
        public static int MANA = 6;
        public static int STRENGTH = 8;
        public static int INTELLIGENCE = 2;
        public static int DEXTERITY = 4;

        public static class Growth
        {
            public static float HEALTH = 2.0f;
            public static float MANA = 1.0f;
            public static float STRENGTH = 1.4f;
            public static float INTELLIGENCE = 0.4f;
            public static float DEXTERITY = 0.8f;
        }
    }

    public static class Archer
    {
        public static float HEALTH = 15;
        public static int MANA = 6;
        public static int STRENGTH = 5;
        public static int INTELLIGENCE = 4;
        public static int DEXTERITY = 8;

        public static class Growth
        {
            public static float HEALTH = 1.5f;
            public static float MANA = 1.5f;
            public static float STRENGTH = 1.0f;
            public static float INTELLIGENCE = 0.8f;
            public static float DEXTERITY = 1.6f;
        }
    }

    public static class Mage
    {
        public static float HEALTH = 10;
        public static int MANA = 12;
        public static int STRENGTH = 4;
        public static int INTELLIGENCE = 8;
        public static int DEXTERITY = 5;
        public static class Growth
        {
            public static float HEALTH = 1.0f;
            public static float MANA = 3.0f;
            public static float STRENGTH = 0.5f;
            public static float INTELLIGENCE = 1.8f;
            public static float DEXTERITY = 0.8f;


        }
    }


    public static void GetStats(string c, ref float hp, ref int mp, ref int str, ref int intel, ref int dex,
                                ref float hpGrowth, ref float manaGrowth, ref float strGrowth, ref float intelGrowth, ref float dexGrowth)
    {

        switch (c)
        {
            case "Warrior":
            case "EnemyWarrior":
                hp = Warrior.HEALTH;
                mp = Warrior.MANA;
                str = Warrior.STRENGTH;
                intel = Warrior.INTELLIGENCE;
                dex = Warrior.DEXTERITY;
                hpGrowth = Warrior.Growth.HEALTH;
                manaGrowth = Warrior.Growth.MANA;
                strGrowth = Warrior.Growth.STRENGTH;
                intelGrowth = Warrior.Growth.INTELLIGENCE;
                dexGrowth = Warrior.Growth.DEXTERITY;
                break;
            case "Archer":
            case "EnemyArcher":
                hp = Archer.HEALTH;
                mp = Archer.MANA;
                str = Archer.STRENGTH;
                intel = Archer.INTELLIGENCE;
                dex = Archer.DEXTERITY;
                hpGrowth = Archer.Growth.HEALTH;
                manaGrowth = Archer.Growth.MANA;
                strGrowth = Archer.Growth.STRENGTH;
                intelGrowth = Archer.Growth.INTELLIGENCE;
                dexGrowth = Archer.Growth.DEXTERITY;
                break;
            case "Mage":
            case "EnemyMage":
                hp = Mage.HEALTH;
                mp = Mage.MANA;
                str = Mage.STRENGTH;
                intel = Mage.INTELLIGENCE;
                dex = Mage.DEXTERITY;
                hpGrowth = Mage.Growth.HEALTH;
                manaGrowth = Mage.Growth.MANA;
                strGrowth = Mage.Growth.STRENGTH;
                intelGrowth = Mage.Growth.INTELLIGENCE;
                dexGrowth = Mage.Growth.DEXTERITY;
                break;
        }
    }
}
