using UnityEngine;
using UnityEngine.Events;

public class QuestionnaireEventManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestionEvent : UnityEvent<string> { }

    [SerializeField] private QuestionnaireManager questionnaireManager;
    public QuestionEvent onQuestionTriggered;

    public void TriggerQuestionEvent(string questionID)
    {
        if (questionID != null)
        {
            questionnaireManager.TriggerQuestion(questionID);
        }
    }
}
