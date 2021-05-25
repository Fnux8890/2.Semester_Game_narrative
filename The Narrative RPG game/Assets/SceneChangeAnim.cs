using System;
using System.Collections;
using System.Collections.Generic;
using GameSystems.CustomEventSystems.Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeAnim : MonoBehaviour
{
    public Animator transition;

    private void Start()
    {
        InteractionHandler.Instance.levelAnim += (index) => StartCoroutine(LoadLevel(index));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelIndex);
    }
}
