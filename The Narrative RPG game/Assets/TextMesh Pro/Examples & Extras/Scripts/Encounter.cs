using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    public static string LastSceneName;

    public static Vector2 LastScenePosition;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LastSceneName = SceneManager.GetActiveScene().name;

            Debug.Log("Collision");
            SceneManager.LoadScene("CombatScene");
        }
        
    }
}
