using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace example_modNS
{
    public class example_mod : Mod
    {
        private void Awake()
        {
            Harmony harmony = new Harmony("example_mod");
            harmony.PatchAll();
        }

        public override void Ready()
        {
            Logger.Log("Ready!");
        }

        
    }

    [HarmonyPatch(typeof(GameScreen), "searchKnowledge")]
    public class SearchHarmonyPatch
    {
        public static bool Prefix(ref bool __result, IKnowledge knowledge, string searchTerm) 
        {
            if (ExampleModSearchMode.searchMode == SearchMode.SearchInIdeaDescription)
            {
                if (knowledge.KnowledgeText.ToLower().Replace(" ", "").Contains(searchTerm.ToLower().Replace(" ", "")))
                {
                    __result = true;
                }
                else
                {
                    __result = false;
                }
            }
            else if (ExampleModSearchMode.searchMode == SearchMode.SearchInIdeaName)
            {
                if (knowledge.KnowledgeName.ToLower().Replace(" ", "").Contains(searchTerm.ToLower().Replace(" ", "")))
                {
                    __result = true;
                }
                else
                {
                    __result = false;
                }
            }

            // return false to replace original Method
            return false;
        }
    }

    [HarmonyPatch(typeof(WorldManager), "Update")]
    public class SearchPatches
    {
        

        
        public static void Postfix()
        {
            if (InputController.instance.GetKeyDown(Key.F) && ExampleModSearchMode.searchMode == SearchMode.SearchInIdeaName)
            {
                ExampleModSearchMode.searchMode = SearchMode.SearchInIdeaDescription;
            }
            else if (InputController.instance.GetKeyDown(Key.F) && ExampleModSearchMode.searchMode == SearchMode.SearchInIdeaDescription)
            {
                ExampleModSearchMode.searchMode = SearchMode.SearchInIdeaName;
            }


            if (
                WorldManager.instance.HoveredCard != null
                && Mouse.current.rightButton.isPressed
            )
            {
                GameScreen.instance.IdeaSearchField.text = WorldManager.instance.HoveredCard.CardData.Name;
                GameScreen.instance.UpdateIdeasLog();
            }
            
        }

        
    }

    public enum SearchMode
    {
        SearchInIdeaName,
        SearchInIdeaDescription
    }

    public static class ExampleModSearchMode
    {
        public static SearchMode searchMode = SearchMode.SearchInIdeaDescription;
    }
}