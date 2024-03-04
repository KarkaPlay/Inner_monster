using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonHandler : MonoBehaviour
{
    public TextAsset diaryJSON;

    [System.Serializable]
    public class shortText
    {
        public string Text;
        public List<string> Points;
    }


    [System.Serializable]
    public class Paragraph
    {
        public int ParagraphID;
        public string Text;
        public shortText ShortText;
    }


    [System.Serializable]
    public class file
    {
        public string Name;
        public string ImagePath;
        public List<Paragraph> LongText;
    }


    [System.Serializable]
    public class Category
    {
        public string Name;
        public List<file> Files;
    }


    [System.Serializable]
    public class Diary
    {
        public List<Category> Categories;
    }


    public static Diary diary = new Diary();

    //singleton setup
    public static JsonHandler instance {get; private set;}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }

        Destroy(this.gameObject);
    }



    //add new file to category / returns null if category was not found
    public static Diary add_file(string name, string CategoryName)
    {
        int CategoryNumber = get_category(CategoryName);

        if (CategoryNumber == -1)
        {
            return null;
        }

        file new_file = new file();
        new_file.Name = name;

        diary.Categories[CategoryNumber].Files.Add(new_file);

        save_diary();
        return diary;
    }

    //add a new paragraph to file in category / returns null if category or file don't exist
    public static Diary add_paragraph(string text, int ID, shortText s_text, string CategoryName, string FileName)
    {
        Paragraph new_paragraph = new Paragraph();
        new_paragraph.ParagraphID = ID;
        new_paragraph.Text = text;
        new_paragraph.ShortText = s_text;

        int CategoryNumber = get_category(CategoryName);
        int FileNumber = get_file(CategoryNumber, FileName);

        if (FileNumber == -1 || CategoryNumber == -1)
        {
            return null;
        }

        diary.Categories[CategoryNumber].Files[FileNumber].LongText.Add(new_paragraph);

        save_diary();
        return diary;
    }

    //remove a paragraph by id / returns null if paragraph was not found
    public static Diary remove_paragraph(int ID)
    {
        for (int c = 0; c < diary.Categories.Count; c++)
        {
            for (int f = 0; f < diary.Categories[c].Files.Count; f++)
            {
                for (int p = 0; p < diary.Categories[c].Files[f].LongText.Count; p++)
                {
                    if (diary.Categories[c].Files[f].LongText[p].ParagraphID == ID)
                    {
                        diary.Categories[c].Files[f].LongText.RemoveAt(p);

                        save_diary();
                        return diary;
                    }
                }
            }
        }

        return null;
    }

    //add point to shortText to file in category / returns null if category or file don't exist
    public static Diary add_short_text_point(string point, string CategoryName, string FileName, int ParagraphID)
    {
        int CategoryNumber = get_category(CategoryName);
        int FileNumber = get_file(CategoryNumber, FileName);
        int ParagraphNumber = get_paragraph(CategoryNumber, FileNumber, ParagraphID);

        if (FileNumber == -1 || CategoryNumber == -1 || ParagraphNumber == -1)
        {
            return null;
        }

        diary.Categories[CategoryNumber].Files[FileNumber].LongText[ParagraphNumber].ShortText.Points.Add(point);

        save_diary();
        return diary;
    }

    //returns a list of all files in category / returns null if category was not found
    public static List<file> GetAllFiles(string CategoryName)
    {
        List<file> files = new List<file>();

        int CategoryNumber = get_category(CategoryName);

        if (CategoryNumber == -1)
        {
            return null;
        }

        for (int f = 0; f < diary.Categories[CategoryNumber].Files.Count; f++)
        {
            files.Add(diary.Categories[CategoryNumber].Files[f]);
        }

        return files;
    }

    //returns index of category by its name / returns -1 if was not found
    public static int get_category(string CategoryName)
    {
        for (int c = 0; c < diary.Categories.Count; c++)
        {
            if (diary.Categories[c].Name == CategoryName)
            {
                return c;
            }
        }

        return -1;
    }

    //returns index of file from category by its name / returns -1 if was not found
    public static int get_file(int CategoryNumber, string FileName)
    {
        for (int f = 0; f < diary.Categories[CategoryNumber].Files.Count; f++)
        {
            if (diary.Categories[CategoryNumber].Files[f].Name == FileName)
            {
                return f;
            }
        }

        return -1;
    }

    //returns indxe of paragraph from file from category by its ID / returns -1 if was not found
    public static int get_paragraph(int CategoryNumber, int FileNumber, int ParagraphID)
    {
        for (int paragraph = 0;
             paragraph < diary.Categories[CategoryNumber].Files[FileNumber].LongText.Count;
             paragraph++)
        {
            if (diary.Categories[CategoryNumber].Files[FileNumber].LongText[paragraph].ParagraphID == ParagraphID)
            {
                return paragraph;
            }
        }

        return -1;
    }

    //returns true if paragraph with this id is present in the diary
    public bool paragraph_id_exists(int ID)
    {
        for (int c = 0; c < diary.Categories.Count; c++)
        {
            for (int f = 0; f < diary.Categories[c].Files.Count; f++)
            {
                for (int p = 0; p < diary.Categories[c].Files[f].LongText.Count; p++)
                {
                    if (diary.Categories[c].Files[f].LongText[p].ParagraphID == ID)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //returns short text as string
    public static string construct_shortText(Paragraph InParagraph)
    {
        string text = InParagraph.ShortText.Text;
        for (int s = 0; s < InParagraph.ShortText.Points.Count; s++)
        {
            text += InParagraph.ShortText.Points[s];
        }

        if (text.Length > 15)
        {
            return text.Substring(0, 15) + "...";
        }

        return text;
    }

    //loads diary from json file and returns it
    public Diary load_diary()
    {
        diary = JsonUtility.FromJson<Diary>(diaryJSON.text);
        return diary;
    }

    //save diary to json file
    public static void save_diary()
    {
        string strOutput = JsonUtility.ToJson(diary, true);
        string path = Application.dataPath + "/Scripts/UI/Diary.json";
        File.WriteAllText(path, strOutput);
    }


    private void Start()
    {
        load_diary();
    }
}

/*
а как пользоваца?


сначала вызываем load_diary(), чтобы закинуть данные из жсона в переменную, это достаточно сделать 1 раз

-----------------------------------------------------------------------------------------------------------
чтобы добавить: (аргументы функций смотри в самих функциях, они выше)
НЕ ОЧЕНЬ ВАЖНО: эти функции вызывают save_diary() сами, так что можно не париться

add_file() - добавить новый ПУСТОЙ файл в категорию
add_paragraph() - добавить новый параграф в файл
add_short_text_point() - добавить дополнение в shortText

-----------------------------------------------------------------------------------------------------------
чтобы редактировать:
ВАЖНО: после редактирования вызываем save_diary() - всегда!!!!!!!

get_category() - вернёт номер категории по её названию. (зачем? чтобы вызвать diary.Categories[get_category()] и получить объект типа Category)

get_file() - вернёт номер файла по его названию и НОМЕРУ категории.
(например:
    CategoryNumber = get_category("Characters");
    FileNumber = get_file(CategoryNumber, "Kuznets");

    diary.Categories[CategoryNumber].Files[FileNumber]; - объект типа file
)

get_paragraph() - вернёт номер параграфа по НОМЕРУ категории, НОМЕРУ файла и ИНДЕКСУ параграфа
(например:
    CategoryNumber = get_category("Characters");
    FileNumber = get_file(CategoryNumber, "Stareyshina");
    ParagraphNumber = get_paragraph(CategoryNumber, FileNumber, ID);

    diary.Categories[CategoryNumber].Files[FileNumber].LongText[ParagraphNumber].Text = new_text; - изменить текс параграфа на new_text
        ИЛИ
    diary.Categories[CategoryNumber].Files[FileNumber].LongText.RemoveAt(ParagraphNumber); - удалить параграф из файла
)

ShortText - переменная внутри параграфа, хранит Text - основной текст и Points - дополнения
diary.Categories[CategoryNumber].Files[FileNumber].LongText[ParagraphNumber].ShortText.Text = new_text; - изменить текст подсказки
--------------------------------------------------------------------------------------------------------
чё ещё есть:

construct_shortText() - создаст строку подсказки со всеми дополнениями

paragraph_id_exists() - вернёт true, если параграф с таким id есть в дневнике

*/
