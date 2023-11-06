public interface IQuestCondition
{
    string Description { get; }
    bool CheckCondition();
}