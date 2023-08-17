using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthUI _healthUI;

    private int _max;
    private int _current;

    public void SetMax(int value)
    {
        _max = value;
        UpdateUI();
    }

    public void SetCurrent(int value)
    {
        _current = value;
        UpdateUI();
    }

    public void ApplyDamage(int value)
    {
        SetCurrent(_current - value);
    }

    private void UpdateUI()
    {
        _healthUI.UpdateHealth(_current, _max);
    }
}
