// using DG.Tweening;

using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    public float xRotation;
    public float yRotation;

    public GameObject player;
    private void Start()
    {
        sensX = PlayerPrefs.GetFloat("sensX",100);
        sensY = PlayerPrefs.GetFloat("sensY",100);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        var mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX;
        var mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        player.transform.rotation =  Quaternion.Euler(0, yRotation, 0);
    }

    // public void DoFov(float endValue)
    // {
    //     GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    // }
    //
    // public void DoTilt(float zTilt)
    // {
    //     transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    // }
}