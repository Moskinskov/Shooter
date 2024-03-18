using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthUi : MonoBehaviour
    {
        [SerializeField] private Image imgHp;

        private void Start()
        {
            imgHp.fillAmount = 1;
        }

        public void UpdateUi(float max, float current)
        {
            float percent = current / max;
            imgHp.fillAmount = percent;
        }
    }
}