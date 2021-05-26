using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Combat
{
    public class BattleHUD : MonoBehaviour
    {
        public Text nameText;
        public Text levelText;
        public Slider hpSlider;

        public void SetHUD(Unit unit)
        {
            nameText.text = unit.unitName;
            levelText.text = "Lvl " + unit.unitLevel;
            hpSlider.maxValue = unit.maxHP;
            hpSlider.value = unit.currentHP;
        }

        public void setHP(int hp)
        {
            hpSlider.value = hp;
        }

        public void disable()
        {
            gameObject.SetActive(false);
        }
    }
}
