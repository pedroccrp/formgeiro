using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGameTrigger : MonoBehaviour
{
    public LayerMask player;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 2f, player);

        if (hit)
        {
            Finish();
        }
    }

    private void Finish()
    {
        PlayerController.canControl = false;
        InventoryManager.canOpen = false;

        PlayerController.moveDir = 1;

        AudioManager.Play("getAnt");

        SceneTransitionManager.ChangeTo("PostGame");

        Destroy(this);
    }
}
