using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public static Platform Platform { get; private set; }

    public delegate void SelectDog1Event(int index);
    public delegate void SelectDog2Event(int index);
    public event SelectDog1Event OnSelectDogKey;

    public delegate void SelectDogEvent();
    public event SelectDogEvent OnSelectDogMouse;
    public delegate void DogIsMovingEvent(bool isMoving);
    public event DogIsMovingEvent OnDogIsMoving;


    private TouchControls _controls;

    void Awake()
    {
        Instance = this;
        _controls = new TouchControls();
        Platform = Mouse.current == null ? Platform.Mobile : Platform.PC;
    }

    void OnEnable()
    {
        _controls.Enable();
    }

    void OnDisable()
    {
        _controls.Disable();
    }

    void Start()
    {
        //Debug.LogWarning("Remove This when building!!!", this);
        //Platform = Platform.Mobile; // REMOVE THIS ON BUILD!!!

        switch (Platform)
        {
            case Platform.Mobile:
                Debug.Log("Set Mobile Events");
                _controls.TouchPC.TouchPress.started += ctx => MoveDog(ctx);
                _controls.TouchPC.TouchPress.canceled += ctx => StopDog(ctx);
                _controls.TouchPC.TouchSelect.started += ctx => SelectDog(ctx);
                break;
            case Platform.PC:
                _controls.TouchPC.SelectDog1.performed += ctx => SelectDog1(ctx);
                _controls.TouchPC.SelectDog2.performed += ctx => SelectDog2(ctx);
                _controls.TouchPC.MouseSelect.started += ctx => SelectDog(ctx);
                _controls.TouchPC.MouseMove.started += ctx => MoveDog(ctx);
                _controls.TouchPC.MouseMove.canceled += ctx => StopDog(ctx);
                break;
        }
    }

    void SelectDog1(InputAction.CallbackContext context)
    {
        Debug.Log("Selected Dog 0");

        if (OnSelectDogKey == null || Helper.isOverUI(_controls, Platform)) return;

        OnSelectDogKey(0);
    }

    void SelectDog2(InputAction.CallbackContext context)
    {
        

        if (OnSelectDogKey == null || Helper.isOverUI(_controls, Platform)) return;

        Debug.Log("Selected Dog 1");

        OnSelectDogKey(1);
    }

    void SelectDog(InputAction.CallbackContext context)
    {
        

        if (OnSelectDogMouse == null || Helper.isOverUI(_controls, Platform)) return;

        Debug.Log("Selected Dog with Mouse");

        OnSelectDogMouse();
    }

    void MoveDog(InputAction.CallbackContext context)
    {
        

        if (OnDogIsMoving == null || Helper.isOverUI(_controls, Platform)) return;

        Debug.Log("Started moving Dog");

        OnDogIsMoving(true);
    }

    void StopDog(InputAction.CallbackContext context)
    {
        

        if (OnDogIsMoving == null) return;

        Debug.Log("Stopped moving Dog");

        OnDogIsMoving(false);
    }

    public Vector2 GetDogMoveRayOrigin()
    {
        Vector2 position = Vector2.zero;

        if (Platform == Platform.Mobile) position = _controls.TouchPC.TouchPosition.ReadValue<Vector2>();
        else if (Platform == Platform.PC) position = Mouse.current.position.ReadValue();

        return position;
    }
}

public enum Platform
{
    Mobile = 0,
    PC = 1,
}
