public interface IQuestCondition
{
    string Description { get; }
    bool CheckCondition();
    bool IsCompleted { get; } // ������ ��� ������ ������
}

public abstract class QuestConditionBase : IQuestCondition
{
    public abstract string Description { get; }

    private bool isCompleted = false;
    public bool IsCompleted => isCompleted; // ���������� ������� �� ����������

    // �������� ������� � ���������� ����� ����������
    public bool CheckCondition()
    {
        if (!isCompleted && Check())
        {
            isCompleted = true;
            OnComplete();
        }

        return isCompleted;
    }

    // ����������� ����� ��� ���������� ������������� �������� � �����������
    protected abstract bool Check();

    // �����, ������� ���������� ��� ������ �������� ���������� �������
    protected virtual void OnComplete()
    {
        // �� ��������� ������ �� ������, ����� ���� ������������ � �����������
    }
}