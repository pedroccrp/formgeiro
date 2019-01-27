using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public static bool choosingItem = false;
    public static bool canOpen = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
     }

    public static List<Ants> inventoryAnts;

    private void Start()
    {
        inventoryAnts = new List<Ants>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !inventoryCanvas.activeSelf && canOpen)
        {
            OpenInventory();
        }  
    }

    public InteractableSpot currentInteractableSpot;

    [Header(header:"Inventory")]
    public GameObject inventoryCanvas;
    public Transform inventoryContent;
    public GameObject inventoryItem;

    [Header(header: "Inventory Item")]
    public GameObject AntInfo;
    public TextMeshProUGUI NameBox;
    public TextMeshProUGUI DescriptionBox;

    public void OpenForInteraction(InteractableSpot interactable)
    {
        currentInteractableSpot = interactable;

        choosingItem = true;

        OpenInventory();
    }

    public void OpenInventory()
    {
        InteractableSpot.canInteract = false;

        PlayerController.canControl = false;
        PlayerController.moveDir = 0;

        inventoryCanvas.SetActive(true);

        StartCoroutine(KeepInventory());
    }

    public void UpdateInventory()
    {
        int childs = inventoryContent.childCount;

        for (int i = childs - 1; i >= 0; i--)
        {
            Destroy(inventoryContent.GetChild(i).gameObject);
        }

        foreach (Ants ant in inventoryAnts)
        {
            GameObject item = GameObject.Instantiate(inventoryItem, inventoryContent);
            item.transform.GetChild(0).GetComponent<Image>().sprite = ant.sprite;
            item.transform.GetComponentInChildren<InventoryItem>().ant = ant;
        }
    }

    private IEnumerator KeepInventory ()
    {
        while (!Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I))
        {
            yield return null;
        }

        CloseInventory();
    }

    public void CloseInventory()
    {
        inventoryCanvas.SetActive(false);

        AntInfo.SetActive(false);

        InteractableSpot.canInteract = true;

        choosingItem = false;

        PlayerController.canControl = true;
    }

    public void AddItem (Ants ant)
    {
        inventoryAnts.Add(ant);

        UpdateInventory();
    }
}
