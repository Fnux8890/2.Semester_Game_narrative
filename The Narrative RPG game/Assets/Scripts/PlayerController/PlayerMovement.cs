using System;
using System.Collections;
using GameSystems.Dialogue;
using UnityEngine;

namespace PlayerController
{
    public class PlayerMovement : MonoBehaviour
    {
        private bool _isMoving;
        private DialogueManager _dialogueManager;
        public TextAsset json;
        

        [SerializeField]
        private float walkSpeed = 3f;
        


        // Update is called once per frame
        void Update()
        {
            MovePlayer();
            if (Input.GetKeyUp("y") && _dialogueManager == null)
            {

                try
                {
                    _dialogueManager = new DialogueManager(json);
                }
                finally
                {
                    Debug.Log("Dialogue loaded");
                }
                
            }
            if (Input.GetKeyUp("u") && _dialogueManager != null)
            {
                try
                {
                    _dialogueManager = null;
                }
                finally
                {
                    Debug.Log($"Dialogue disposed {_dialogueManager?.Equals(null)}");   
                }
            }
            if (Input.GetKeyUp("h") && _dialogueManager != null)
            {
                Debug.Log(_dialogueManager.Dialogue.nodes[2].Text);
            }
        }
        

        private void MovePlayer()
        {
            if (_isMoving) return;
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                input.y = 0;
            else
                input.x = 0;

            if (!input.Equals(Vector2.zero))
            {
                StartCoroutine(Move(transform, input));
            }
        }

        private IEnumerator Move(Transform entity, Vector2 input)
        {
            _isMoving = true;
            Vector2 startPos = entity.position;
            float t = 0;

            Vector2 endPos = new Vector3(startPos.x + Math.Sign(input.x), startPos.y + Math.Sign(input.y));

            while (t < 1f)
            {
                t += Time.deltaTime * walkSpeed;
                entity.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            _isMoving = false;

            yield return null;
        }

        
        
        
        
    }

    
}