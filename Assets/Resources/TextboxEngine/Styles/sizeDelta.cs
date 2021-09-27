using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sizeDelta : MonoBehaviour
{
    // Start is called before the first frame update
    RectTransform rectTransform;
    Rect rect;

    public Vector2 rectSize;
    public Vector2 sizDelta;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rect = rectTransform.rect;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rectSize.x = rectTransform.rect.x;
        rectSize.y = rectTransform.rect.y;
        sizDelta = rectTransform.sizeDelta;
    }
}
