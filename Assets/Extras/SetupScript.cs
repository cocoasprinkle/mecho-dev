using System.IO;
using UnityEngine;

public static class SetupScript
{
    public static void CreateDefaultFolders()
    {
        Folders.CreateDefault("_Project", "Animation", "Art", "Materials", "Prefabs", "ScriptableObjects", "Scripts", "Settings");
        UnityEditor.AssetDatabase.Refresh();

    }

    static class Folders
    {
        public static void CreateDefault(string root, params string[] folders)
        {
            var fullpath = Path.Combine(Application.dataPath, root);
            foreach (var folder in folders)
            {
                var path = Path.Combine(fullpath, folder);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }
    }
}
