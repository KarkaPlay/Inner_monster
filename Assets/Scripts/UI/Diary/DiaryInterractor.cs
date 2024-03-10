using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryInterractor : MonoBehaviour
{

    public void AddParagraph(JsonHandler.Paragraph paragraph, string file_name, string category_name)
    {
        int CategoryNumber = JsonHandler.get_category(category_name);

        if (CategoryNumber == -1)
        {
            Debug.Log("category does not exist");
            return;
        }

        int FileNumber = JsonHandler.get_file(CategoryNumber, file_name);

        if (FileNumber == -1)
        {
            JsonHandler.add_file(file_name, category_name);
            FileNumber = JsonHandler.get_file(CategoryNumber, file_name);
        }

        int ParagraphNumber = JsonHandler.get_paragraph(CategoryNumber, FileNumber, paragraph.ParagraphID);

        if (ParagraphNumber == -1)
        {
            JsonHandler.add_paragraph(paragraph.Text, paragraph.ParagraphID, paragraph.ShortText, category_name, file_name);
        }
    }

}
