using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class PickupAnt : MonoBehaviour
{
    public Ants ant;

    private float distanceFromPlayer;
    public float distanceToInteract;

    public GameObject DialogBox;

    private void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

        GetComponent<SpriteRenderer>().sprite = ant.sprite;
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, PlayerController.player.transform.position);
    }

    private void OnMouseDown()
    {
        if (distanceFromPlayer <= distanceToInteract)
        {
            GetAnt();
        }
    }

    private void OnMouseOver()
    {
        if (distanceFromPlayer <= distanceToInteract)
        {
            ShowDialogBox();
        }
        else
        {
            CloseDialogBox();
        }
    }

    private void OnMouseExit()
    {
        CloseDialogBox();
    }

    private void ShowDialogBox()
    {
        DialogBox.GetComponentInChildren<TextMeshProUGUI>().text = ant.pickupText;

        DialogBox.SetActive(true);
    }

    private void CloseDialogBox()
    {
        DialogBox.SetActive(false);
    }

    private void GetAnt()
    {
        AudioManager.Play("getAnt");

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        InventoryManager.instance.AddItem(ant);

        ShowGetDisplay();
    }

    public GameObject antGetCanvas;
    public TextMeshProUGUI antNameTMPro;
    public Image antSpriteRend;
    public Button exitButton;

    private void ShowGetDisplay ()
    {
        PlayerController.canControl = false;
        PlayerController.moveDir = 0;

        InventoryManager.canOpen = false;

        antNameTMPro.text = "Nice! You found " + ant.name + "!";
        antSpriteRend.sprite = ant.sprite;

        exitButton.onClick.AddListener(CloseDisplay);

        antGetCanvas.SetActive(true);

        StartCoroutine(KeepOpen());
    }

    private IEnumerator KeepOpen ()
    {
        while (!Input.GetKeyDown(KeyCode.Escape))
        {
            yield return null;
        }

        CloseDisplay();
    }

    private void CloseDisplay ()
    {
        PlayerController.canControl = true;

        InventoryManager.canOpen = true;

        antGetCanvas.SetActive(false);

        Destroy(gameObject);
    }
}
