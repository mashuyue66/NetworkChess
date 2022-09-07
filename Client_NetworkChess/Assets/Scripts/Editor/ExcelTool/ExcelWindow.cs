using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExcelWindow : EditorWindow
{
    private const string m_ExcelOpenPath = "";
    private const string m_ExcelDataPath = "";
    private const string m_ExcelClassPath = "";

    private ExcelReader m_ExcelReader;
    private ExcelClassCreator m_ExcelClassCreator;

    private List<string> m_ExcelPaths;
    private List<string> m_ExcelSelectedPaths;
    private Dictionary<string, string> m_FilterRes;

    private void OnEnable()
    {
        m_FilterRes = new Dictionary<string, string> 
        {
            {Application.dataPath.Remove(Application.dataPath.LastIndexOf("/")), ".xlsx,.xls" }
        };

        m_ExcelReader = new ExcelReader();
        m_ExcelClassCreator = new ExcelClassCreator();
        m_ExcelPaths = new List<string>();
        m_ExcelSelectedPaths = new List<string>();
        OpenExcelFolder();
    }

    private void OpenExcelFolder()
    {
        Utils.RecursivePath(m_ExcelOpenPath, m_FilterRes, m_ExcelPaths);

        for (int i = 0; i < m_ExcelPaths.Count; i++)
            if (EditorPrefs.GetBool(m_ExcelPaths[i], false))
                m_ExcelSelectedPaths.Add(m_ExcelPaths[i]);
        EditorUtility.ClearProgressBar();
    }

    private bool Filter(string path)
    {
        if (path.Contains(".meta") || path.Contains("~")) return false;
        if (path.Contains(".xlsx") || path.Contains(".xls"))
        {
            string excelName = path.Replace(m_ExcelOpenPath, "");
            EditorUtility.DisplayProgressBar("Load Excel", string.Format("Loading ... {0}", excelName), 100);
            return true;
        }
        return false;
    }

    private void ShowDirectoryArea()
    {
        GUIStyle helpBoxStyle = EditorStyleUtils.GetHelpBoxStyle();
        EditorGUILayout.BeginVertical(helpBoxStyle);
        {
            GUIStyle textFieldStyle = EditorStyleUtils.GetTextFieldStyle();
            textFieldStyle.fontSize = 13;
            textFieldStyle.alignment = TextAnchor.MiddleLeft;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Export Class : " + m_ExcelClassPath.Replace(Application.dataPath, "Assets"), textFieldStyle, GUILayout.Height(30));
            }
        }
    }
}
