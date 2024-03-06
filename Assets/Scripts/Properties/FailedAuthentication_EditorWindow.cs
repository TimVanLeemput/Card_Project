using UnityEditor;
using UnityEngine;


public class FailedAuthentication_EditorWindow : EditorWindow
{
    private string message = "Authentication Failed! Please try again.";
    private GUIStyle messageStyle;
    private GUIStyle buttonStyle;

    [MenuItem("Window/Failed Authentication Warning")]
    public static void ShowWindow()
    {
        FailedAuthentication_EditorWindow window = GetWindow<FailedAuthentication_EditorWindow>();
        window.titleContent = new GUIContent("Authentication Warning");
        window.minSize = new Vector2(350, 150); // Set width and height
        window.maxSize = new Vector2(350, 150); // Set width and height
        window.Show();
    }

    private void OnGUI()
    {
        if (messageStyle == null)
        {
            InitializeStyles();
        }

        GUILayout.Space(20);
        EditorGUILayout.LabelField(message, messageStyle);

        GUILayout.FlexibleSpace(); // Add flexible space to center the button vertically

        if (GUILayout.Button("OK", buttonStyle, GUILayout.Height(40)))
        {
            this.Close();
        }

        GUILayout.Space(20); // Add space after the button for better spacing
    }

    // Initialize GUI styles
    private void InitializeStyles()
    {
        float _greyScale = 0.36f;
        messageStyle = new GUIStyle(EditorStyles.label);
        messageStyle.alignment = TextAnchor.MiddleCenter;
        messageStyle.normal.textColor = new Color(_greyScale*3, _greyScale*3, _greyScale*3, 0.9f);
        messageStyle.fontSize = 16;
        messageStyle.wordWrap = true;
        messageStyle.padding = new RectOffset(10, 10, 10, 10);

        messageStyle.normal.background = MakeTex(2, 2, new Color(_greyScale, _greyScale, _greyScale, 0.9f)); // Soft red background

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 14;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.4f, 0.6f, 1f));
        buttonStyle.hover.background = MakeTex(2, 2, new Color(0.3f, 0.5f, 0.7f, 1f));
        buttonStyle.active.background = MakeTex(2, 2, new Color(0.1f, 0.3f, 0.5f, 1f));
    }

    // Utility method to create a texture
    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = color;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
