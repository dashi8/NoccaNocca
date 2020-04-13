using UnityEngine;
using System.Collections;

public class UGuiPositionSetter : MonoBehaviour
{
    public enum PositionX
    {
        LEFT,
        CENTER,
        RIGHT,
        FREE
    };
    public enum PositionY
    {
        TOP,
        MIDDLE,
        BOTTOM,
        FREE
    };

    [SerializeField]
    public PositionX positionX = PositionX.FREE;
    [SerializeField]
    public PositionY positionY = PositionY.FREE;
    public bool isUpdate = true;

    public Vector2 margin;

    private RectTransform _rt;

    void Start()
    {
        _rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isUpdate)
        {
            setPosition();
            isUpdate = false;
        }
    }

    private void setPosition()
    {
        Vector2 pos = _rt.anchoredPosition;
        switch (positionX)
        {
            case PositionX.CENTER:
                pos.x = 0f;
                break;
            case PositionX.LEFT:
                pos.x = -Screen.width * 0.5f + _rt.rect.width * 0.5f + margin.x;
                break;
            case PositionX.RIGHT:
                pos.x = Screen.width * 0.5f - _rt.rect.width * 0.5f - margin.x;
                break;
        }

        switch (positionY)
        {
            case PositionY.MIDDLE:
                pos.y = 0f;
                break;
            case PositionY.BOTTOM:
                pos.y = -Screen.height * 0.5f + _rt.rect.height * 0.5f + margin.y;
                break;
            case PositionY.TOP:
                pos.y = Screen.height * 0.5f - _rt.rect.height * 0.5f - margin.y;
                break;
        }
        _rt.anchoredPosition = pos;
    }
}