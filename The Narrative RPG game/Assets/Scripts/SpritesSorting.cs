using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NonPlayerObjects
{
    public class SpritesSorting : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderBase = 100;
        
        private Renderer _renderer;
        private GameObject _player;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _player = GameObject.Find("Player");
        }

        private void LateUpdate()
        {
            CalculateOrder();
        }

        private void CalculateOrder()
        {
            if (!(Math.Abs(_player.transform.position.y - transform.position.y) < 0.5f) ||
                gameObject.name == "Player") return;
            if (_player.transform.position.y < transform.position.y)
            {
                _renderer.sortingOrder = _player.GetComponent<SpriteRenderer>().sortingOrder - 5;
            }
            else
            {
                _renderer.sortingOrder = _player.GetComponent<SpriteRenderer>().sortingOrder + 5;
            }
        }
    }
}