using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // ����������� ��������� ��� ������� � ��������� �������
    public static QuestManager Instance { get; private set; }
    public GameObject player;

    private List<Quest> quests = new List<Quest>();


    public static Transform PlayerTransform
    {
        get
        {
            if (Instance != null && Instance.player.transform != null)
                return Instance.player.transform;
            else
                return null;
        }
    }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ���������, ��� ���������� ������ ���� ���������
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ��� �������� ����
        }
    }

    // ����� ��� ���������� ������ ������
    public void AddQuest(Quest newQuest)
    {
        quests.Add(newQuest);
    }

    // ����� ��� ���������� �������
    public void UpdateQuests()
    {
        foreach (var quest in quests)
        {
            quest.UpdateStatus();
        }
    }

    public Quest GetQuestByTitle(string title)
    {
        return quests.Find(quest => quest.Title.Equals(title));
    }

    public Quest GetQuestById(QuestIDs questId)
    {
        return quests.Find(quest => quest.QuestId.Equals(questId));
    }



    void Update()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
            if (!player) return;
        }
        UpdateQuests();
    }
}
