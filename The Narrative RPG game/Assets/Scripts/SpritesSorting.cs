using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NonPlayerObjects
{
    public class SpritesSorting : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderBase = 5000;
        [SerializeField]
        private int offset = 0;
        [SerializeField]
        private bool runOnlyOnce = false;

        private Renderer _renderer;
        private GameObject _player;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _player = GameObject.Find("Player");
        }

        private void LateUpdate()
        {
            switch (gameObject.name)
            {
                case "UpperPart":
                    CalculateOrder(50);
                    break;
                case "TreeMisc":
                    CalculateOrder(51);
                    break;
                case "StartingLevelMisc":
                    CalculateOrder(51);
                    break;
                default:
                    CalculateOrder(0);
                    break;
            }
            if (runOnlyOnce)
            {
                Destroy(this);
            }
        }

        private void CalculateOrder(int addition)
        {
            _renderer.sortingOrder = (int) (sortingOrderBase - transform.position.y - offset) + addition;
        }
    }
}