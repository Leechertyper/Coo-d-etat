using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenShake : MonoBehaviour
{
    private float _shakeTimeRemaining, _shakePower, _shakeFadeTime;
    private Floor _floor;

    public void StartShake(float length, float power)
    {
        _shakeTimeRemaining = length;
        _shakePower = power;

        _shakeFadeTime = power / length;
    }

    private void LateUpdate()
    {
        if (!(_shakeTimeRemaining > 0)) return;
        
        _shakeTimeRemaining -= Time.deltaTime;

        var xAmount = Random.Range(-1f,1f) * _shakePower;
        var yAmount = Random.Range(-1f,1f) * _shakePower;

        transform.position += new Vector3(xAmount, yAmount, 0);

        _shakePower = Mathf.MoveTowards(_shakePower, 0f, _shakeFadeTime*Time.deltaTime);
    }
}
