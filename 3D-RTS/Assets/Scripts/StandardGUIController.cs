using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGUIController : MonoBehaviour
{
     delegate void MyDelegate(int num);
    MyDelegate myDelegate;

    //Prefabs
    public GameObject rosterUnitPrefab;
    public GameObject multipleSelectionPrefab;
    public GameObject unitLetterIconPrefab;
    public GameObject guildhallGUIColliderPrefab;
    public GameObject standardGUIColliderPrefab;

    public PlayerController _playerController;
    public GameObject standardGUIPanel;

    public Group currentGroup;
    public Building currentBuilding;
    public List<Group> selectedGroups;
    public GameObject selectedGroupsPanel;
    public List<GameObject> selectedGroupsPanels;
    public string currentSelectionType;

    public static GameObject groupInfoPanel;
    public static GameObject unitInfoPanel;
    public static List<GameObject> groupMembers;
    public static List<UnityEngine.UI.Button> buttonsGroupMembers;
    public static GameObject buildingInfoPanel;
    public static GameObject buildingStatusPanel;
    public static GameObject itemInfoPanel;

    public GameObject currentActivePanel;

    public GameObject minimap;
    public GameObject selectedUnitInformationPanel;

    public GuildHallGUIUtil guildGUI;
    public GameObject guildGUIPanel;

    private int activeMember;
    private int activeMemberPanel;
    public int partyCreationWindowMemberClicked;
    public int groupSelectedToPreview;

    public GameObject guildhallGUICollider;
    public GameObject standardGUICollider;

    [ColorUsageAttribute(true, true, 0, 1, 0, 1, order = 0)]
    public Color selectionRectFillColor;


    [ColorUsageAttribute(true, true, 0, 1, 0, 1, order = 0)]
    public Color selectionRectBorderColor;

    Rect rect;

    private float updateTimer;

    private class MouseData
    {
        public bool mouse_1;
        public Vector2 mouse_1_StartPos;
        public Vector2 mouse_1_EndPos;

        public MouseData()
        {
            mouse_1 = false;
            mouse_1_StartPos = new Vector2();
        }

    }
    
    private MouseData mouseData;

    void Awake()
    {
        _playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();

    }
    // Use this for initialization
    void Start()
    {
        updateTimer = 0.0f;
        mouseData = new MouseData();
        currentSelectionType = "null";
        groupSelectedToPreview = 0;
        groupInfoPanel = GameObject.FindGameObjectWithTag("GroupInformationPanel");
        selectedGroupsPanel = groupInfoPanel.transform.Find("SelectedGroups").gameObject;
        selectedGroupsPanel.SetActive(false);
        selectedGroupsPanels = new List<GameObject>();
        unitInfoPanel = groupInfoPanel.transform.Find("StatusWindow").gameObject;
        groupMembers = new List<GameObject>();
        buttonsGroupMembers = new List<UnityEngine.UI.Button>();
        guildGUIPanel = GameObject.FindGameObjectWithTag("GuildHallGUIPanel");
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

        guildGUI = new GuildHallGUIUtil(GameObject.FindGameObjectWithTag("GuildHallGUIPanel"), this);
        guildGUIPanel.SetActive(false);

        standardGUIPanel = GameObject.FindGameObjectWithTag("StandardGUIPanel");

        currentActivePanel = standardGUIPanel;


        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        guildhallGUICollider = Instantiate(guildhallGUIColliderPrefab);
        guildhallGUICollider.transform.SetParent(mainCamera.transform, false);
        guildhallGUICollider.SetActive(false);
        standardGUICollider = Instantiate(standardGUIColliderPrefab);
        standardGUICollider.transform.SetParent(mainCamera.transform, false);

        //buildingInfoPanel = GameObject.FindGameObjectWithTag("BuildingInformationPanel");

    }

    // Update is called once per frame
    void Update()
    {
        updateTimer += Time.deltaTime;
        UpdateGUIInputs();
        


        if (guildGUI.guildCon == null)
        {
            if (GameObject.FindGameObjectWithTag("GuildHall") != null)
            {
                if (GameObject.FindGameObjectWithTag("GuildHall").GetComponent<GuildHallController>().building.IsPlaced)
                {
                    guildGUI.guildCon = GameObject.FindGameObjectWithTag("GuildHall").GetComponent<GuildHallController>();
                    guildGUI.InitButtons();
                }
            }
        }

        if (_playerController.selectedGroups != null && _playerController.selectedGroups.Count > 0)
        {
            if (_playerController.selectedGroups.Count == 1)
            {
                if (currentGroup == null)
                    currentGroup = _playerController.selectedGroups[0];



                if (_playerController.playerSelectedSingleGroup)
                {
                    currentGroup = _playerController.selectedGroups[0];
                    currentSelectionType = "group";

                    unitInfoPanel.SetActive(true);
                    GroupsPanelsDestructor();
                    standardGUIPanel.SetActive(true);
                    guildGUIPanel.SetActive(false);
                    _playerController.playerSelectedSingleGroup = false;
                    activeMemberPanel = -1;
                    activeMember = 0;
                    //buildingInfoPanel.SetActive(false);

                }

                if (activeMemberPanel != activeMember || updateTimer > 0.5f)
                {
                    updateTimer = 0.0f;
                    if (activeMemberPanel != activeMember && activeMember < currentGroup.GetUnits().Count)
                    {
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
                        PopulateStatusWindow(currentGroup.GetUnits()[activeMember], unitInfoPanel);
                        activeMemberPanel = activeMember;
                    }
                }
            }
            else if (_playerController.selectedGroups.Count > 1)
            {
                if (_playerController.playerSelectedGroups)
                {
                    _playerController.playerSelectedGroups = false;
                    currentSelectionType = "groups";
                    unitInfoPanel.SetActive(false);
                    GroupsPanelsConstructor(_playerController.selectedGroups);

                }
                currentGroup = _playerController.selectedGroups[groupSelectedToPreview];
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

        }

        else if (_playerController.buildingSelected != null)
        {
            if (_playerController.buildingSelected.name == "GuildHall")
            {

                if (currentSelectionType != "GuildHall" || !guildGUIPanel.activeSelf || _playerController.playerSelectedGuildHall)
                {
                    currentSelectionType = "GuildHall";
                    guildGUIPanel.SetActive(true);
                    standardGUIPanel.SetActive(false);
                    guildGUI.recruitPanel.SetActive(false);
                    if (selectedGroupsPanel.activeSelf)
                    {
                        GroupsPanelsDestructor();
                    }
                    _playerController.playerSelectedGuildHall = false;
                }
                guildGUI.UpdatePartyCreationPanel();
                guildGUI.UpdateRosterUnitsPanel();
            }
        }

        /*
        //added stuff here TAKE A LOOK AT IT should be commented here 
        if (_playerController.selectedGroups.Count == 0 && _playerController.buildingSelected == null)
        {
            //I have deselected everything go back to usual unit info panel 
            guildGUIPanel.SetActive(false);
            standardGUIPanel.SetActive(true);
            unitInfoPanel.SetActive(true);
        }
        */

        UpdateColliders();
    }

    void OnGUI()
    {
        if (mouseData.mouse_1)
        {
            if (Vector2.Distance(mouseData.mouse_1_StartPos,Input.mousePosition) > 10)
            {
                mouseData.mouse_1_EndPos = Input.mousePosition;
                // Create a rect from both mouse positions
                rect = GUIUtility.GetScreenRect(mouseData.mouse_1_StartPos, mouseData.mouse_1_EndPos);
                GUIUtility.DrawSelectionRect(rect, selectionRectFillColor);
                GUIUtility.DrawSelectionRectBorder(rect, 2, selectionRectBorderColor);
            }
        }
    }

    void UpdateGUIInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseData.mouse_1 = true;
            mouseData.mouse_1_StartPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseData.mouse_1 = false;
            if (Vector2.Distance(mouseData.mouse_1_StartPos, Input.mousePosition) > 10)
            {
                _playerController.SelectOnRect(mouseData.mouse_1_StartPos, Input.mousePosition);
            }
        }
    }

    void UpdateColliders()
    {
        guildhallGUICollider.SetActive(guildGUIPanel.activeSelf);
        standardGUICollider.SetActive(standardGUIPanel.activeSelf);
    }
    public void SetActiveMember(int i)
    {
        Debug.Log(i);
        activeMember = i;
    }

    public void PartyCreationWindowMemberClicked(int i)
    {
        Debug.Log("PartyCreationWindowMemberClicked called");
        guildGUI.PartyCreationWindowMemberClicked(i);

    }

    public void ClassSelectedInRecruitPanel()
    {

        guildGUI.ClassSelectedInRecruitPanel();

    }
    public void RecruitSelectedUnit()
    {
            guildGUI.RecruitSelectedUnit();

    }

    public void RosterUnitSelected(int i)
    {
        guildGUI.RosterUnitSelected(i);
    }

    public void RosterUnitSubmitted()
    {
        guildGUI.RosterUnitSubmitted();
    }

    public void DeployNewGroup()
    {
        Debug.Log("DeployNewGroup function called");
        guildGUI.DeployNewGroup();
    }

    public void GroupPreviewSelected(int i)
    {
        Debug.Log(i);
        if (i == groupSelectedToPreview)
        {
            Group temp = _playerController.selectedGroups[groupSelectedToPreview];
            _playerController.selectedGroups = new List<Group>();
            _playerController.selectedGroups.Add(temp);
            _playerController.playerSelectedSingleGroup = true;
        }
        else
        {
            groupSelectedToPreview = i;
        }
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

        Debug.Log(member.GetName());

        panel.transform.Find("Name").GetComponent<UnityEngine.UI.Text>().text = member.GetName();
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

    public void GroupsPanelsConstructor(List<Group> groups)
    {
        selectedGroupsPanel.SetActive(true);

        if (selectedGroupsPanels.Count > 0)
        {
            for (int i = 0; i < selectedGroupsPanels.Count; i++)
            {
                Destroy(selectedGroupsPanels[i]);
            }
            selectedGroupsPanels = new List<GameObject>();
        }

        for (int i = 0; i < groups.Count; i++)
        {
            selectedGroupsPanels.Add(GroupPanelConstructor(groups[i], i));
            selectedGroupsPanels[i].transform.SetParent(selectedGroupsPanel.transform, false);
        }


    }


    public GameObject GroupPanelConstructor(Group group, int index)
    {

        GameObject groupPanel = Instantiate(multipleSelectionPrefab);

        for (int i = 0; i < group.GetUnits().Count; i++)
        {
            GameObject unitIconPanel = Instantiate(unitLetterIconPrefab);
            unitIconPanel.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = group.GetUnits()[i].GetClassName().Substring(3, 1);
            unitIconPanel.transform.SetParent(groupPanel.transform, false);
        }

        myDelegate = GroupPreviewSelected;
        groupPanel.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { GroupPreviewSelected(index); });

        return groupPanel;
    }

    public void GroupsPanelsDestructor()
    {
        selectedGroupsPanel.SetActive(true);

        if (selectedGroupsPanels.Count > 0)
        {
            for (int i = 0; i < selectedGroupsPanels.Count; i++)
            {
                Destroy(selectedGroupsPanels[i]);
            }
        }

        selectedGroupsPanel.SetActive(false);
    }
}
