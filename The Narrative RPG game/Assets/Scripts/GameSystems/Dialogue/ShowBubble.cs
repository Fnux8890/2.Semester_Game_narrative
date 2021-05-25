using GameSystems.CustomEventSystems.Interaction;
using UnityEngine;

namespace GameSystems.Dialogue
{
    [RequireComponent(typeof(Animator))]
    public class ShowBubble : MonoBehaviour
    {
        public int id;
        private Animator _animator;
        private static readonly int Showing = Animator.StringToHash("Showing");
        private static readonly int Exit = Animator.StringToHash("Exit");
        private static readonly int Enter = Animator.StringToHash("Enter");

        void Start()
        {
            _animator = GetComponent<Animator>();
            InteractionHandler.Instance.ShowBubble += Show;
            InteractionHandler.Instance.HideBubble += Hide;
        }
        

        private void Show(int id)
        {
            if (this.id == id)
            {
                _animator.SetTrigger(Enter);
                _animator.SetBool(Showing, true);
            }
        }

        private void Hide(int id)
        {
            if (this.id == id)
            {
                _animator.SetTrigger(Exit);
            }
        }
    }
}
