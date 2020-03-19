using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBoxController : MonoBehaviour
{
    public GameObject infoBoxPrefab;
    public GameObject buttonPrefab;
    protected GameObject _infoBox;
    protected GameObject _textObject;
    protected GameObject _exitButtonObject;
    protected Text _messageText;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Initializing Info Box
        _infoBox = Instantiate(infoBoxPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        _infoBox.transform.SetParent(gameObject.transform);

        // Initializing Text
        _textObject = new GameObject();
        _textObject.transform.SetParent(_infoBox.transform);

        _messageText = _textObject.AddComponent<Text>();
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        _messageText.font = ArialFont;
        _messageText.material = ArialFont.material;
        _messageText.fontSize = 20;
        _messageText.transform.SetParent(_infoBox.transform);

        // Set position of text
        RectTransform rectTransform = _messageText.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(400, 200);

        // Initialize and position exit button
        _exitButtonObject = Instantiate(buttonPrefab);
        _exitButtonObject.transform.SetParent(_infoBox.transform);
        _exitButtonObject.GetComponentInChildren<Text>().text = "Close";
        _exitButtonObject.transform.localPosition = new Vector3(140, -135, 0);
        _exitButtonObject.GetComponent<Button>().onClick.AddListener(HideBox);

        // Hide everything
        HideBox();
    }

    public void ShowBox(bool showExitButton)
    {
        _textObject.SetActive(true);
        _infoBox.SetActive(true);
        if (showExitButton)
            _exitButtonObject.SetActive(true);
    }

    public void ShowBox()
    {
        ShowBox(true);
    }

    public void HideBox()
    {
        _textObject.SetActive(false);
        _infoBox.SetActive(false);
        _exitButtonObject.SetActive(false);
        SetText("");
    }

    public virtual void DestroyBox()
    {
        Destroy(_infoBox);
        Destroy(_textObject);
    }

    public void SetText(string message)
    {
        _messageText.text = message;
    }
}
