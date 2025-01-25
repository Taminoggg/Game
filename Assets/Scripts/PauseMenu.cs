using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static State _state;
    public GameObject pauseMenuGameObject;
    // public GameObject timerGameObject;
    // private  GameObject _timerGameObject;
    private GameObject _gameObject;
    private PlayerCam _playerCam;
    public GameObject _choosePowerUpUI;

    public void SetPlayerCam()
    {
        _gameObject = GameObject.Find("Main Camera");
        _playerCam = _gameObject.GetComponent<PlayerCam>();
        _state = State.Play;
    }
    
    private void Start()
    {
        // SetPlayerCam();
        // _timerGameObject = timerGameObject;
        // _state = State.Play;
        //_choosePowerUpUI = GameObject.FindWithTag("PowerUpScreen");
        _choosePowerUpUI.SetActive(false);
    }

    public static void SetState(State setState)
    {
        _state = setState;
    }
    
    public static State GetState()
    {
        return _state;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.SelectItem:
                _playerCam.enabled = false;
                _choosePowerUpUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case State.Inventory:
                _playerCam.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case State.Pause:
                pauseMenuGameObject.SetActive(true);
                // _timerGameObject.SetActive(false);
                Time.timeScale = 0f;
                _playerCam.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case State.Fail:
                // _timerGameObject.SetActive(false);
                Time.timeScale = 0f;
                _playerCam.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case State.Play:
            default:
                pauseMenuGameObject.SetActive(false);
                //_timerGameObject.SetActive(true);
                Time.timeScale = 1f;
                _choosePowerUpUI.SetActive(false);
                _playerCam.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _state = _state switch
            {
                State.Play => State.Pause,
                State.Pause => State.Play,
                _ => _state
            };
        }
    }

    public void Resume()
    {
        _state = State.Play;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private static string DisplayTime(float timeToDisplay)
    {
        // float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        // float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        // float milliseconds = timeToDisplay % 1 * 100;
        // return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
        var ts = System.TimeSpan.FromSeconds(timeToDisplay);
        return $"{ts:mm\\:ss\\:ff}";

    }
}

public enum State
{
    Play,
    Pause,
    Fail,
    SelectItem,
    Inventory
}