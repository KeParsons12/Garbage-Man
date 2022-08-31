using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector] public bool _isGamePaused;

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _camera;

    [Header("Inputs")]
    private PlayerActions _playerInputActions;
    private InputAction _pause;

    private void Awake()
    {
        //Inputs
        _playerInputActions = new PlayerActions();
    }

    private void HandlePause(InputAction.CallbackContext context)
    {
        if (!_isGamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        _isGamePaused = false;
        Time.timeScale = 1f;
        _camera.SetActive(true);
        _pauseMenu.SetActive(false);
    }

    public void PauseGame()
    {
        _isGamePaused = true;
        Time.timeScale = 0f;
        _camera.SetActive(false);
        _pauseMenu.SetActive(true);
    }

    private void OnEnable()
    {
        _pause = _playerInputActions.PlayerControls.Pause;
        _pause.Enable();
        _pause.performed += HandlePause;
    }

    private void OnDisable()
    {
        _pause.Disable();
    }
}
