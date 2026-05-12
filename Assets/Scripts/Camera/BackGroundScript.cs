using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DynamicBackground : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("Насколько сильно фон реагирует на движение камеры. 0 = фон стоит на месте, 1 = двигается как камера.")]
    [Range(0f, 1f)] public float parallaxFactor = 0.3f;

    [Header("Depth Settings")]
    [Tooltip("Глубина Z, чтобы фон не перекрывал платформы (например, 10).")]
    public float backgroundZ = 10f;

    private Transform cam;
    private Vector3 lastCamPos;
    private SpriteRenderer sr;
    private float textureUnitSizeX;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
        sr = GetComponent<SpriteRenderer>();

        // Гарантируем, что фон всегда позади
        transform.position = new Vector3(transform.position.x, transform.position.y, backgroundZ);

        // Размер текстуры (для повторяющегося фона)
        if (sr.sprite != null)
        {
            Texture2D texture = sr.sprite.texture;
            textureUnitSizeX = sr.sprite.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        // Параллакс движение
        Vector3 deltaMovement = cam.position - lastCamPos;
        transform.position -= new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0f);
        lastCamPos = cam.position;

        // Поддерживаем правильную глубину (чтобы не перекрывал платформы)
        if (transform.position.z != backgroundZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, backgroundZ);
        }
    }
}
