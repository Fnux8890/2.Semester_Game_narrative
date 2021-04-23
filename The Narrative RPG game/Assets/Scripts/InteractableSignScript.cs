using System;
using System.Collections;
using System.Collections.Generic;
using scribble_objects.Characters;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableSignScript : MonoBehaviour
{
    public CharacterInteractable test;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entering");
        }
    }
}




