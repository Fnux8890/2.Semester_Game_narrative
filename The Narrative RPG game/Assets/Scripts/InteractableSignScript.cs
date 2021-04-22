using System;
using System.Collections;
using System.Collections.Generic;
using scribble_objects.Characters;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableSignScript : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider2D bc;
    public CharacterInteractable test;
    [HideInInspector]
    public InteractDirection enumDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<BoxCollider2D>();
        bc = GetComponent<BoxCollider2D>();
        bc.transform.position = test.Direction;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }

}




