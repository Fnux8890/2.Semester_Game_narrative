using System;
using System.Collections;
using GameSystems.Combat;
using GameSystems.CustomEventSystems;
using GameSystems.CustomEventSystems.Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneChangeAnim : Singleton<SceneChangeAnim>
{
    private Animator _transition;
    private static readonly int StartAnimation = Animator.StringToHash("Start");
    private SceneLoadManager _sceneManager;
    private GameObject _levelLoader;
    
    private void OnEnable()
    {
        UpdateRef();
        _sceneManager = SceneLoadManager.Instance;
        InteractionHandler.Instance.LevelAnimInt += (index) => StartCoroutine(LoadLevel(index));
        InteractionHandler.Instance.LevelAnimName += (levelName) => StartCoroutine(LoadLevel(levelName));
        InteractionHandler.Instance.LevelAnimPrevious += () => StartCoroutine(LoadPreviousLevel());
    }

    private void UpdateRef()
    {
        _levelLoader = GameObject.Find("LevelLoader");
        _transition = _levelLoader.transform.GetChild(0).GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        if (InteractionHandler.Instance != null)
        {
            InteractionHandler.Instance.LevelAnimInt -= (index) => StartCoroutine(LoadLevel(index));
            InteractionHandler.Instance.LevelAnimName -= (levelName) => StartCoroutine(LoadLevel(levelName));
        }
        
    }
    


    private IEnumerator LoadLevel(int levelIndex)
    {
        UpdateRef();
        SceneLoadHandler.Instance.OnStoreLastPosition(GameObject.Find("Player").transform.position);
        SceneLoadHandler.Instance.OnStoreLastSceneName(SceneManager.GetActiveScene().name);
        _transition.SetTrigger(StartAnimation);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelIndex);
    }
    
    private IEnumerator LoadLevel(string levelName)
    {
        UpdateRef();
        SceneLoadHandler.Instance.OnStoreLastPosition(GameObject.Find("Player").transform.position);
        SceneLoadHandler.Instance.OnStoreLastSceneName(SceneManager.GetActiveScene().name);
        _transition.SetTrigger(StartAnimation);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelName);
    }

    private IEnumerator LoadPreviousLevel()
    {
        UpdateRef();
        var lastScene = SceneLoadHandler.Instance.OnGetLastSceneName();
        _transition.SetTrigger(StartAnimation);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(lastScene);
    }
    
}
