using System;
using System.Collections.Generic;
using UnityEngine; // ��������� ������������ ���� ��� ������� � Debug.Log
using System.Linq;

// ����������� ��������� ������
public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed,
    Failed
}

// ������� ����� Quest
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
        Debug.Log($"����� ������: {Title} - {Description}");
    }

    // ������ ������
    public void StartQuest()
    {
        if (Status == QuestStatus.NotStarted)
        {
            Status = QuestStatus.InProgress;
            Debug.Log($"����� �����: {Title}");
        }
    }

    // �������� ���������� ���� ������� ������
    public void UpdateStatus()
    {
        if (Status == QuestStatus.InProgress)
        {
            foreach (var condition in Conditions)
            {
                if (!condition.CheckCondition()) // ���������, ��������� �� �������
                {
                    return; // ���� �����-�� ������� �� ���������, �������
                }
            }

            // ���������, ��������� �� ��� �������
            if (Conditions.All(c => c.IsCompleted))
            {
                Status = QuestStatus.Completed; // ���� ��� ������� ���������, ����� ��������� �����������
                Debug.Log($"����� ��������: {Title}");
            }
        }
    }

    // ���������� ������� � ������
    public void AddCondition(IQuestCondition condition)
    {
        Conditions.Add(condition);
        Debug.Log($"������� ��������� � ������ '{Title}': {condition.Description}");
    }

    public void RemoveCondition(IQuestCondition condition)
    {
        Conditions.Remove(condition);
        Debug.Log($"������� ������� �� ������ '{Title}': {condition.Description}");
    }

}