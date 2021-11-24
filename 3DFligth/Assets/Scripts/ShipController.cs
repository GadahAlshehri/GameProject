using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{



    public float maxForwardSpeed = 25f;
    public float acceleration = 5f;

    private float activeForwardSpeed;

    public float rollSpeed = 100f;
    public float lookSpeed = 100f;

    private float pitch;
    private float yaw;
    private float roll;
    private Vector2 screenCenter;

    public float maxStrafeSpeed = 25f;

    private float activeStrafeSpeed;

    public float maxHoverSpeed = 25f;
    private float activeHoverSpeed;
    // Start is called before the first frame update
    void Start()
    {
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;

        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        yaw = (Input.mousePosition.x - screenCenter.x) / screenCenter.x;
        pitch = (Input.mousePosition.y - screenCenter.y) / screenCenter.x;
        // the value should be "screenCenter.y" ------------------------^
        Debug.Log("1");
        roll = Mathf.Lerp(roll, Input.GetAxisRaw("Roll"), acceleration * Time.deltaTime);
        Debug.Log("2");
        yaw = Mathf.Clamp(yaw, -1f, 1f);
        pitch = Mathf.Clamp(pitch, -1f, 1f);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed,
            Input.GetAxisRaw("Vertical") * maxForwardSpeed, acceleration * Time.deltaTime);

        //activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed,
        //    Input.GetAxisRaw("Horizontal") * maxStrafeSpeed, acceleration * Time.deltaTime);

        //activeHoverSpeed = Mathf.Lerp(activeHoverSpeed,
        //    Input.GetAxisRaw("Hover") * maxHoverSpeed, acceleration * Time.deltaTime);

        //transform.Rotate(pitch * lookSpeed * Time.deltaTime,
        //    yaw * lookSpeed * Time.deltaTime,
        //    roll * rollSpeed * Time.deltaTime,
        //    Space.Self);

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        //transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;
        //transform.position += transform.up * activeHoverSpeed * Time.deltaTime;
    }















































   /* public float forwardSpeed = 25f;
    public float rollSpeed = 100f;
    public float strafeSpeed = 100f;
    public float hoverSpeed = 100f;
    public float lookSpeed = 100f;
    public bool invertPitchControl = true;

    public float acceleration = 3f;

    private float pitch;
    private float activePitch;
    private float yaw;
    private float activeYaw;
    private float roll;
    private float activeRoll;

    private float activeForwardSpeed;
    private float activeHoverSpeed;
    private float activeStrafeSpeed;

    private Vector2 screenCenter;
    private Vector3 previousMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;

        // Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 mouseMove = Input.mousePosition - previousMousePosition;
        // Vector3 mouseDir = mouseMove.normalized;
        // previousMousePosition = Input.mousePosition;

        // yaw += mouseDir.x;
        // pitch += mouseDir.y;

        //  if pitch is 0 (not moving up or down) 
        //  go back to original

        Debug.Log(Input.GetAxisRaw("Mouse X"));

        // pitch = Mathf.Lerp(pitch, Input.GetAxisRaw("Mouse Y") * 20, acceleration * Time.deltaTime);
        // yaw = Mathf.Lerp(yaw, Input.GetAxisRaw("Mouse X") * 5, acceleration * Time.deltaTime);
        // roll = Mathf.Lerp(roll, Input.GetAxisRaw("Roll"), acceleration * Time.deltaTime);
        roll = Mathf.Lerp(roll, -Input.GetAxisRaw("Mouse X") * 20, acceleration * Time.deltaTime);

        yaw = Mathf.Clamp(yaw, -1f, 1f);
        pitch = Mathf.Clamp(pitch, -1f, 1f);

        activePitch = Mathf.Lerp(activePitch, pitch, acceleration * Time.deltaTime);
        activeYaw = Mathf.Lerp(activeYaw, yaw, acceleration * Time.deltaTime);
        activeRoll = Mathf.Lerp(activeRoll, roll, acceleration * Time.deltaTime);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed,
            Input.GetAxisRaw("Vertical") * forwardSpeed, acceleration * Time.deltaTime);

        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed,
            Input.GetAxisRaw("Hover") * hoverSpeed, acceleration * Time.deltaTime);

        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed,
            Input.GetAxisRaw("Horizontal") * strafeSpeed, acceleration * Time.deltaTime);

        int invertedMultiplier = invertPitchControl ? -1 : 1;

        transform.Rotate(invertedMultiplier * activePitch * lookSpeed * Time.deltaTime,
            activeYaw * lookSpeed * Time.deltaTime,
            activeRoll * rollSpeed * Time.deltaTime,
            Space.Self);

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;
        transform.position += transform.up * activeHoverSpeed * Time.deltaTime;
    }*/
}
