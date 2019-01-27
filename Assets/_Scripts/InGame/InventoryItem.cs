using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Ants ant;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.instance.NameBox.text = ant.name;
        InventoryManager.instance.DescriptionBox.text = ant.description;

        InventoryManager.instance.AntInfo.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryManager.choosingItem)
        {
            InventoryManager.instance.currentInteractableSpot.ChooseAnt(ant);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.instance.AntInfo.SetActive(false);
    }
}
