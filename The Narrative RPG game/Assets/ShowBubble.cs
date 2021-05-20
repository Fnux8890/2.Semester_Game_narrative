using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class ShowBubble : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Showing = Animator.StringToHash("Showing");
    private static readonly int Exit = Animator.StringToHash("Exit");
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        Collider2D signCollider = gameObject.GetComponentInParent<Collider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entering");
            _animator.SetBool(Showing, true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exiting");
            _animator.SetTrigger(Exit);
        }
    }
}
