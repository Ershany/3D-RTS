using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGUIController : MonoBehaviour {

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


    public GameObject minimap;
    public GameObject selectedUnitInformationPanel;



    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        
    }
    // Use this for initialization
    void Start () {
        currentSelectionType = "null";
        groupInfoPanel = GameObject.FindGameObjectWithTag("GroupInformationPanel");
        unitInfoPanel = groupInfoPanel.transform.Find("StatusWindow").gameObject;
        groupInfoPanel.transform.Find("GroupMembers").gameObject.GetComponentsInChildren<GameObject>(true, GroupInformation.members);

        buildingInfoPanel = this.transform.Find("BuildingInformationPanel").gameObject;

	}
    
	
	// Update is called once per frame
	void Update () {
        if (playerController.SelectedGroup != null)
        {

            if (playerController.SelectedGroup != currentGroup || currentSelectionType != "group")
            {
                currentGroup = playerController.SelectedGroup;
                currentSelectionType = "group";

                groupInfoPanel.SetActive(true);
                buildingInfoPanel.SetActive(false);

                for (int i = 0; i < 4; i++)
                {
                    if (i < currentGroup.GetUnits().Count)
                    {

                    }
                }


            }

        }

        else if (playerController.buildingSelected != null)
        {

        }
	}
}
