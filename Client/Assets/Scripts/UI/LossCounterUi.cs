using TMPro;
using UnityEngine;

namespace UI
{
    public class LossCounterUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private int playerLoss;
        private int enemyLoss;

        public void SetPlayerLoss(int value)
        {
            playerLoss = value;
            UpdateText();
        }

        public void SetEnemyLoss(int value)
        {
            enemyLoss = value;
            UpdateText();
        }

        private void UpdateText()
        {
            text.SetText($"{playerLoss} : {enemyLoss}");
        }
    }
}