using System;
using System.Collections.Generic;
using UnityEngine; // ��������� ������������ ���� ��� ������� � Debug.Log

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
    public QuestStatus Status { get; private set; }
    public List<IQuestCondition> Conditions { get; private set; }

    public Quest(string title, string description)
    {
        Title = title;
        Description = description;
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
                if (!condition.CheckCondition())
                {
                    return; // ���� �����-�� ������� �� ���������, �������
                }
            }

            // ���� ��� ������� ���������, ����� ��������� �����������
            Status = QuestStatus.Completed;
            Debug.Log($"����� ��������: {Title}");
        }
    }

    // ���������� ������� � ������
    public void AddCondition(IQuestCondition condition)
    {
        Conditions.Add(condition);
        Debug.Log($"������� ��������� � ������ '{Title}': {condition.Description}");
    }

}