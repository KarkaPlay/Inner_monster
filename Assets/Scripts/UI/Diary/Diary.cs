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
    public GameObject diaryWindow;
    public JsonHandler.Diary diary;
    public GameObject NameCard;
    private string category;
    private List<GameObject> allFiles = new List<GameObject>();

    public GameObject NamesList;
    private bool visible = false;
    private List<GameObject> pagesOfDiary = new List<GameObject>();
    private int currentPageIndex = -1;
    private List<JsonHandler.Paragraph> pinnedParagraphs = new List<JsonHandler.Paragraph>();
    public GameObject pins;
    private List<GameObject> allParagraphs = new List<GameObject>();
    public GameObject paragraphPrefab, paragraphParent;

    private void Start()
    {
        diary = GetComponent<JsonHandler>().load_diary(); //load diary via JsonHandler
        diaryWindow.SetActive(false);
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
            });
            tmp.transform.Find("PinBtn").GetComponent<FileData>().paragraghs = newParagraph;

            TextMeshProUGUI text = tmp.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            text.text = draw[i].Name;
            allFiles.Add(tmp);
        }
    }


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


    private void pinNote(int paragraphIndex)
    {
        Debug.Log(paragraphIndex);
        pinnedParagraphs.Add(EventSystem.current.currentSelectedGameObject.GetComponent<FileData>()
            .paragraghs[paragraphIndex]);

        int currentPage = 0;

        TextMeshProUGUI firstText = pins.transform.Find("firstOnPage").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI secondText = pins.transform.Find("secondOnPage").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI thirdText = pins.transform.Find("thirdOnPage").GetComponent<TextMeshProUGUI>();

        pins.transform.Find("ArrowLeft").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (currentPage == 0)
            {
                currentPage = pinnedParagraphs.Count - 1 - (pinnedParagraphs.Count - 1) % 3;
            }
            else
            {
                currentPage -= 3;
            }

            show_pin_page(currentPage, firstText, secondText, thirdText);
        });

        pins.transform.Find("ArrowRight").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (pinnedParagraphs.Count < currentPage + 4)
            {
                currentPage = 0;
            }
            else
            {
                currentPage += 3;
            }

            show_pin_page(currentPage, firstText, secondText, thirdText);
        });
    }

    //diplays 3 shortTexts of pinnedParagraphs
    void show_pin_page(int page_index, TextMeshProUGUI firstText, TextMeshProUGUI secondText, TextMeshProUGUI thirdText)
    {
        Debug.Log(page_index);
        if (pinnedParagraphs.Count != 0)
        {
            firstText.text = JsonHandler.construct_shortText(pinnedParagraphs[page_index]);
            if (pinnedParagraphs.Count > page_index + 1)
            {
                secondText.text = JsonHandler.construct_shortText(pinnedParagraphs[page_index + 1]);
            }
            else
            {
                secondText.text = "";
            }

            if (pinnedParagraphs.Count > page_index + 2)
            {
                thirdText.text = JsonHandler.construct_shortText(pinnedParagraphs[page_index + 2]);
            }
            else
            {
                thirdText.text = "";
            }
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

        List<JsonHandler.Paragraph> fileData =
            EventSystem.current.currentSelectedGameObject.GetComponent<FileData>().paragraghs;
        for (int i = 0; i < fileData.Count; i++)
        {
            GameObject paragraphObject = Instantiate(paragraphPrefab, paragraphParent.transform);
            allParagraphs.Add(paragraphObject);
            paragraphObject.GetComponent<TextMeshProUGUI>().text = fileData[i].Text;
            paragraphObject.transform.Find("pinParagraph").GetComponent<Button>().onClick.AddListener(() =>
            {
                pinNote(0);
            });
            paragraphObject.transform.Find("pinParagraph").GetComponent<FileData>().paragraghs = fileData;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            visible = !visible;
            diaryActive();
        }
    }
}