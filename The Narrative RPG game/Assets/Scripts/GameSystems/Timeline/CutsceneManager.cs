using System;
using System.Collections.Generic;
using GameSystems.CustomEventSystems;
using GameSystems.CustomEventSystems.Interaction;
using GameSystems.Dialogue;
using PlayerControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Utilities;

namespace GameSystems.Timeline
{
    public class CutsceneManager : Singleton<CutsceneManager>
    {
        private List<TimelineAsset> _cutscenes = new List<TimelineAsset>();
        
        private void OnEnable()
        {
            if (!(_cutscenes.Count > 0))
            {
                var assets = Resources.LoadAll<TimelineAsset>("Cutscenes/Timeline");
                _cutscenes.AddRange(assets);
            }
            CutsceneHandler.Instance.StartCutsceneWithDialogue += PlayCutsceneWithDialogue;
            CutsceneHandler.Instance.StartCutsceneWithNoDialogue += PlayCutsceneWithNoDialogue;
            TriggerCutSceneHandler.Instance.TriggerCutScene += PlayCutsceneWithDialogue;
        }

        private void OnDisable()
        {
            if (CutsceneHandler.Instance != null)
            {
                CutsceneHandler.Instance.StartCutsceneWithDialogue -= PlayCutsceneWithDialogue;
                CutsceneHandler.Instance.StartCutsceneWithNoDialogue -= PlayCutsceneWithNoDialogue;
            }
        }


        private void PlayCutsceneWithDialogue(TextAsset json, string cutscene)
        {
            var director = GameObject.Find("GameManagers").transform.Find("TimelineManager")
                .GetComponent<PlayableDirector>();
            director.playableAsset = _cutscenes.Find(x => x.name == cutscene);
            DialogueHandleUpdate.Instance.OnUpdateCanvas();
            director.Play();
            PlayerActionControlsManager.Instance.PlayerControls.Land.Movement.Disable();
            if (GameObject.Find("TutorialCanvas") != null && GameObject.Find("TutorialCanvas").activeSelf)
            {
                GameObject.Find("TutorialCanvas").gameObject.SetActive(false);
            }

            PlayerActionControlsManager.Instance.PlayerControls.Land.Movement.Disable();
            director.stopped += dir =>
            {
                PlayerActionControlsManager.Instance.PlayerControls.Land.Interact.performed += Interact;
                InteractionHandler.Instance.OnStartCutscene(json);
                //InteractionHandler.Instance.OnStartCutscene(json);
            };
        }

        private void PlayCutsceneWithNoDialogue(string cutscene)
        {
            var director = GameObject.Find("GameManagers").transform.Find("TimelineManager")
                .GetComponent<PlayableDirector>();
            director.playableAsset = _cutscenes.Find(x => x.name == cutscene);
            DialogueHandleUpdate.Instance.OnUpdateCanvas();
            director.Play();
            PlayerActionControlsManager.Instance.PlayerControls.Land.Movement.Disable();
            director.stopped += dir =>
            {
                PlayerActionControlsManager.Instance.PlayerControls.Land.Movement.Enable();
            };
        }
        
        
        
        
        private void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                InteractionHandler.Instance.OnInteract();
            }
        }
    }
}