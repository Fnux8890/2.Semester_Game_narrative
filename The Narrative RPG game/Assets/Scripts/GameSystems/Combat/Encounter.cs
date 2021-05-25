using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystems.Combat
{
    public class Encounter : MonoBehaviour
    {
        public static string LastSceneName;
        private void Start()
        {
            LastSceneName = SceneManager.GetActiveScene().name;
        }

    }
}
