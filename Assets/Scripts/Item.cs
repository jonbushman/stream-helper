using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.Collections;

public class Item : MonoBehaviour
{
    public TMP_Text CurrentDirectory;
    public Button ChangeDirectoryButton;
    public TMP_InputField InputField;
    public TMP_Dropdown DropDown;
    public Button DecreaseButton;
    public Button IncreaseButton;

    private string _fileDirectory;

    private ErrorWindow _errorWindow;

    public void OnEnable()
    {
        if (DropDown.gameObject.activeSelf)
        {
            DropDown.onValueChanged.AddListener(DropDownChange);
        } else if (IncreaseButton.gameObject.activeSelf)
        {
            InputField.onValueChanged.AddListener(TextChange);
            InputField.enabled = false;

            DecreaseButton.onClick.AddListener(Decrease);
            IncreaseButton.onClick.AddListener(Increase);
        } else
        {
            InputField.onEndEdit.AddListener(TextChange);
        }

        _errorWindow = FindFirstObjectByType<ErrorWindow>();
    }
    public void OnDisable()
    {
        if (DropDown.gameObject.activeSelf)
        {

        }
        else if (IncreaseButton.gameObject.activeSelf)
        {
            InputField.onValueChanged.RemoveListener(TextChange);
            DecreaseButton.onClick.RemoveListener(Decrease);
            IncreaseButton.onClick.RemoveListener(Increase);
        }
        else
        {
            InputField.onEndEdit.RemoveListener(TextChange);
        }
    }

    public void ChangeDirectory()
    {
        StartCoroutine(GoddamnLoadDialogCoroutine());
    }

    private IEnumerator GoddamnLoadDialogCoroutine()
    {

        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Select File", "Select");

        if (FileBrowser.Success)
        {
            _fileDirectory = FileBrowser.Result[0].ToString();
            UpdateDirectoryLabel();
        }
        else Debug.Log("cancelled or something, idk");
    }

    public void UpdateDirectoryLabel()
    {
        var fileName = _fileDirectory.Split("\\");
        CurrentDirectory.text = fileName[fileName.Count() - 1];
    }

    private void WriteChangeToFile(string arg0)
    {
        if (string.IsNullOrEmpty(_fileDirectory) || _fileDirectory == "no directory")
        {
            _errorWindow.DisplayError("Please choose directory first", "No Directory");
        }
        else
        {
            File.WriteAllText(_fileDirectory, arg0);
        }
    }

    private void TextChange(string arg0)
    {
        Debug.Log("attempting to change text");
        WriteChangeToFile(arg0);
    }

    private void DropDownChange(int arg0)
    {
        Debug.Log("attempting to change drop down");
        if (arg0 != 0)
        {
            var val = DropDown.options[arg0].text;
            WriteChangeToFile(val.ToString());
        }
    }

    private void Increase()
    {
        Debug.Log("attempting to increase value");
        InputField.enabled = true;
        int val;
        if (string.IsNullOrEmpty(InputField.text))
        {
            val = 0;
        }
        else
        {
            val = int.Parse(InputField.text);
        }
        val++;
        InputField.text = val.ToString();
        WriteChangeToFile(val.ToString());
        InputField.enabled = false;
    }

    private void Decrease()
    {
        Debug.Log("attempting to decrease value");
        InputField.enabled = true;
        int val;
        if (string.IsNullOrEmpty(InputField.text))
        {
            val = 0;
        }
        else
        {
            val = int.Parse(InputField.text);
        }
        val--;
        InputField.text = val.ToString();
        WriteChangeToFile(val.ToString());
        InputField.enabled = false;
    }

    #region getters and setters for saving and loading
    public string GetDirectory()
    {
        return _fileDirectory;
    }
    public void SetDirectory(string text)
    {
        _fileDirectory = text;
    }
    public string GetInput()
    {
        return InputField.text;
    }
    public void SetInput(string text)
    {
        InputField.text = text;
    }
    #endregion
}
