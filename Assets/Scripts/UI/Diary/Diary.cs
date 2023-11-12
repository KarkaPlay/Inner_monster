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
    public GameObject diaryWindow;                                                  //⢿⣿⣿⣿⣭⠹⠛⠛⠛⢿⣿⣿⣿⣿⡿⣿⠷⠶⠿⢻⣿⣛⣦⣙⠻⣿     some
    public JsonHandler.Diary diary;                                                 //⣿⣿⢿⣿⠏⠀⠀⡀⠀⠈⣿⢛⣽⣜⠯⣽⠀⠀⠀⠀⠙⢿⣷⣻⡀⢿      variables
    public GameObject NameCard;                                                     //⠐⠛⢿⣾⣖⣤⡀⠀⢀⡰⠿⢷⣶⣿⡇⠻⣖⣒⣒⣶⣿⣿⡟⢙⣶⣮
    private string category;                                                        //⣤⠀⠀⠛⠻⠗⠿⠿⣯⡆⣿⣛⣿⡿⠿⠮⡶⠼⠟⠙⠊⠁⠀⠸⢣⣿
    private List<GameObject> allFiles = new List<GameObject>();                     //⣿⣷⡀⠀⠀⠀⠀⠠⠭⣍⡉⢩⣥⡤⠥⣤⡶⣒⠀⠀⠀⠀⠀⢰⣿⣿
                                                                                    //⣿⣿⡽⡄⠀⠀⠀⢿⣿⣆⣿⣧⢡⣾⣿⡇⣾⣿⡇⠀⠀⠀⠀⣿⡇⠃
    public GameObject NamesList;                                                    //⣿⣿⣷⣻⣆⢄⠀⠈⠉⠉⠛⠛⠘⠛⠛⠛⠙⠛⠁⠀⠀⠀⠀⣿⡇⢸
    private bool visible = false;                                                   //⢞⣿⣿⣷⣝⣷⣝⠦⡀⠀⠀⠀⠀⠀⠀⠀⡀⢀⠀⠀⠀⠀⠀⠛⣿⠈
    private List<GameObject> pagesOfDiary = new List<GameObject>();                 //⣦⡑⠛⣟⢿⡿⣿⣷⣝⢧⡀⠀⠀⣶⣸⡇⣿⢸⣧⠀⠀⠀⠀⢸⡿⡆
    private int currentPageIndex = -1;                                              //⣿⣿⣷⣮⣭⣍⡛⠻⢿⣷⠿⣶⣶⣬⣬⣁⣉⣀⣀⣁⡤⢴⣺⣾⣽⡇
    public List<JsonHandler.Paragraph> pinnedParagraphs = new List<JsonHandler.Paragraph>();
    public GameObject pins;
    private List<GameObject> allParagraphs = new List<GameObject>();
    public GameObject paragraphPrefab, paragraphParent;

    private int currentPage = 0;

    public GameObject firstText;
    public GameObject secondText;
    public GameObject thirdText;

    private void Start()
    {
        diary = GetComponent<JsonHandler>().load_diary(); //load diary via JsonHandler
        diaryWindow.SetActive(false);
        

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
        if (currentPage == 0) //dodge negative list index, set index to last page
        {
            currentPage = pinnedParagraphs.Count - 1 - (pinnedParagraphs.Count - 1) % 3;
        }
        else
        {
            currentPage -= 3;
        }

        show_pin_page();
    }


    private void page_right()
    {
        if (pinnedParagraphs.Count < currentPage + 4) //dodge list index out of range, set index to 0
        {
            currentPage = 0;
        }
        else
        {
            currentPage += 3;
        }

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
            tmp.transform.Find("PinBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                for (int j = 0; j < EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().paragraghs.Count; j++)
                {
                    //pin all paragraphs of file
                    pinNote(j);
                }
                show_pin_page();
            });
            tmp.transform.Find("PinBtn").GetComponent<FileData>().paragraghs = newParagraph;

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
    private void pinNote(int paragraphIndex)
    {
        JsonHandler.Paragraph paragraph_to_pin = EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().paragraghs[paragraphIndex];

        int tmp_inex = pinnedParagraphs.FindIndex(paragraph => paragraph.ParagraphID == paragraph_to_pin.ParagraphID); //index of paragraph_to_pin within pinnedParagraphs

        //Debug.Log("Index: " + tmp_inex.ToString());

        if(tmp_inex != -1) //if index != -1 then paragraph_to_pin already exists in pinnedParagraphs, and should be removed
        {
            pinnedParagraphs.RemoveAt(tmp_inex); //so we remove it
            return; //return to prevent function from executing further
        }

        pinnedParagraphs.Add(paragraph_to_pin);

    }

    //diplays 3 shortTexts of pinnedParagraphs
    void show_pin_page()
    {
        if (pinnedParagraphs.Count != 0)
        {
            if (pinnedParagraphs.Count <= currentPage) {page_left();return;} //if texts from this page were removed, we turn page to the left

            firstText.transform.Find("textFromDiary").GetComponent<TextMeshProUGUI>().text = (pinnedParagraphs.Count > currentPage)
                ? // гугли "тернарный условный оператор", если не понятно, зачем вопросики
                 JsonHandler.construct_shortText(pinnedParagraphs[currentPage]) : "";

            secondText.transform.Find("textFromDiary").GetComponent<TextMeshProUGUI>().text = (pinnedParagraphs.Count > currentPage + 1)
                ?
                JsonHandler.construct_shortText(pinnedParagraphs[currentPage + 1]) : "";

            thirdText.transform.Find("textFromDiary").GetComponent<TextMeshProUGUI>().text = (pinnedParagraphs.Count > currentPage + 2)
                ?
                JsonHandler.construct_shortText(pinnedParagraphs[currentPage + 2]) : "";
        }
        else
        {
            firstText.GetComponent<TextMeshProUGUI>().text = "";
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
        //print all files from file data
        for (int i = 0; i < fileData.Count; i++)
        {
            GameObject paragraphObject = Instantiate(paragraphPrefab, paragraphParent.transform); //create a new prefab for paragraph
            allParagraphs.Add(paragraphObject); //add new paragraph to list, so we have a link to it
            paragraphObject.GetComponent<TextMeshProUGUI>().text = fileData[i].Text; //set new paragraph text
            paragraphObject.transform.Find("pinParagraph").GetComponent<Button>().onClick.AddListener(() =>
            {
                pinNote(0);
                show_pin_page();
            });
            paragraphObject.transform.Find("pinParagraph").GetComponent<FileData>().paragraghs = new List<JsonHandler.Paragraph>(); //add file data to paragraph
            paragraphObject.transform.Find("pinParagraph").GetComponent<FileData>().paragraghs.Add(fileData[i]); //add single element to list, so we call it on button. slojno obyasnit ya hz
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
    }

}
