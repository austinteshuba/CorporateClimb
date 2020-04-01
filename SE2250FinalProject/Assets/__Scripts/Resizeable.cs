using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ResizeableView: MonoBehaviour
{
    protected Vector2 HARDCODED_RESOLUTION = new Vector2(1530f, 765f);

    protected void ResizeText(Text obj, Vector2 resolution)
    {
        RectTransform oldTransform = obj.rectTransform;

        oldTransform.anchoredPosition = new Vector3(oldTransform.anchoredPosition.x / resolution.x * Screen.width, oldTransform.anchoredPosition.y / resolution.y * Screen.height, oldTransform.position.z);
        oldTransform.localScale = new Vector3(Screen.width / HARDCODED_RESOLUTION.x, Screen.height / HARDCODED_RESOLUTION.y, 1f);

        obj.GetComponent<RectTransform>().anchoredPosition = oldTransform.anchoredPosition;
        obj.GetComponent<RectTransform>().localScale = oldTransform.localScale;
    }

    protected void ResizeButton(Button obj, Vector2 resolution)
    {
        RectTransform oldTransform = obj.GetComponent<RectTransform>();

        oldTransform.anchoredPosition = new Vector3(oldTransform.anchoredPosition.x / resolution.x * Screen.width, oldTransform.anchoredPosition.y / resolution.y * Screen.height, oldTransform.position.z);
        oldTransform.localScale = new Vector3(Screen.width / HARDCODED_RESOLUTION.x, Screen.height / HARDCODED_RESOLUTION.y, 1f);

        obj.GetComponent<RectTransform>().anchoredPosition = oldTransform.anchoredPosition;
        obj.GetComponent<RectTransform>().localScale = oldTransform.localScale;
    }

    protected void ResizeGameObject(GameObject obj, Vector2 resolution)
    {
        RectTransform oldTransform = obj.GetComponent<RectTransform>();

        oldTransform.anchoredPosition = new Vector3(oldTransform.anchoredPosition.x / resolution.x * Screen.width, oldTransform.anchoredPosition.y / resolution.y * Screen.height, oldTransform.position.z);
        oldTransform.localScale = new Vector3(Screen.width / HARDCODED_RESOLUTION.x, Screen.height / HARDCODED_RESOLUTION.y, 1f);

        obj.GetComponent<RectTransform>().anchoredPosition = oldTransform.anchoredPosition;
        obj.GetComponent<RectTransform>().localScale = oldTransform.localScale;
    }


}
