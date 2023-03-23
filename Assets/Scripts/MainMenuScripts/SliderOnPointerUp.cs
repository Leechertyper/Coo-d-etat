    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class SliderOnPointerUp : MonoBehaviour, IPointerUpHandler {

        [SerializeField] Slider slider;
        float oldValue;

        void Start() {
            slider = GetComponent<Slider>();
            oldValue = slider.value;
        }

        public void OnPointerUp(PointerEventData eventData) {
            if(slider.value != oldValue) {
                Debug.Log("Slider value changed from " + oldValue + " to " + slider.value);
                AkSoundEngine.PostEvent("Play_Heal", this.gameObject);
                oldValue = slider.value;
            }
        }
    }