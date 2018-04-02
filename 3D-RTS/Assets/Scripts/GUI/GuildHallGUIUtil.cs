using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GuildHallGUIUtil : MonoBehaviour
{
    delegate void MyDelegate(int num);
    MyDelegate myDelegate;


    public GameObject gameObject;

    //Party Creation Window
    public  List<GameObject> groupMembers;
    public  List<Button> groupMembersButtons;
    public  GameObject deployButton;

    //Roster Window
    public GameObject rosterUnitsPanel;
    public List<GameObject> rosterPanels;
    public List<Button> rosterPanelsButtons;
    public GameObject rosterStatusWindow;
    public GameObject addToPartyButton;



    public GameObject recruitPanel;
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


    public GuildHallGUIUtil(GameObject gui,StandardGUIController sGC)
    {
        gameObject = gui;
        stdGUICon = sGC;
        guildCon = null;
        deployButton = gui.transform.Find("DeploymentPanel").Find("GroupCreation").Find("PartyCreationButtons").Find("DeployButton").gameObject;

        groupMembers = new List<GameObject>();
        groupMembersButtons = new List<Button>();
        rosterPanels = new List<GameObject>();
        rosterPanelsButtons = new List<Button>();
        for (int i = 0; i < 4; i++)
        {
            groupMembers.Add(gui.transform.Find("DeploymentPanel").Find("GroupCreation").Find("GroupCreationWindow").Find("CreateGroup").Find("NewGroupMember" + (i + 1).ToString()).gameObject);
            groupMembersButtons.Add(groupMembers[i].transform.Find("Button").GetComponent<Button>());
        }

        rosterUnitsPanel = gui.transform.Find("DeploymentPanel").Find("RosterInformationPanel").Find("Roster").Find("RosterUnits").Find("Viewport").Find("Content").gameObject;
        rosterStatusWindow = gui.transform.Find("DeploymentPanel").Find("RosterInformationPanel").Find("StatusWindow").gameObject;

        recruitPanel = gui.transform.Find("RecruitPanel").gameObject;


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
    public void ClassSelectedInRecruitPanel()
    {
        if (recruitPanel.transform.Find("NewUnitSelect").Find("Warrior").GetComponent<UnityEngine.UI.Toggle>().isOn)
            guildCon.SetSelectedUnitNum(1);

        else if (recruitPanel.transform.Find("NewUnitSelect").Find("Archer").GetComponent<UnityEngine.UI.Toggle>().isOn)
            guildCon.SetSelectedUnitNum(2);

        else if (recruitPanel.transform.Find("NewUnitSelect").Find("Mage").GetComponent<UnityEngine.UI.Toggle>().isOn)
            guildCon.SetSelectedUnitNum(3);


        UpdateRecruitPanel();


    }
    public void RecruitSelectedUnit()
    {
        if (guildCon.selectedNewUnitNum != -1)
        {
            guildCon.CreateUnit();
        }
    }


    public void RosterUnitSelected(int i)
    {
        guildCon.selectedRosterUnitNum = i;
        stdGUICon.PopulateStatusWindow(guildCon.roster[i], rosterStatusWindow);
    }

    public void RosterUnitSubmitted()
    {
        Debug.Log(guildCon.roster.Count);
        Debug.Log(guildCon.selectedRosterUnitNum);
        if ((guildCon.selectedRosterUnitNum - 1) < guildCon.roster.Count && guildCon.roster.Count > 0 && guildCon.unitsToBeDeployed.Count < 4)
        {
            Destroy(rosterPanels[guildCon.selectedRosterUnitNum]);
            rosterPanels.RemoveAt(guildCon.selectedRosterUnitNum);
            guildCon.AddToDeployedUnits(guildCon.selectedRosterUnitNum);
        }
    }

    public void DeployNewGroup()
    {
        guildCon.DeployUnits();
    }



    public void InitButtons()
    {
        deployButton.GetComponent<Button>().onClick.AddListener(guildCon.DeployUnits);
    }


    public void UpdateRosterUnitsPanel()
    {
        bool rosterUpdated = false;
        for(int i = 0; i < guildCon.roster.Count; i++)
        {
            if (i < rosterPanels.Count)
            {
                if (rosterPanels[i].transform.Find("TextValues").Find("Name").GetComponent<Text>().text != guildCon.roster[i].GetName() ||
                    rosterPanels[i].transform.Find("Text").GetComponent<Text>().text != (i.ToString())) 
                {

                    rosterUpdated = true;
                    GameObject temp = rosterPanels[i];
                    rosterPanels[i] = RosterPanelConstructor(guildCon.roster[i], i);
                    rosterPanels[i].transform.SetParent(rosterUnitsPanel.transform, false);
                    Destroy(temp);

                }
            }
            else
            {
                rosterUpdated = true;
                rosterPanels.Add(RosterPanelConstructor(guildCon.roster[i], i));
                rosterPanels[i].transform.SetParent(rosterUnitsPanel.transform, false);
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
                    stdGUICon.PopulateMemberInfoPanel(groupMembers[i].transform.Find("TextValues").gameObject, guildCon.unitsToBeDeployed[i]);
                }
            }
            else
            {
                groupMembers[i].SetActive(false);
            }

        }

    }

    public void UpdateRecruitPanel()
    {
        Debug.Log(guildCon.selectedNewUnitNum);
        if (guildCon.selectedNewUnitNum > 0 && guildCon.selectedNewUnitNum <= guildCon.defaultUnits.Count)
        {
            stdGUICon.PopulateStatusWindow(guildCon.defaultUnits[guildCon.selectedNewUnitNum - 1], recruitStatusWindow);
        }
    }



    public GameObject RosterPanelConstructor(DynamicUnit rosterMember, int i)
    {

        GameObject rosterPanel = Instantiate(stdGUICon.rosterUnitPrefab);

        rosterPanel.transform.Find("TextValues").Find("Name").GetComponent<Text>().text = rosterMember.GetName();
        rosterPanel.transform.Find("TextValues").Find("Class").GetComponent<Text>().text = rosterMember.GetClassName().Substring(3);
        rosterPanel.transform.Find("TextValues").Find("Level").Find("LevelValue").GetComponent<Text>().text = rosterMember.GetLevel().ToString();

        rosterPanel.transform.Find("Text").GetComponent<Text>().text = i.ToString();

        myDelegate = stdGUICon.RosterUnitSelected;
        rosterPanel.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { stdGUICon.RosterUnitSelected(i); });

        return rosterPanel;
    }

}
