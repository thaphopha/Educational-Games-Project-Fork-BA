using UnityEngine;

public class AnswerContainer : MonoBehaviour
{
    [Tooltip("Spacing between answer prefabs.")]
    [SerializeField] private float spacing = 5.0f;

    public void SetPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            transform.position = player.transform.position + new Vector3(0, 30f, 0);
        }
    }

    public void SetPositionOfChildren()
    {
        float totalWidth = (transform.childCount - 1) * spacing;
        float startX = -totalWidth / 2;
        int i = 0;

        foreach (Transform child in transform)
        {
            child.localPosition = new Vector3(startX + i * spacing, child.localPosition.y, 0);
            i++;
        }
    }

    public void RemoveAnswers(float delay = 0f)
    {
        gameObject.SetActive(false);
    }
}
