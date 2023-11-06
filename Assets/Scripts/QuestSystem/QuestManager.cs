using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // Статический экземпляр для доступа к менеджеру квестов
    public static QuestManager Instance { get; private set; }

    private List<Quest> quests = new List<Quest>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Убедитесь, что существует только один экземпляр
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке сцен
        }
    }

    // Метод для добавления нового квеста
    public void AddQuest(Quest newQuest)
    {
        quests.Add(newQuest);
    }

    // Метод для обновления квестов
    public void UpdateQuests()
    {
        foreach (var quest in quests)
        {
            quest.UpdateStatus();
        }
    }

    // Методы для доступа и управления квестами
    // ...

    // Пример метода для получения квеста по названию
    public Quest GetQuestByTitle(string title)
    {
        return quests.Find(quest => quest.Title.Equals(title));
    }

    void Update()
    {
        UpdateQuests();
    }
}
