using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class Diary : MonoBehaviour
{
    public JsonHandler.Diary diary;

    //presets
    public GameObject ShortTextsList;

    public GameObject LargeImage;
    public GameObject itemInShortText;
    public GameObject diaryWindow;
    public GameObject NameCard;
    public GameObject pins;
    public GameObject paragraphPrefab, paragraphParent;
    public GameObject NamesList;                                                    //⢿⣿⣿⣿⣭⠹⠛⠛⠛⢿⣿⣿⣿⣿⡿⣿⠷⠶⠿⢻⣿⣛⣦⣙⠻⣿     some
                                                                                    //⣿⣿⢿⣿⠏⠀⠀⡀⠀⠈⣿⢛⣽⣜⠯⣽⠀⠀⠀⠀⠙⢿⣷⣻⡀⢿      variables
    //Diary related                                                                 //⠐⠛⢿⣾⣖⣤⡀⠀⢀⡰⠿⢷⣶⣿⡇⠻⣖⣒⣒⣶⣿⣿⡟⢙⣶⣮
    private string category;                                                        //⣤⠀⠀⠛⠻⠗⠿⠿⣯⡆⣿⣛⣿⡿⠿⠮⡶⠼⠟⠙⠊⠁⠀⠸⢣⣿
    private List<GameObject> allFiles = new List<GameObject>();                     //⣿⣷⡀⠀⠀⠀⠀⠠⠭⣍⡉⢩⣥⡤⠥⣤⡶⣒⠀⠀⠀⠀⠀⢰⣿⣿
    private bool visible = false;                                                   //⣿⣿⡽⡄⠀⠀⠀⢿⣿⣆⣿⣧⢡⣾⣿⡇⣾⣿⡇⠀⠀⠀⠀⣿⡇⠃
    private List<GameObject> pagesOfDiary = new List<GameObject>();                 //⣿⣿⣷⣻⣆⢄⠀⠈⠉⠉⠛⠛⠘⠛⠛⠛⠙⠛⠁⠀⠀⠀⠀⣿⡇⢸
    private List<GameObject> allParagraphs = new List<GameObject>();                //⢞⣿⣿⣷⣝⣷⣝⠦⡀⠀⠀⠀⠀⠀⠀⠀⡀⢀⠀⠀⠀⠀⠀⠛⣿⠈
                                                                                    //⣦⡑⠛⣟⢿⡿⣿⣷⣝⢧⡀⠀⠀⣶⣸⡇⣿⢸⣧⠀⠀⠀⠀⢸⡿⡆
                                                                                    //⣿⣿⣷⣮⣭⣍⡛⠻⢿⣷⠿⣶⣶⣬⣬⣁⣉⣀⣀⣁⡤⢴⣺⣾⣽⡇
    //pins related
    private int currentKeyIndex = 0;
    private List<GameObject> DisplayedPins = new List<GameObject>();
    private Dictionary<string, List<JsonHandler.Paragraph>> pinnedParagraphs = new Dictionary<string, List<JsonHandler.Paragraph>>();
    private List<String> pinnedFiles = new List<String>();


    private void Start()
    {
        diary = JsonHandler.instance.load_diary(); //load diary via JsonHandler
        diaryWindow.SetActive(false);

        //❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️

        //load file data from button
        List<JsonHandler.Paragraph> fileData = new List<JsonHandler.Paragraph>();
        int CategoryNumber = JsonHandler.get_category("Characters");
        int FileNumber = JsonHandler.get_file(CategoryNumber, "Ancient");
        for (int i = 0; i < diary.Categories[1].Files[FileNumber].LongText.Count; i++)
        {
            fileData.Add(diary.Categories[1].Files[FileNumber].LongText[i] );
        }
        //print all files from file data
        for (int i = 0; i < fileData.Count; i++)
        {
            GameObject paragraphObject = Instantiate(paragraphPrefab, paragraphParent.transform); //create a new prefab for paragraph
            allParagraphs.Add(paragraphObject); //add new paragraph to list, so we have a link to it
            paragraphObject.GetComponent<TextMeshProUGUI>().text = fileData[i].Text; //set new paragraph text
        }


        List<JsonHandler.file> draw = JsonHandler.GetAllFiles("Characters"); //get list of new texts to draw

        for (int i = 0; i < draw.Count; i++) //draw new texts
        {
            GameObject tmp = Instantiate(NameCard, NamesList.transform);

            List<JsonHandler.Paragraph> newParagraph = draw[i].LongText;
            //fill pin component data and add listeners
            tmp.transform.Find("Avatar").GetComponent<Button>().onClick.AddListener(() => { showFullText(); });
            tmp.transform.Find("Avatar").GetComponent<FileData>().paragraghs = newParagraph;
            tmp.transform.Find("Avatar").GetComponent<FileData>().name = draw[i].Name;
            //Sprite sprite1 = new Sprite();
            Sprite sprite1 = Resources.Load<Sprite>(draw[i].Name);
            tmp.transform.Find("Avatar").GetComponent<Button>().image.sprite = sprite1;

            tmp.transform.Find("PinBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                for (int j = 0; j < EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().paragraghs.Count; j++)
                {
                    //pin all paragraphs of file
                    pinNote(j, EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().name);
                }
                show_pin_page();
            });
            tmp.transform.Find("PinBtn").GetComponent<FileData>().paragraghs = newParagraph;
            tmp.transform.Find("PinBtn").GetComponent<FileData>().name = draw[i].Name;

            TextMeshProUGUI text = tmp.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            text.text = draw[i].Name;
            allFiles.Add(tmp); //add object to list to have a link
        }



        diaryWindow.transform.Find("preveiwImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("Ancient");

        //❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️❤️

        pins.transform.Find("ArrowLeft").GetComponent<Button>().onClick.AddListener(() =>
        {
            page_left();
        });

        pins.transform.Find("ArrowRight").GetComponent<Button>().onClick.AddListener(() =>
        {
            page_right();
        });
    }


    private void page_left()
    {
        if (currentKeyIndex == 0){return;}//if on first page, pass
        currentKeyIndex--;
        show_pin_page();
    }


    private void page_right()
    {
        if (currentKeyIndex == pinnedFiles.Count-1){return;}//if on last page, pass
        currentKeyIndex++;
        show_pin_page();
    }

    //executed when a category button is pressed, needs to be attached to buttons
    public void OnCategorySelected()
    {
        int length = allFiles.Count;
        for (int i = 0; i < length; i++) //delete old texts
        {
            Destroy(allFiles[0].gameObject);
            allFiles.RemoveAt(0);
        }

        category = EventSystem.current.currentSelectedGameObject.name; //get name of pressed button

        List<JsonHandler.file> draw = JsonHandler.GetAllFiles(category); //get list of new texts to draw

        for (int i = 0; i < draw.Count; i++) //draw new texts
        {
            GameObject tmp = Instantiate(NameCard, NamesList.transform);

            List<JsonHandler.Paragraph> newParagraph = draw[i].LongText;
            //fill pin component data and add listeners
            tmp.transform.Find("Avatar").GetComponent<Button>().onClick.AddListener(() => { showFullText(); });
            tmp.transform.Find("Avatar").GetComponent<FileData>().paragraghs = newParagraph;
            tmp.transform.Find("Avatar").GetComponent<FileData>().name = draw[i].Name;
            //Sprite sprite1 = new Sprite();
            Sprite sprite1 = Resources.Load<Sprite>(draw[i].Name);
            tmp.transform.Find("Avatar").GetComponent<Button>().image.sprite = sprite1;

            tmp.transform.Find("PinBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                for (int j = 0; j < EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().paragraghs.Count; j++)
                {
                    //pin all paragraphs of file
                    pinNote(j, EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().name);
                }
                show_pin_page();
            });
            tmp.transform.Find("PinBtn").GetComponent<FileData>().paragraghs = newParagraph;
            tmp.transform.Find("PinBtn").GetComponent<FileData>().name = draw[i].Name;

            TextMeshProUGUI text = tmp.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            text.text = draw[i].Name;
            allFiles.Add(tmp); //add object to list to have a link
        }
    }

    //switch diary visibility based on 'visible' variable
    private void diaryActive()
    {
        if (visible)
        {
            diaryWindow.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            diaryWindow.SetActive(false);
            Cursor.visible = false;
        }
    }

    //called when we try to pin a note
    private void pinNote(int paragraphIndex, string fileName)
    {
        Debug.Log(paragraphIndex + " " + fileName);
        JsonHandler.Paragraph paragraph_to_pin = EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().paragraghs[paragraphIndex];

        if (!pinnedParagraphs.ContainsKey(fileName))//is there a file with this name in pinnedParagraphs
        {
            List<JsonHandler.Paragraph> new_list = new List<JsonHandler.Paragraph>() {paragraph_to_pin};//create new list with one paragraph in it
            pinnedParagraphs.Add(fileName, new_list);
            pinnedFiles = new List<String>(pinnedParagraphs.Keys); //modify pinnedFiles
            return;
        }
        //is there a paragraph within this files pins
        int tmp_inex = pinnedParagraphs[fileName].FindIndex(paragraph => paragraph.ParagraphID == paragraph_to_pin.ParagraphID); //index of paragraph_to_pin within pinnedParagraphs

        //Debug.Log("Index: " + tmp_inex.ToString());

        if(tmp_inex != -1) //if index != -1 then paragraph_to_pin already exists in pinnedParagraphs, and should be removed
        {
            pinnedParagraphs[fileName].RemoveAt(tmp_inex); //so we remove it
            if (pinnedParagraphs[fileName].Count == 0)
            {
                pinnedParagraphs.Remove(fileName); //if there are no more paragraphs with this fileName pinned, remove fileName
                currentKeyIndex = Math.Max(0, currentKeyIndex - 1); //decrement currentKeyIndex without going below 0
            }
            pinnedFiles = new List<String>(pinnedParagraphs.Keys);//modify pinnedFiles
            return; //return to prevent function from executing further
        }

        pinnedParagraphs[fileName].Add(paragraph_to_pin);

    }

    //diplays pinned paragraphs of related file
    void show_pin_page()
    {
        int length = DisplayedPins.Count;
        for (int i = 0; i < length; i++) //delete old texts
        {
            Destroy(DisplayedPins[0].gameObject);
            DisplayedPins.RemoveAt(0);
        }

        if (pinnedFiles.Count == 0){return;} //if nothing is pinned, pass

        List<JsonHandler.Paragraph> paragraphs = pinnedParagraphs[pinnedFiles[currentKeyIndex]];

        for (int p = 0; p < paragraphs.Count; p++)
        {
            GameObject tmp = Instantiate(itemInShortText, ShortTextsList.transform);
            tmp.transform.Find("textFromDiary").GetComponent<TextMeshProUGUI>().text = JsonHandler.construct_shortText(paragraphs[p]);
            DisplayedPins.Add(tmp);
        }
    }


    private void showFullText()
    {
        int length = allParagraphs.Count;
        for (int i = 0; i < length; i++) //delete old texts
        {
            Destroy(allParagraphs[0].gameObject);
            allParagraphs.RemoveAt(0);
        }
        //load file data from button
        List<JsonHandler.Paragraph> fileData =
            EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().paragraghs;

        diaryWindow.transform.Find("previewImage").GetComponent<Image>().sprite = Resources.Load<Sprite>(EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().name);

        //print all files from file data
        for (int i = 0; i < fileData.Count; i++)
        {
            GameObject paragraphObject = Instantiate(paragraphPrefab, paragraphParent.transform); //create a new prefab for paragraph
            allParagraphs.Add(paragraphObject); //add new paragraph to list, so we have a link to it
            paragraphObject.GetComponent<TextMeshProUGUI>().text = fileData[i].Text; //set new paragraph text
            paragraphObject.transform.Find("pinParagraph").GetComponent<Button>().onClick.AddListener(() =>{pinNote(0, EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().name);show_pin_page();});
            paragraphObject.transform.Find("pinParagraph").GetComponent<FileData>().paragraghs = new List<JsonHandler.Paragraph>();
            //add file data to paragraph             
            paragraphObject.transform.Find("pinParagraph").GetComponent<FileData>().paragraghs.Add(fileData[i]);
            //add single element to list, so we call it on button. slojno obyasnit ya hz             
            paragraphObject.transform.Find("pinParagraph").GetComponent<FileData>().name = EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().name;// мне стыдно
        }
    }

    //eto update, nado obyasnyat'?
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            visible = !visible;
            diaryActive();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            pins.SetActive(!pins.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            page_right();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            page_left();
        }
    }

}
