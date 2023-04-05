using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 pos, int damage)
    {
        var damagePopupTransform = Instantiate(GameAssets.i.DamagePopup, pos, Quaternion.identity);
        var popup = damagePopupTransform.GetComponent<DamagePopup>();
        popup.Setup(damage);

        return popup;
    }

    private Text _t;
    private float _disappearTimer;
    private Color _textColor;

    private void Awake()
    {
        _t = transform.GetComponentInChildren<Text>();
    }

    public void Setup(int damageAmount)
    {
        _t.text = damageAmount.ToString();
        _textColor = _t.color;
        _disappearTimer = 0.5f;
    }

    private void Update()
    {
        var moveYSpeed = 3f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        _disappearTimer -= Time.deltaTime;
        if (_disappearTimer < 0)
        {
            var disappearSpeed = 5f;
            _textColor.a -= disappearSpeed * Time.deltaTime;
            _t.color = _textColor;
            if (_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
