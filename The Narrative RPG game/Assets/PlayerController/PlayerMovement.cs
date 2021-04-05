using System;
using System.Collections;
using UnityEngine;

namespace PlayerController
{
    public class PlayerMovement : MonoBehaviour
    {
        private bool _isMoving;

        [SerializeField]
        private float walkSpeed = 3f;
        


        // Update is called once per frame
        void Update()
        {
            MovePlayer();
            
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