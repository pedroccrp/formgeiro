using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Animator transitionAnimator;

    public static string sceneName;
    
    public static void ChangeTo(string sceneName)
    {
        SceneTransitionManager.sceneName = sceneName;

        instance.transitionAnimator.SetTrigger("fadeout");
    }

    public static void RealChange ()
    {
        SceneManager.LoadScene(sceneName);
    }
}
