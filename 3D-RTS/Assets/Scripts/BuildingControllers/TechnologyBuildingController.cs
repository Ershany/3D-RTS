using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyBuildingController : MonoBehaviour
{
    //general stat buff controller 
    //Stats
    [Range(1.0f, 1000.0f)] public float buildingHealth;

    public Building building;
    private GameController gameController;
    private PlayerController playerController;

    private float healthBuff;
    private int manaBuff;
    private int strengthBuff;
    private int intelligenceBuff;
    private int dexterityBuff;

    private string healthBuffName;
    private string manaBuffName;
    private string strengthBuffName;
    private string intelligenceBuffName;
    private string dexterityBuffName;

    private string affectedUnit;

    // Use this for initialization
    void Awake()
    {
        //references to player and gameController
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        //setup general values
        buildingHealth = 200.0f;
        healthBuff = 0.0f;
        manaBuff = 0;
        strengthBuff = 0;
        intelligenceBuff = 0;
        dexterityBuff = 0;

        healthBuffName = "";
        manaBuffName = "";
        strengthBuffName = "";
        intelligenceBuffName = "";
        dexterityBuffName = "";
        affectedUnit = "";
    }

    public void AddBuff(string buffName)
    {
        if (buffName == healthBuffName)
        {
            BuffHealth();
        }
        else if (buffName == strengthBuffName)
        {
            BuffStrength();
        }
        else if (buffName == manaBuffName)
        {
            BuffMana();
        }
        else if (buffName == intelligenceBuffName)
        {
            BuffIntelligence();
        }
        else if (buffName == dexterityBuffName)
        {
            BuffDexterity();
        }
    }
     
    //we can optimize this later
    //gets all the units that are warriors since blacksmiths only buff warriors
    public List<DynamicUnit> FindUnitsToBuff(List<Group> units)
    {
        List<DynamicUnit> myUnits = new List<DynamicUnit>();

        for (int i = 0; i < units.Count; i++)
        {
            for (int j = 0; j < units[i].GetUnits().Count; j++)
            {
                //might need to change that later dunno what it is
                if (units[i].GetUnits()[j].GetClassName() == affectedUnit)
                {
                    myUnits.Add(units[i].GetUnits()[j]);
                }
            }
        }

        return myUnits;
    }

    //Buff health of warriors
    public void BuffHealth()
    {
        //get units to buff (swordsmen for the moment)
        List<DynamicUnit> unitsToBuff = FindUnitsToBuff(playerController.groups);

        if (affectedUnit == "Warrior")
        {
            StatKey.Warrior.HEALTH += healthBuff;
        }
        else if (affectedUnit == "Archer")
        {
            StatKey.Archer.HEALTH += healthBuff;
        }
        else if (affectedUnit == "Mage")
        {
            StatKey.Mage.HEALTH += healthBuff;
        }

        for (int i = 0; i < unitsToBuff.Count; i++)
        {
            unitsToBuff[i].PermaBuffMaxHealth(healthBuff);
        }
    }

    //buff mana of warriors
    public void BuffMana()
    {
        //get units to buff (swordsmen for the moment)
        List<DynamicUnit> unitsToBuff = FindUnitsToBuff(playerController.groups);

        if (affectedUnit == "Warrior")
        {
            StatKey.Warrior.MANA += manaBuff;
        }
        else if (affectedUnit == "Archer")
        {
            StatKey.Archer.MANA += manaBuff;
        }
        else if (affectedUnit == "Mage")
        {
            StatKey.Mage.MANA += manaBuff;
        }

        for (int i = 0; i < unitsToBuff.Count; i++)
        {
            unitsToBuff[i].PremaBuffMaxMana(manaBuff);
        }
    }

    //buff strength of warriors
    public void BuffStrength()
    {
        //get units to buff (swordsmen for the moment)
        List<DynamicUnit> unitsToBuff = FindUnitsToBuff(playerController.groups);

        if (affectedUnit == "Warrior")
        {
            StatKey.Warrior.STRENGTH += strengthBuff;
        }
        else if (affectedUnit == "Archer")
        {
            StatKey.Archer.STRENGTH += strengthBuff;
        }
        else if (affectedUnit == "Mage")
        {
            StatKey.Mage.STRENGTH += strengthBuff;
        }

        for (int i = 0; i < unitsToBuff.Count; i++)
        {
            unitsToBuff[i].PermaBuffMaxStrength(strengthBuff);
        }
    }

    //buff intelligence of warriors
    public void BuffIntelligence()
    {
        //get units to buff (swordsmen for the moment)
        List<DynamicUnit> unitsToBuff = FindUnitsToBuff(playerController.groups);

        if (affectedUnit == "Warrior")
        {
            StatKey.Warrior.INTELLIGENCE += intelligenceBuff;
        }
        else if (affectedUnit == "Archer")
        {
            StatKey.Archer.INTELLIGENCE += intelligenceBuff;
        }
        else if (affectedUnit == "Mage")
        {
            StatKey.Mage.INTELLIGENCE += intelligenceBuff;
        }

        for (int i = 0; i < unitsToBuff.Count; i++)
        {
            unitsToBuff[i].PermaBuffMaxIntelligence(intelligenceBuff);
        }
    }

    //buff dexterity of warriors
    public void BuffDexterity()
    {
        //get units to buff (swordsmen for the moment)
        List<DynamicUnit> unitsToBuff = FindUnitsToBuff(playerController.groups);

        if (affectedUnit == "Warrior")
        {
            StatKey.Warrior.DEXTERITY += dexterityBuff;
        }
        else if (affectedUnit == "Archer")
        {
            StatKey.Archer.DEXTERITY += dexterityBuff;
        }
        else if (affectedUnit == "Mage")
        {
            StatKey.Mage.DEXTERITY += dexterityBuff;
        }

        for (int i = 0; i < unitsToBuff.Count; i++)
        {
            unitsToBuff[i].PermaBuffMaxDexterity(dexterityBuff);
        }
    }

    //this should be called when u create a buff building
    public void SetupBuilding(List<string> upgradeNames , List<int> buffs , string affectedUnit , float health , string name , bool isPlayer)
    {
        //setup technologies
        healthBuffName = upgradeNames[0];
        strengthBuffName = upgradeNames[1];
        manaBuffName = upgradeNames[2];
        intelligenceBuffName = upgradeNames[3];
        dexterityBuffName = upgradeNames[4];

        //setup buff values
        healthBuff = buffs[0];
        strengthBuff = buffs[1];
        manaBuff = buffs[2];
        intelligenceBuff = buffs[3];
        dexterityBuff = buffs[4];

        //setup unit affected and building 
        this.affectedUnit = affectedUnit;
        buildingHealth = health;
        building = new Building(gameObject , buildingHealth , name , isPlayer);
    }
}
