using UnityEngine;
using UnityEngine.UI;

public class LineBetweenTwoObjects : MonoBehaviour
{
    private RectTransform object1;
    private RectTransform object2;
    private Image image;
    private RectTransform rectTransform;

    public float OFFSET_NUM = 0f;

    public void SetObjects(GameObject one, GameObject two, Color color)
    {
        object1 = one.GetComponent<RectTransform>();
        object2 = two.GetComponent<RectTransform>();

        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        image.color = color;

        RectTransform aux;
        if (object1.position.x > object2.position.x)
        {
            aux = object1;
            object1 = object2;
            object2 = aux;
        }

        if (object1.gameObject.activeSelf && object2.gameObject.activeSelf)
        {
            rectTransform.position = (object1.position + object2.position) / 2;
            Vector3 dif = object2.position - object1.position;
            rectTransform.sizeDelta = new Vector3(dif.magnitude + OFFSET_NUM, 5);
            rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        }
    }
}
