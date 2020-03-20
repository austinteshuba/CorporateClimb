using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This controller handles the box that comes with showing alerts
public class InfoBoxController : MonoBehaviour
{
    // Prefabs to show InfoBox
    public GameObject InfoBoxPrefab;
    public GameObject ButtonPrefab;
    protected GameObject _infoBox;
    protected GameObject _textObject;
    protected GameObject _exitButtonObject;
    protected Text _messageText;
    private GameManager _gameManager;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Initializing Info Box
        _infoBox = Instantiate(InfoBoxPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
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
        rectTransform.localPosition = new Vector3(5, 0, 0);
        rectTransform.sizeDelta = new Vector2(380, 200);

        // Initialize and position exit button
        _exitButtonObject = Instantiate(ButtonPrefab);
        _exitButtonObject.transform.SetParent(_infoBox.transform);
        _exitButtonObject.GetComponentInChildren<Text>().text = "Close";
        _exitButtonObject.transform.localPosition = new Vector3(140, -135, 0);
        _exitButtonObject.GetComponent<Button>().onClick.AddListener(HideBox);

        _gameManager = GameManager.Instance;
        // Hide everything
        HideBox();
    }

    // Show the box
    public void ShowBox(bool showExitButton)
    {
        _textObject.SetActive(true);
        _infoBox.SetActive(true);
        if (showExitButton)
            _exitButtonObject.SetActive(true);
    }
    // convenience handler
    public void ShowBox()
    {
        ShowBox(true);
    }

    // Hide box if needed
    public void HideBox()
    {
        _textObject.SetActive(false);
        _infoBox.SetActive(false);
        _exitButtonObject.SetActive(false);
        SetText("");
        _gameManager.DisableAlert();
    }

    // destroy box if needed
    public virtual void DestroyBox()
    {
        Destroy(_infoBox);
        Destroy(_textObject);
    }

    // Set text in alert
    public void SetText(string message)
    {
        _messageText.text = message;
    }
}
