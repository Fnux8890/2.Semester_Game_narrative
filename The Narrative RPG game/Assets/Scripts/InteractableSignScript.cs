using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameSystems.Dialogue;
using scribble_objects.Characters;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(ArcCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class InteractableSignScript : MonoBehaviour
{
    public CharacterInteractable test;
    public TextAsset json;
    
    private PolygonCollider2D _polygonCollider;
    private ArcCollider2D _arcCollider2D;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        // Init
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _arcCollider2D = GetComponent<ArcCollider2D>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        
        
        
        //modify
        _arcCollider2D.PizzaSlice = true;
        _arcCollider2D.Radius = (float) 1.5;
        _arcCollider2D.OffsetRotation = 190;
        _arcCollider2D.TotalAngle = 160;

        _polygonCollider.isTrigger = true;

        if (_spriteRenderer.sprite.name == "outside_4")
        {
            _boxCollider.offset = new Vector2((float) -0.009367943, (float) -0.3634593);
            _boxCollider.size = new Vector2((float) 0.8948994, (float) 0.4104857);
        }
        
        DialogueManager dm = new DialogueManager(json);



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




