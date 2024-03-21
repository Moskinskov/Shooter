using UI;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthUi ui;

    private int max;
    private int current;

    public void SetMax(int value)
    {
        max = value;
        current = value;
        UpdateUi();
    }

    public void SetCurrent(int value)
    {
        current = value;
        UpdateUi();
    }

    public void ApplyDamage(int value)
    {
        current -= value;
        UpdateUi();
    }

    private void UpdateUi() => ui.UpdateUi(max, current);
}