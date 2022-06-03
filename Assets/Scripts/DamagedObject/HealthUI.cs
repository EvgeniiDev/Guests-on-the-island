using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text playerNameText;

    [SerializeField]
    private Text playerHealthText;

    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    PlayerManager target;
    float characterControllerHeight;
    Transform targetTransform;
    Renderer targetRenderer;
    CanvasGroup _canvasGroup;
    Vector3 targetPosition;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        transform.SetParent(GameObject.Find("Canvas")
                                        .GetComponent<Transform>(), false);
    }

    void Update()
    {
        //Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (playerHealthSlider != null)
        {
            playerHealthSlider.value = target._damaged.Health / target._damaged.MaxHealth;
            playerHealthText.text = $"{(int)target._damaged.Health} / {(int)target._damaged.MaxHealth}";
        }

        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;

            transform.position = Camera.main.WorldToScreenPoint(targetPosition);
        }

    }

    public void SetTarget(PlayerManager _target)
    {
        if (_target == null)
            return;

        target = _target;
        targetTransform = target.GetComponent<Transform>();

        var collider = target.GetComponent<Collider>();
        if (collider != null)
            characterControllerHeight = collider.bounds.max.y;
    }
}
