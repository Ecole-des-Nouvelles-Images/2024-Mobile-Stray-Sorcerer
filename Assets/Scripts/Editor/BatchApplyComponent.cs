#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UI.Effects;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Editor
{
    public static class BatchApplyComponent
    {
        public static bool LogModifiedObjects = true;

        #region RuntimeInitialization

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnRuntimeInitialization()
        {
            LogModifiedObjects = true;
        }

        #endregion
        
        [MenuItem("Tools/Batch Apply Components/Apply SelectableHighlighter to all Selectables")]
        public static void ApplyHighlighterToAllSelectables()
        {
            Selectable[] selectables = Object.FindObjectsByType<Selectable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<GameObject> modifiedObjects = new();
            
            int count = 0;
            int skippedCount = 0;

            foreach (Selectable selectable in selectables)
            {
                if (selectable.GetComponent<SelectableHighlighter>() || !selectable.interactable)
                {
                    skippedCount++;
                    continue; // Skip if the component already exists
                };
                
                selectable.gameObject.AddComponent<SelectableHighlighter>();
                count++;
                EditorUtility.SetDirty(selectable.gameObject); // Marque l'objet comme modifié (empêche une entrée "Undo")
                modifiedObjects.Add(selectable.gameObject);
            }

            PrintLog(count, modifiedObjects, skippedCount);
            AssetDatabase.SaveAssets();
        }
        
        [MenuItem("Tools/Batch Apply Components/Remove SelectableHighlighter from all Selectables")]
        public static void RemoveHighlighterFromAllSelectables()
        {
            SelectableHighlighter[] highlighters = Object.FindObjectsByType<SelectableHighlighter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<SelectableHighlighter> modifiedComponents = new();
            List<GameObject> modifiedObjects = new();
            
            int count = 0;

            foreach (SelectableHighlighter highlighter in highlighters)
            {
                if (!highlighter || !highlighter.gameObject) continue;
                
                EditorUtility.SetDirty(highlighter.gameObject); // Marque l'objet comme modifié (empêche une entrée "Undo")
                modifiedComponents.Add(highlighter);
                modifiedObjects.Add(highlighter.gameObject);
                count++;
            }

            PrintLog(count, modifiedObjects, 0, true);
            
            foreach (var component in modifiedComponents)
                Object.DestroyImmediate(component);
            
            AssetDatabase.SaveAssets(); // Sauvegarde les modifications
        }

        // --- Option pour afficher/masquer l'arborescence (avec coche) ---
        [MenuItem("Tools/Batch Apply Components/Log Modified Objects", priority = -1)]
        public static void ToggleShowModifiedObjectsHierarchy()
        {
            LogModifiedObjects = !LogModifiedObjects;
        }
        
        #region Validation des menus

        // Validation des menus (désactive le menu si aucun Selectable/SelectableHighlighter n'est trouvé)
        
        [MenuItem("Tools/Batch Apply Components/Apply SelectableHighlighter to all Selectables", true)]
        private static bool ValidateApplyHighlighterToAllSelectables()
        {
            return Object.FindObjectsByType<Selectable>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length > 0;
        }

        [MenuItem("Tools/Batch Apply Components/Remove SelectableHighlighter from all Selectables", true)]
        private static bool ValidateRemoveHighlighterFromAllSelectables()
        {
            return Object.FindObjectsByType<SelectableHighlighter>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length > 0;
        }
        
        [MenuItem("Tools/Batch Apply Components/Log Modified Objects", true)]
        private static bool ValidateToggleShowModifiedObjectsHierarchy()
        {
            Menu.SetChecked("Tools/Batch Apply Components/Log Modified Objects", LogModifiedObjects);
            return true;
        }

        #endregion

        #region LogTree

        private class TreeNode
        {
            public string Name;
            public List<TreeNode> Children = new List<TreeNode>();

            public TreeNode(string name)
            {
                Name = name;
            }
        }
        
        private static TreeNode BuildHierarchyTree(IEnumerable<GameObject> objects)
        {
            TreeNode root = new TreeNode("Root"); // Nœud racine temporaire
            foreach (GameObject obj in objects)
            {
                string[] pathParts = GetObjectFullPath(obj).Split('/');
                TreeNode current = root;

                // Parcourt chaque partie du chemin pour construire l'arbre
                foreach (string part in pathParts)
                {
                    TreeNode child = current.Children.FirstOrDefault(c => c.Name == part);
                    if (child == null)
                    {
                        child = new TreeNode(part);
                        current.Children.Add(child);
                    }
                    current = child;
                }
            }
            return root;
        }
       
        private static string PrintTree(TreeNode node, string prefix = "", bool isLast = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
            sb.Append(isLast ? "└── " : "├── ");
            sb.Append(node.Name);
            sb.AppendLine();

            // Prépare le préfixe pour les enfants
            string newPrefix = prefix + (isLast ? "    " : "│   ");

            // Affiche les enfants
            for (int i = 0; i < node.Children.Count; i++)
            {
                sb.Append(PrintTree(node.Children[i], newPrefix, i == node.Children.Count - 1));
            }

            return sb.ToString();
        }

        private static void PrintLog(int modifiedCount, List<GameObject> modifiedList, int skippedCount = 0, bool pendingRemove = false)
        {
            string log = $"<Editor/Batch Apply Components> [SelectableHighlighter] : " +
                         (modifiedCount > 0 ? $"{(pendingRemove ? $"Remove" : "Add")} component {(pendingRemove ? "from" : "to")} {modifiedCount} GameObjects.\n" : "") +
                         (skippedCount > 0 && !pendingRemove ? $"⚠️ Skipped {skippedCount} GameObjects.\n" : "");
            
            if (LogModifiedObjects && modifiedList.Count > 0)
            {
                TreeNode root = BuildHierarchyTree(modifiedList);
                string hierarchy = PrintTree(root);

                Regex regex = new(@"^└── Root");
                hierarchy = "Modified objects :\n\n" + regex.Replace(hierarchy, "Root", 1);
                log += "\n" + hierarchy;
            }
            
            Debug.Log(log);
        }

        #endregion
        
        private static string GetObjectFullPath(GameObject obj)
        {
            string path = obj.name;
            Transform parent = obj.transform.parent;
            while (parent)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }
    }
}

#endif
