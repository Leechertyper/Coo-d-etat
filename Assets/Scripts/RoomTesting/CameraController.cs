using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _mainCam;
    public bool IsMoving { get; private set; }
    
    private void Awake()
    {
        _mainCam = Camera.main;
        
    }

    public void MoveCameraToStart(Transform startRoomPosition)
    {
        _mainCam.transform.position = new Vector3(startRoomPosition.position.x - 1.5f, startRoomPosition.position.y + 2f, -10);
    }

    /// <summary>
    /// Moves the Camera to the room left of the current one
    /// </summary>
    public void MoveLeft()
    {
        if (IsMoving) return;
        IsMoving = true;
        var pos = new Vector3(_mainCam.transform.position.x - FloorConstants.HorizontalRoomOffset, _mainCam.transform.position.y,-10);
        StartCoroutine(MoveToPosition(pos, FloorConstants.TransitionSpeed));
    }
    
    /// <summary>
    /// Moves the Camera to the room right of the current one
    /// </summary>
    public void MoveRight()
    {
        if (IsMoving) return;
        IsMoving = true;
        var pos = new Vector3(_mainCam.transform.position.x + FloorConstants.HorizontalRoomOffset, _mainCam.transform.position.y, -10);
        StartCoroutine(MoveToPosition(pos, FloorConstants.TransitionSpeed));
    }
    
    /// <summary>
    /// Moves the Camera to the room above the current one
    /// </summary>
    public void MoveUp()
    {
        if (IsMoving) return;
        IsMoving = true;
        var pos = new Vector3(_mainCam.transform.position.x, _mainCam.transform.position.y + FloorConstants.VerticalRoomOffset,-10);
        StartCoroutine(MoveToPosition(pos, FloorConstants.TransitionSpeed));
    }
    
    /// <summary>
    /// Moves the Camera to the room below the current one
    /// </summary>
    public void MoveDown()
    {
        if (IsMoving) return;
        IsMoving = true;
        var pos = new Vector3(_mainCam.transform.position.x, _mainCam.transform.position.y - FloorConstants.VerticalRoomOffset,-10);
        StartCoroutine(MoveToPosition(pos, FloorConstants.TransitionSpeed));
    }
    
    /// <summary>
    /// moves the camera to a target position over an amount of seconds
    /// </summary>
    /// <param name="posToMoveTo">Final position</param>
    /// <param name="duration">Amount of time the movement should take (in seconds)</param>
    /// <returns>N/A</returns>
    private IEnumerator MoveToPosition(Vector3 posToMoveTo, float duration) 
    {
        float time = 0;
        var start = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(start, posToMoveTo, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = posToMoveTo;
        IsMoving = false;
    }
}

