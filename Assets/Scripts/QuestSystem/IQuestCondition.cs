public interface IQuestCondition
{
    string Description { get; }
    bool CheckCondition();
    bool IsCompleted { get; } // Теперь это только геттер
}

public abstract class QuestConditionBase : IQuestCondition
{
    public abstract string Description { get; }

    private bool isCompleted = false;
    public bool IsCompleted => isCompleted; // Реализация геттера из интерфейса

    // Проверка условия с установкой флага выполнения
    public bool CheckCondition()
    {
        if (!isCompleted && Check())
        {
            isCompleted = true;
            OnComplete();
        }

        return isCompleted;
    }

    // Абстрактный метод для реализации специфической проверки в наследниках
    protected abstract bool Check();

    // Метод, который вызывается при первом успешном выполнении условия
    protected virtual void OnComplete()
    {
        // По умолчанию ничего не делает, может быть переопределён в наследниках
    }
}