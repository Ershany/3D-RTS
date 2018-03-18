using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGUIController : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject standardGUIPanel;

    public Group currentGroup;
    public Building currentBuilding;
    public List<Group> selectedGroups;
    public string currentSelectionType;

    public static GameObject groupInfoPanel;
    public static GameObject unitInfoPanel;
    public static List<GameObject> groupMembers;
    public static List<UnityEngine.UI.Button> buttonsGroupMembers;
    public static GameObject buildingInfoPanel;
    public static GameObject buildingStatusPanel;
    public static GameObject itemInfoPanel;

    public GameObject minimap;
    public GameObject selectedUnitInformationPanel;

    public GuildHallGUIUtil guildGUI;

    public GameObject rosterUnitPrefab;

    private int activeMember;
    private int activeMemberPanel;
    public int partyCreationWindowMemberClicked;


    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

    }
    // Use this for initialization
    void Start()
    {
        currentSelectionType = "null";
        groupInfoPanel = GameObject.FindGameObjectWithTag("GroupInformationPanel");
        unitInfoPanel = groupInfoPanel.transform.Find("StatusWindow").gameObject;
        groupMembers = new List<GameObject>();
        buttonsGroupMembers = new List<UnityEngine.UI.Button>();
        for (int i = 0; i < 4; i++)
        {
            groupMembers.Add(groupInfoPanel.transform.Find("GroupMembers").Find("GroupMember" + (i + 1)).gameObject);
            buttonsGroupMembers.Add(groupMembers[i].transform.Find("Button").gameObject.GetComponent<UnityEngine.UI.Button>());

        }

        for (int i = 0; i < groupMembers.Count; i++)
        {
            Debug.Log(groupMembers[i].name);
        }
        //groupInfoPanel.transform.Find("GroupMembers").GetComponentsInChildren<GameObject>(true, groupMembers);
   

        activeMember = 0;
        activeMemberPanel = -1;
        partyCreationWindowMemberClicked = -1;
        buildingInfoPanel = new GameObject();

        if (GameObject.FindGameObjectWithTag("GuildHall") != null)
            guildGUI = new GuildHallGUIUtil(GameObject.FindGameObjectWithTag("GuildHallGUIPanel"), GameObject.FindGameObjectWithTag("GuildHall").GetComponent<GuildHallController>(), this);
        else
            guildGUI = null;
        standardGUIPanel = GameObject.FindGameObjectWithTag("StandardGUIPanel");

        //buildingInfoPanel = GameObject.FindGameObjectWithTag("BuildingInformationPanel");

    }


    // Update is called once per frame
    void Update()
    {
        if (guildGUI == null)
        {
            if (GameObject.FindGameObjectWithTag("GuildHall") != null)
                guildGUI = new GuildHallGUIUtil(GameObject.FindGameObjectWithTag("GuildHallGUIPanel"), GameObject.FindGameObjectWithTag("GuildHall").GetComponent<GuildHallController>(), this);

        }


        if (playerController.selectedGroup != null)
        {
            if (currentGroup == null)
                currentGroup = playerController.selectedGroup;

            if (playerController.selectedGroup != currentGroup || currentSelectionType != "group")
            {
                currentGroup = playerController.selectedGroup;
                currentSelectionType = "group";

                groupInfoPanel.SetActive(true);
                //buildingInfoPanel.SetActive(false);

                for (int i = 0; i < 4; i++)
                {
                    if (i < currentGroup.GetUnits().Count)
                    {
                        groupMembers[i].SetActive(true);
                        PopulateMemberInfoPanel(groupMembers[i], currentGroup.GetUnits()[i]);
                    }
                    else
                    {
                        groupMembers[i].SetActive(false);
                    }
                }
            }

            if (currentGroup != null)
            {
                if (activeMemberPanel != activeMember && activeMember < currentGroup.GetUnits().Count)
                {
                    PopulateStatusWindow(currentGroup.GetUnits()[activeMember], unitInfoPanel);
                    activeMemberPanel = activeMember;
                }
            }
        }

        else if (playerController.buildingSelected != null)
        {

        }
    }

    public void SetActiveMember(int i)
    {
        Debug.Log(i);
        activeMember = i;
    }

    public void PartyCreationWindowMemberClicked(int i)
    {

    }





    public void PopulateMemberInfoPanel(GameObject panel,  DynamicUnit member)
    {
        int str = 0;
        int intel = 0;
        int dex = 0;
        float mHP = 0;
        float currHP = 0;
        int mMP = 0;
        int currMP = 0;
        int level = 0;

        member.GetStatus(ref str, ref intel, ref dex, ref mHP, ref currHP, ref mMP, ref currMP, ref level);

        panel.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = member.GetName(); ;
        panel.transform.Find("Class").GetComponent<UnityEngine.UI.Text>().text = member.GetClassName().Substring(3) ;
        panel.transform.Find("Level").Find("LevelValue").GetComponent<UnityEngine.UI.Text>().text = member.GetLevel().ToString();
        panel.transform.Find("Level").Find("LevelValue").GetComponent<UnityEngine.UI.Text>().text = level.ToString();

        panel.transform.Find("Health").Find("HealthCurrent").GetComponent<UnityEngine.UI.Text>().text = ((int)currHP).ToString();
        panel.transform.Find("Health").Find("HealthMax").GetComponent<UnityEngine.UI.Text>().text = ((int)mHP).ToString();
        panel.transform.Find("Mana").Find("ManaCurrent").GetComponent<UnityEngine.UI.Text>().text = ((int)currMP).ToString();
        panel.transform.Find("Mana").Find("ManaMax").GetComponent<UnityEngine.UI.Text>().text = ((int)mMP).ToString();

    }

    public void PopulateStatusWindow(DynamicUnit member, GameObject statusWindow)
    {
        int str = 0;
        int intel = 0;
        int dex = 0;
        float mHP = 0;
        float currHP = 0;
        int mMP = 0;
        int currMP = 0;
        int level = 0;

        member.GetStatus(ref str, ref intel, ref dex, ref mHP, ref currHP, ref mMP, ref currMP, ref level);

        statusWindow.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = member.GetName();
        statusWindow.transform.Find("Class").GetComponent<UnityEngine.UI.Text>().text = member.GetClassName().Substring(3);
        statusWindow.transform.Find("Level").Find("LevelValue").GetComponent<UnityEngine.UI.Text>().text = member.GetLevel().ToString();
        statusWindow.transform.Find("Level").Find("LevelValue").GetComponent<UnityEngine.UI.Text>().text = level.ToString();

        statusWindow.transform.Find("Health").Find("HealthCurrent").GetComponent<UnityEngine.UI.Text>().text = ((int)currHP).ToString();
        statusWindow.transform.Find("Health").Find("HealthCurrent").GetComponent<UnityEngine.UI.Text>().text = ((int)currHP).ToString();
        statusWindow.transform.Find("Health").Find("HealthMax").GetComponent<UnityEngine.UI.Text>().text = ((int)mHP).ToString();
        statusWindow.transform.Find("Mana").Find("ManaCurrent").GetComponent<UnityEngine.UI.Text>().text = ((int)currMP).ToString();
        statusWindow.transform.Find("Mana").Find("ManaMax").GetComponent<UnityEngine.UI.Text>().text = ((int)mMP).ToString();

        statusWindow.transform.Find("Strength").Find("StrengthValue").GetComponent<UnityEngine.UI.Text>().text = ((int)str).ToString();
        statusWindow.transform.Find("Intelligence").Find("IntelligenceValue").GetComponent<UnityEngine.UI.Text>().text = ((int)intel).ToString();
        statusWindow.transform.Find("Dexterity").Find("DexterityValue").GetComponent<UnityEngine.UI.Text>().text = ((int)dex).ToString();

        statusWindow.transform.Find("Damage").Find("DamageMin").GetComponent<UnityEngine.UI.Text>().text = ((int)str).ToString();
        statusWindow.transform.Find("Damage").Find("DamageMax").GetComponent<UnityEngine.UI.Text>().text = ((int)str).ToString();

        statusWindow.transform.Find("Experience").Find("ExperienceCurrent").GetComponent<UnityEngine.UI.Text>().text = member.GetExperience().ToString();
        statusWindow.transform.Find("Experience").Find("ExperienceRequired").GetComponent<UnityEngine.UI.Text>().text = member.GetExperienceRequired().ToString();

    }


}
