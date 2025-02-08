using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance;

    internal UIShowHide ShowingMainMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    private PlayerCameraInputActionMap _controls;

    // Start is called before the first frame update
    void Awake()
    {
        _controls = new PlayerCameraInputActionMap();
        _controls.MainMenu.Enable();
        _controls.MainMenu.Back.started += CloseCurrentMenu;
    }
    
    private void OnDestroy()
    {
        _controls.MainMenu.Back.started -= CloseCurrentMenu;
        _controls.MainMenu.Disable();
    }

    private void CloseCurrentMenu(InputAction.CallbackContext context)
    {
        if (ShowingMainMenuUI == null) return;

        ShowingMainMenuUI.ButtonPress();
    }
}
