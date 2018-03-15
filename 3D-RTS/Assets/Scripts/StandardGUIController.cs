using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGUIController : MonoBehaviour
{
    public PlayerController playerController;

    public Group currentGroup;
    public Building currentBuilding;
    public List<Group> selectedGroups;
    public string currentSelectionType;

    public static GameObject groupInfoPanel;
    public static GameObject unitInfoPanel;
    public static List<GameObject> groupMembers;
    public static GameObject buildingInfoPanel;
    public static GameObject buildingStatusPanel;
    public static GameObject itemInfoPanel;
    public static List<GameObject> memberPanelButtons;
    

    public GameObject minimap;
    public GameObject selectedUnitInformationPanel;

    private int activeMember;
    private int activeMemberPanel;


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
        groupInfoPanel.transform.Find("GroupMembers").GetComponentsInChildren<GameObject>(true, groupMembers);
        //doesn't work
        //groupInfoPanel.transform.Find("GroupMembers").gameObject.GetComponentsInChildren<GameObject>(true, GroupInformation.members);
        activeMember = 0;
        activeMemberPanel = -1;
        buildingInfoPanel = this.transform.Find("BuildingInformationPanel").gameObject;

    }


    // Update is called once per frame
    void Update()
    {
        if (playerController.selectedGroup != null)
        {
            if (currentGroup == null)
                currentGroup = playerController.SelectedGroup;

            if (playerController.selectedGroup != currentGroup || currentSelectionType != "group")
            {
                currentGroup = playerController.selectedGroup;
                currentSelectionType = "group";

                groupInfoPanel.SetActive(true);
                buildingInfoPanel.SetActive(false);

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
                    PopulateStatusWindow(currentGroup.GetUnits()[activeMember]);
                    activeMemberPanel = activeMember;
                }
                else if(activeMember >= currentGroup.GetUnits().Count)
                {
                    activeMember = activeMemberPanel;
                }
            }
           


        }

        else if (playerController.buildingSelected != null)
        {

        }
    }

    void SetActiveMember(int i)
    {
        activeMember = i;
    }
    void PopulateMemberInfoPanel(GameObject panel,  DynamicUnit member)
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
        
        panel.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = member.GetName();
        panel.transform.Find("Class").GetComponent<UnityEngine.UI.Text>().text = member.GetClassName();
        panel.transform.Find("Level").Find("LevelValue").GetComponent<UnityEngine.UI.Text>().text = member.GetLevel().ToString();
        panel.transform.Find("Level").Find("LevelValue").GetComponent<UnityEngine.UI.Text>().text = level.ToString();

        panel.transform.Find("Health").Find("HealthCurrent").GetComponent<UnityEngine.UI.Text>().text = ((int)currHP).ToString();
        panel.transform.Find("Health").Find("HealthMax").GetComponent<UnityEngine.UI.Text>().text = ((int)mHP).ToString();
        panel.transform.Find("Mana").Find("ManaCurrent").GetComponent<UnityEngine.UI.Text>().text = ((int)currMP).ToString();
        panel.transform.Find("Mana").Find("ManaMax").GetComponent<UnityEngine.UI.Text>().text = ((int)mMP).ToString();

    }

    void PopulateStatusWindow(DynamicUnit member)
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

        unitInfoPanel.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = member.GetName();
        unitInfoPanel.transform.Find("Class").GetComponent<UnityEngine.UI.Text>().text = member.GetClassName();
        unitInfoPanel.transform.Find("Level").Find("LevelValue").GetComponent<UnityEngine.UI.Text>().text = member.GetLevel().ToString();
        unitInfoPanel.transform.Find("Level").Find("LevelValue").GetComponent<UnityEngine.UI.Text>().text = level.ToString();

        unitInfoPanel.transform.Find("Health").Find("HealthCurrent").GetComponent<UnityEngine.UI.Text>().text = ((int)currHP).ToString();
        unitInfoPanel.transform.Find("Health").Find("HealthMax").GetComponent<UnityEngine.UI.Text>().text = ((int)mHP).ToString();
        unitInfoPanel.transform.Find("Mana").Find("ManaCurrent").GetComponent<UnityEngine.UI.Text>().text = ((int)currMP).ToString();
        unitInfoPanel.transform.Find("Mana").Find("ManaMax").GetComponent<UnityEngine.UI.Text>().text = ((int)mMP).ToString();

        unitInfoPanel.transform.Find("Experience").Find("ExperienceCurrent").GetComponent<UnityEngine.UI.Text>().text = member.GetExperience().ToString();
        unitInfoPanel.transform.Find("Experience").Find("ExperienceRequired").GetComponent<UnityEngine.UI.Text>().text = member.GetExperienceRequired().ToString();

    }


}
