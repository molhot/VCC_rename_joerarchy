using UnityEditor;
using UnityEngine;

namespace Tomoya_2
{
	public partial class TuketeneEditor_2 : EditorWindow
	{
		public string target_top_file;
		public string suffix;
		public int level = 0;
		private string hierarchyText = "";
		private Vector2 scrollPosition = Vector2.zero; // スクロール位置
		public Object top_file;
		public string rename_texts;

		[MenuItem("marimo/marimo")]
		private static void Init()
		{
			EditorWindow.GetWindow(typeof(TuketeneEditor_2));
		}

		private void OnGUI()
        {
            GUILayout.Label("変更したいファイル名につける言葉を入力してください:", EditorStyles.boldLabel);
			suffix = EditorGUILayout.TextField("追加する単語", suffix);
            top_file = EditorGUILayout.ObjectField("最上層のファイル名", top_file, typeof(Object), true);
			GUILayout.Space(10);
        	GUILayout.Label(top_file.name);

            if (GUILayout.Button("Rename GameObjects"))
            {
                GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
				hierarchyText = "";
				foreach (GameObject root in rootObjects)
				{
					if (root.name == top_file.name){
						hierarchyText += GetObjectHierarchy(root, 0);
					}
				}

				rename_texts = "";
				foreach (GameObject root in rootObjects)
				{
					if (root.name == top_file.name){
						rename_texts += AppendSuffix(root, 0, suffix);
					}
				}
			}

			GUILayout.Space(10);
        	GUILayout.Label("ヒエラルキー内容:", EditorStyles.boldLabel);
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
			EditorGUILayout.TextArea(hierarchyText, GUILayout.ExpandHeight(true));
			EditorGUILayout.EndScrollView();

			GUILayout.Space(10);
        	GUILayout.Label("変更後:", EditorStyles.boldLabel);
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
			EditorGUILayout.TextArea(rename_texts, GUILayout.ExpandHeight(true));
			EditorGUILayout.EndScrollView();

			GUILayout.Space(10);
			if (GUILayout.Button("Fixed"))
			{
				GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
				foreach (GameObject root in rootObjects)
				{
					if (root.name == top_file.name){
						RenameHierarchy(root, suffix);
					}
				}
			}
        }

		private string GetObjectHierarchy(GameObject obj, int level)
		{
			string result = new string('-', level * 2) + obj.name + "\n";
			foreach (Transform child in obj.transform)
			{
				result += GetObjectHierarchy(child.gameObject, level + 1);
			}
			return result;
		}

		private string AppendSuffix(GameObject obj, int level, string suffix)
		{
			string result = new string('-', level * 2) + obj.name + suffix +  "\n";
			foreach (Transform child in obj.transform)
			{
				result += AppendSuffix(child.gameObject, level + 1, suffix);
			}
			return result;
		}

		void RenameHierarchy(GameObject obj, string suffix)
		{
			obj.name = obj.name + suffix + "\n";
			foreach (Transform child in obj.transform)
			{
				RenameHierarchy(child.gameObject, suffix);
			}
		}
    }
}
