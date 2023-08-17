using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private RectTransform _filledTransform;

    private float _defaultWidth;
    private float _defaultHeigth;

    private void Init()
    {
        _defaultWidth = _filledTransform.sizeDelta.x;
        _defaultHeigth = _filledTransform.sizeDelta.y;
    }

    public void UpdateHealth(float current, float max)
    {
        if (_defaultWidth == 0) Init();

        float percent = current / max;
        _filledTransform.sizeDelta = new Vector2(_defaultWidth * percent, _defaultHeigth);
    }
}
