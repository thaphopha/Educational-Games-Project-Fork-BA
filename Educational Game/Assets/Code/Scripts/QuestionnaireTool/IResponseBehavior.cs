// IAnswerBehavior.cs
public interface IResponseBehavior
{
  void DisplayAnswer(QuestionSO questionData);
  void RemoveAnswer(float delay = 0f);
}
