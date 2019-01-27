using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreGameManager : MonoBehaviour
{
    [TextArea]
    public string[] texts;

    public TextMeshProUGUI storyText;

    private int currentText = 0;

    private void Update()
    {
        if (currentText < texts.Length)
        {
            storyText.text = texts[currentText];

            if (Input.GetButtonDown("Submit"))
            {
                currentText++;
            }
        }
        else
        {
            SceneTransitionManager.ChangeTo("Game");

            Destroy(this);
        }
    }
}
