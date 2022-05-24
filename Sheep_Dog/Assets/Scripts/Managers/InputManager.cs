using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; } // VARIABLE FOR SINGLETON
    public static Platform Platform { get; private set; } // VARIABLE FOR DETERMINING CURRENT PLATFORM BEING PLAYED

    public delegate void SelectDogEvent(); // DELEGATE FOR SELECTING DOG WITH A MOUSE
    public event SelectDogEvent OnSelectDogMouse; // EVENT TO TRIGGER SELECTING DOG DELEGATE
    public delegate void DogIsMovingEvent(bool isMoving); // DELEGATE FOR MOVING DOG
    public event DogIsMovingEvent OnDogIsMoving; // EVENT TO TRIGGER MOVING DOG DELEGATE

    public delegate void PauseGameEvent(GameState gameState); // DELEGATE FOR PAUSING GAME
    public event PauseGameEvent OnPauseGame; // EVENT TO TRIGGER PAUSING DELEGATE

    private TouchControls _controls; // VARIABLE FOR HOLDING UNITY CONTROLS WHICH MAP ONTO ALL PLATFORMS

    void Awake()
    {
        Instance = this; // SET SINGLETON TO THIS SCRIPT
        _controls = new TouchControls(); // CREATE NEW CONTROLS
        Platform = Mouse.current == null ? Platform.Mobile : Platform.PC; // DETERMINE WHETHER THIS IS A MOBILE OR PC BUILD
    }

    void OnEnable()
    {
        _controls.Enable(); // ONLY ENABLE CONTROLS WHEN BEING USED
    }

    void OnDisable()
    {
        _controls.Disable(); // ONLY DISABLE CONTROLS WHEN BEING DISCARDED
    }

    void Start()
    {
        //Debug.LogWarning("Remove This when building!!!", this);
        //Platform = Platform.Mobile; // REMOVE THIS ON BUILD!!!

        switch (Platform)
        {
            case Platform.Mobile: // IN CASE OF A MOBILE PLATFORM...
                _controls.TouchPC.TouchPress.started += ctx => SelectDog(ctx); // BIND CONTROL INPUT TO FUNCTION BELOW
                _controls.TouchPC.TouchPress.canceled += ctx => StopDog(ctx); // BIND CONTROL INPUT TO FUNCTION BELOW
                _controls.TouchPC.TouchPress.performed += ctx => MoveDog(ctx); // BIND CONTROL INPUT TO FUNCTION BELOW
                _controls.TouchPC.PausePC.performed += ctx => PauseGame(ctx); // BIND CONTROL INPUT TO FUNCTION BELOW
                break;
            case Platform.PC: // IN CASE OF A MOBILE PLATFORM...
                _controls.TouchPC.MouseSelect.started += ctx => SelectDog(ctx); // BIND CONTROL INPUT TO FUNCTION BELOW
                _controls.TouchPC.MouseMove.started += ctx => MoveDog(ctx); // BIND CONTROL INPUT TO FUNCTION BELOW
                _controls.TouchPC.MouseMove.canceled += ctx => StopDog(ctx); // BIND CONTROL INPUT TO FUNCTION BELOW
                _controls.TouchPC.PausePC.performed += ctx => PauseGame(ctx); // BIND CONTROL INPUT TO FUNCTION BELOW
                break;
        }
    }

    void PauseGame(InputAction.CallbackContext context)
    {
        if (OnPauseGame == null) return; // IF DELEGATE IS EMPTY, DO NOTHING

        OnPauseGame(GameManager.Instance.State); // TRIGGER DELEGATE
    }

    void SelectDog(InputAction.CallbackContext context)
    {
        if (OnSelectDogMouse == null || Helper.isOverUI(_controls, Platform)) return; // IF DELEGATE IS EMPTY, DO NOTHING

        OnSelectDogMouse(); // TRIGGER DELEGATE
    }

    void MoveDog(InputAction.CallbackContext context)
    {
        if (OnDogIsMoving == null || Helper.isOverUI(_controls, Platform)) return; // IF DELEGATE IS EMPTY, DO NOTHING

        OnDogIsMoving(true); // TRIGGER DELEGATE
    }

    void StopDog(InputAction.CallbackContext context)
    {
        if (OnDogIsMoving == null) return; // IF DELEGATE IS EMPTY, DO NOTHING

        OnDogIsMoving(false); // TRIGGER DELEGATE
    }

    public Vector2 GetDogMoveRayOrigin()
    {
        Vector2 position = Vector2.zero; // INITIALISE POSITION VARIABLE

        // GET MOUSE OR TOUCH POSITION BASED ON PLATFORM
        if (Platform == Platform.Mobile) position = _controls.TouchPC.TouchPosition.ReadValue<Vector2>();
        else if (Platform == Platform.PC) position = Mouse.current.position.ReadValue();

        return position; // RETURN POSITION
    }
}

public enum Platform
{
    Mobile = 0,
    PC = 1,
}
