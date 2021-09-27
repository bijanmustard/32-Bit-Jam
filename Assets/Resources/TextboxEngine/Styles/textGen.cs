using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class textGen : MonoBehaviour
{
    Text myTextString;
    TextGenerationSettings settings;
    RectTransform myRect, parentRect;
    TextGenerator cached, cachedLayout;
    [Header("Rect Size")]
    public Vector2 size;
    [Header("Preferred Rect Size")]
    public float preferredWidth;
    public float preferredHeight;
    [Header("Cached TextGen - Preferred Size")]
    public float cachedPreferredWidth;
    public float cachedPreferredHeight;
    [Header("Cached Textgen For Layout - Preferred Size")]
    public float layoutPreferredWidth;
    public float layoutPreferredHeight;
    

    private void Awake()
    {
        myTextString = GetComponent<Text>();
        myRect = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        cached = myTextString.cachedTextGenerator;
        cachedLayout = myTextString.cachedTextGeneratorForLayout;
    }
    // Update is called once per frame
    void Update()
    {
        size = new Vector3(myRect.rect.width, myRect.rect.height);

        TextGenerationSettings settings = new TextGenerationSettings();
        settings.font = myTextString.font;
        settings.fontSize = myTextString.fontSize;
        settings.generationExtents = size;
        settings.richText = myTextString.supportRichText;
        settings.verticalOverflow = myTextString.verticalOverflow;
        settings.textAnchor = myTextString.alignment;

        preferredWidth = LayoutUtility.GetPreferredWidth(myRect);
        preferredHeight = LayoutUtility.GetPreferredHeight(myRect);
        cachedPreferredWidth = cached.GetPreferredWidth(myTextString.text, settings);
        cachedPreferredHeight = cached.GetPreferredHeight(myTextString.text, settings);
        layoutPreferredWidth = cachedLayout.GetPreferredWidth(myTextString.text, settings);
        layoutPreferredHeight = cachedLayout.GetPreferredHeight(myTextString.text, settings);
    }
}
