using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PongManager : ResizeableView
{
    protected Vector2 _resolution; // TODO: Determine if this stuff needs to be in the diagrams
    public Text scoreText;

    public GameObject paddlePrefab;
    private GameObject _playerPaddle;
    private GameObject _partnerPaddle;

    public GameObject ball;

    private int _partnerScore;
    private int _playerScore;

    private bool _gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        HARDCODED_RESOLUTION = new Vector2(1510, 755);
        _resolution = HARDCODED_RESOLUTION;

        ResizeUIComponents();
        _resolution = new Vector2(Screen.width, Screen.height);

        scoreText.text = "";
        _gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameStarted)
        {
            if (Screen.width != _resolution.x || Screen.height != _resolution.y)
            {
                ResizeUIComponents();
                _resolution = new Vector2(Screen.width, Screen.height);
            }

            scoreText.text = _playerScore + " - " + _partnerScore;


            if (Input.GetKey(KeyCode.UpArrow))
            {
                MovePlayerPaddle(true);
            } else if (Input.GetKey(KeyCode.DownArrow))
            {
                MovePlayerPaddle(false);
            }





        }
        

        
    }

    //Source: https://answers.unity.com/questions/427612/moving-an-object-in-its-local-xz-relative-to-the-c.html
    private void RotateRelativeToCamera(Vector3 direction, Camera cam, GameObject obj)
    {
        // rotate given direction by the camera's rotation
        Vector3 camDir = cam.transform.rotation * direction;

        // add result to object's location to get relative direction
        Vector3 objectDir = obj.transform.position + camDir;

        // create quaternion facing direction
        Quaternion targetRotation = Quaternion.LookRotation(objectDir - obj.transform.position);

        // constrain rotation to the X axis
        Quaternion constrained = Quaternion.Euler(targetRotation.eulerAngles.x, obj.transform.rotation.eulerAngles.y , obj.transform.rotation.eulerAngles.z);

        // slerp rotation
        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, constrained, Time.deltaTime * 1f);
    }

    void MovePlayerPaddle(bool up)
    {
        Vector3 dir;
        if (up)
        {
            dir = Camera.main.transform.up;
           
        } else
        {
            dir = -Camera.main.transform.up;
            
        }

        dir.Normalize();
        if (_playerPaddle.transform.position.y + dir.y * 1 * Time.deltaTime <= 3.76 && _playerPaddle.transform.position.y + dir.y * 1 * Time.deltaTime >= 3.32)
        {
            _playerPaddle.transform.Translate(dir * 1f * Time.deltaTime);
            RotateRelativeToCamera(dir, Camera.current, _playerPaddle);
        }
        
        
    }

    protected void ResizeUIComponents()
    {
        ResizeText(scoreText, _resolution);

        // TODO: Resize function for a non rect transform object. 
        //ResizeGameObject(_playerPaddle, _resolution);
        //ResizeGameObject(_partnerPaddle, _resolution);
    }

    public void StartGame()
    {
        //TODO
        Debug.Log("Game Starts");
        _gameStarted = true;
        _partnerScore = 0;
        _playerScore = 0;


        _playerPaddle = Instantiate(paddlePrefab);
        _playerPaddle.GetComponent<Transform>().position = new Vector3(-5.62f , 3.53f, 34.89f);
        _playerPaddle.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(14.442f, -19.831f, -4.328f));
        _playerPaddle.GetComponent<Transform>().localScale = new Vector3(0.02f, 0.1f, 0.05f);

        _partnerPaddle = Instantiate(paddlePrefab);





        
    }


    public bool GetPlaying()
    {
        return _gameStarted;
    }
} 

