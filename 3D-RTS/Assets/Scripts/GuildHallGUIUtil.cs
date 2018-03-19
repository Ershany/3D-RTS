using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GuildHallGUIUtil : MonoBehaviour
{
    public GameObject gameObject;

    //Party Creation Window
    public  List<GameObject> groupMembers;
    public  List<Button> groupMembersButtons;
    public GameObject deployButton;

    //Roster Window
    public GameObject rosterUnitsPanel;
    public List<GameObject> rosterPanels;
    public List<Button> rosterPanelsButtons;
    public GameObject rosterStatusWindow;
    public GameObject addToPartyButton;




    public GameObject recruitStatusWindow;
    public GameObject recruitButton;
    public GameObject goldCostPanel;

    public GameObject newUnitSelectPanel;
    public GameObject newWarriorButton;
    public GameObject newMageButton;
    public GameObject newArcherButton;


    public GameObject recruitUnitButton;
    public GameObject exitButton;


    public GuildHallController guildCon;
    public StandardGUIController stdGUICon;


    public GuildHallGUIUtil(GameObject gui, GuildHallController gh, StandardGUIController sGC)
    {
        gameObject = gui;
        guildCon = gh;
        stdGUICon = sGC;

        deployButton = gui.transform.Find("DeploymentPanel").Find("GroupCreation").Find("PartyCreationButtons").Find("DeployButton").gameObject;

        groupMembers = new List<GameObject>();
        groupMembersButtons = new List<Button>();
        rosterPanels = new List<GameObject>();
        rosterPanelsButtons = new List<Button>();
        for (int i = 0; i < 4; i++)
        {
            groupMembers.Add(gui.transform.Find("DeploymentPanel").Find("GroupCreation").Find("GroupCreationWindow").Find("CreateGroup").Find("NewGroupMember" + (i + 1)).gameObject);
            groupMembersButtons.Add(groupMembers[i].transform.Find("Button").GetComponent<Button>());
        }

        rosterUnitsPanel = gui.transform.Find("DeploymentPanel").Find("GroupCreation").Find("RosterInformationPanel").Find("Roster").Find("RosterUnits").Find("Viewport").Find("Content").gameObject;
        rosterStatusWindow = gui.transform.Find("DeploymentPanel").Find("GroupCreation").Find("RosterInformationPanel").Find("StatusWindow").gameObject;
        recruitStatusWindow = gui.transform.Find("RecruitPanel").Find("PreviewPanel").Find("StatusWindow").gameObject;

        recruitUnitButton = gui.transform.Find("EnlistUnitButtonPanel").Find("Button").gameObject;

        recruitButton = gui.transform.Find("RecruitPanel").Find("RecruitButtonPanel").Find("RecruitButton").gameObject;
        goldCostPanel = gui.transform.Find("RecruitPanel").Find("RecruitButtonPanel").Find("RecruitButton").Find("GoldCost").gameObject;
        
        newUnitSelectPanel = gui.transform.Find("RecruitPanel").Find("NewUnitSelect").gameObject;

        newWarriorButton = gui.transform.Find("RecruitPanel").Find("NewUnitSelect").Find("Warrior").gameObject;
        newMageButton = gui.transform.Find("RecruitPanel").Find("NewUnitSelect").Find("Mage").gameObject;
        newArcherButton = gui.transform.Find("RecruitPanel").Find("NewUnitSelect").Find("Archer").gameObject;

        exitButton = gui.transform.Find("ExitButton").Find("Button").gameObject;
    }
    public void PartyCreationWindowMemberClicked(int i)
    {
        guildCon.RemoveFromDeployedUnits(i);
    }



    public void UpdateRosterUnitsPanel()
    {
        bool rosterUpdated = false;
        for(int i = 0; i < guildCon.roster.Count; i++)
        {
            if (i < rosterPanels.Count)
            {
                if (rosterPanels[i].transform.Find("TextValues").Find("Name").GetComponent<Text>().text != guildCon.roster[i].GetName())
                {
                    rosterUpdated = true;
                    GameObject temp = rosterPanels[i];
                    rosterPanels[i] = RosterPanelConstructor(guildCon.roster[i], i);
                    rosterPanels[i].transform.SetParent(rosterUnitsPanel.transform);
                    Destroy(temp);
                }
            }
            else
            {
                rosterUpdated = true;
                rosterPanels.Add(RosterPanelConstructor(guildCon.roster[i], i));
                rosterPanels[i].transform.SetParent(rosterUnitsPanel.transform);
            }
        }
        for (int i = guildCon.roster.Count; i < rosterPanels.Count; i++)
        {
            Destroy(rosterPanels[i]);
        }

    }

    public void UpdatePartyCreationPanel()
    {

        for (int i = 0; i < 4; i++)
        {
            if ( i < guildCon.unitsToBeDeployed.Count)
            {
                groupMembers[i].SetActive(true);
                if (groupMembers[i].transform.Find("TextValues").Find("Name").GetComponent<Text>().text != guildCon.unitsToBeDeployed[i].GetName())
                {
                    stdGUICon.PopulateMemberInfoPanel(groupMembers[i], guildCon.unitsToBeDeployed[i]);
                }
            }
            else
            {
                groupMembers[i].SetActive(false);
            }

        }

    }



    public GameObject RosterPanelConstructor(DynamicUnit rosterMember, int i)
    {

        GameObject rosterPanel = Instantiate(stdGUICon.rosterUnitPrefab);

        rosterPanel.transform.Find("TextValues").Find("Name").GetComponent<Text>().text = rosterMember.GetName();
        rosterPanel.transform.Find("TextValues").Find("Class").GetComponent<Text>().text = rosterMember.GetClassName();
        rosterPanel.transform.Find("TextValues").Find("Level").Find("LevelValue").GetComponent<Text>().text = rosterMember.GetLevel().ToString();



        return rosterPanel;
    }

}
