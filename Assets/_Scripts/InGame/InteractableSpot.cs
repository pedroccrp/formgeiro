using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableSpot : MonoBehaviour
{
    public static bool canInteract = true;

    private SpriteRenderer rend;

    public Color normalColor, closeEnoughColor, hoverColor;

    [TextArea]
    public string riddleText;

    public GameObject DialogBox;

    private float distanceToPlayer;
    public float distanceToInteract = 1.5f;

    public Ants neededAnt;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, PlayerController.player.transform.position);

        if (canInteract && distanceToPlayer <= distanceToInteract)
        {
            if (rend.color == normalColor)
            {
                rend.color = closeEnoughColor;
            }
        }
        else
        {
            rend.color = normalColor;
        }
    }

    private void OnMouseOver()
    {
        if (canInteract && distanceToPlayer <= distanceToInteract)
        {
            rend.color = hoverColor;

            ShowDialogBox();
        } 
        else
        {
            CloseDialogBox();
        }
    }

    private void OnMouseExit()
    {
        rend.color = normalColor;

        CloseDialogBox();
    }

    private void OnMouseDown()
    {
        if (canInteract)
        {
            InventoryManager.instance.OpenForInteraction(this);
        }
    }

    public void ChooseAnt (Ants antChoosed)
    {
        InventoryManager.instance.CloseInventory();

        if (antChoosed == neededAnt)
        {
            AudioManager.Play("correct");

            Destroy(gameObject);
        }
        else
        {
            AudioManager.Play("wrong");
        }
    }

    private void ShowDialogBox()
    {
        DialogBox.GetComponentInChildren<TextMeshProUGUI>().text = riddleText;

        DialogBox.SetActive(true);
    }

    private void CloseDialogBox()
    {
        DialogBox.SetActive(false);
    }
}
