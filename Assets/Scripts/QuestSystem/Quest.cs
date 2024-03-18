using System;
using System.Collections.Generic;
using UnityEngine; // Добавляем пространство имен для доступа к Debug.Log
using System.Linq;

// Определение состояний квеста
public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed,
    Failed
}

// Базовый класс Quest
[Serializable]
public class Quest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public QuestIDs QuestId { get; set; }
    public QuestStatus Status { get; private set; }
    public List<IQuestCondition> Conditions { get; private set; }

    public Quest(string title, string description, QuestIDs questId)
    {
        Title = title;
        Description = description;
        QuestId = questId;
        Status = QuestStatus.NotStarted;
        Conditions = new List<IQuestCondition>();
        Debug.Log($"Квест создан: {Title} - {Description}");
    }

    // Начало квеста
    public void StartQuest()
    {
        if (Status == QuestStatus.NotStarted)
        {
            Status = QuestStatus.InProgress;
            Debug.Log($"Квест начат: {Title}");
        }
    }

    // Проверка выполнения всех условий квеста
    public void UpdateStatus()
    {
        if (Status == QuestStatus.InProgress)
        {
            foreach (var condition in Conditions)
            {
                if (!condition.CheckCondition()) // Проверяем, выполнено ли условие
                {
                    return; // Если какое-то условие не выполнено, выходим
                }
            }

            // Проверяем, выполнены ли все условия
            if (Conditions.All(c => c.IsCompleted))
            {
                Status = QuestStatus.Completed; // Если все условия выполнены, квест считается завершенным
                Debug.Log($"Квест выполнен: {Title}");
            }
        }
    }

    // Добавление условия к квесту
    public void AddCondition(IQuestCondition condition)
    {
        Conditions.Add(condition);
        Debug.Log($"Условие добавлено к квесту '{Title}': {condition.Description}");
    }

    public void RemoveCondition(IQuestCondition condition)
    {
        Conditions.Remove(condition);
        Debug.Log($"Условие удалено из квеста '{Title}': {condition.Description}");
    }

}