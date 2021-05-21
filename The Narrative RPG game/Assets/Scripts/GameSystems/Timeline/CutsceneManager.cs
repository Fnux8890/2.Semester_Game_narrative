using System;
using GameSystems.CustomEventSystems.Interaction;
using GameSystems.Dialogue;
using PlayerControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Utilities;

namespace GameSystems.Timeline
{
    public class CutsceneManager : MonoBehaviour
    {
        public bool isStartOfGame;
        public PlayableDirector _director;
        public TextAsset json;
        private TextAsset _previousJson;
        private PlayerActionControls _playerActionControls;

        private void Awake()
        {
            _playerActionControls = PlayerActionControlsManager.Instance.PlayerControls;
            _playerActionControls.Land.Interact.performed += Interact;
            InteractionHandler.Instance.EndCutscene += () => _playerActionControls.Land.Interact.performed -= Interact;
        }

        private void Start()
        {
            if (GameObject.Find("TutorialCanvas") != null && GameObject.Find("TutorialCanvas").activeSelf)
            {
                GameObject.Find("TutorialCanvas").gameObject.SetActive (false);
            }
            PlayerActionControlsManager.Instance.PlayerControls.Land.Movement.Disable();
            _director.stopped += director =>
            {
                InteractionHandler.Instance.OnStartCutscene(json);
            };
        }

        private void OnEnable()
        {
            _director.Play();
            PlayerActionControlsManager.Instance.PlayerControls.Land.Movement.Disable();
        }
        
        private void OnValidate()
        {
            UpdateJson();
        }
        
        public void UpdateJson()
        {
            if (_previousJson == null) _previousJson = new TextAsset();
            if (json.GetType() != typeof(TextAsset))
                throw new InvalidOperationException("File can only be Text Assets");
            if (_previousJson.ToString().Equals(json.ToString()) ^ _previousJson.name == json.name) return;
            _previousJson = json;
            CustomUtils.PrettifyJson(json);
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
