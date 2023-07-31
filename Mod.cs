using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace extra_search_modNS
{
    public class extra_search_mod : Mod
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
    public class SearchKnowledgeHarmonyPatch
    {
        public static bool Prefix(ref bool __result, IKnowledge knowledge, string searchTerm) 
        {
            if (ExtraSearchMode.searchMode == SearchMode.SearchInIdeaDescription)
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
            else if (ExtraSearchMode.searchMode == SearchMode.SearchInIdeaName)
            {
                // original search method
                if (knowledge.KnowledgeName.ToLower().Replace(" ", "").Contains(searchTerm.ToLower().Replace(" ", "")))
                {
                    __result = true;
                }
                else
                {
                    __result = false;
                }
            }

            // return false to replace original method
            return false;
        }
    }

    [HarmonyPatch(typeof(WorldManager), "Update")]
    public class InputSearchHarmonyPatches
    {
        public static void Postfix()
        {
            // Toggle the different searchmodes with F
            if (InputController.instance.GetKeyDown(Key.F) && ExtraSearchMode.searchMode == SearchMode.SearchInIdeaName)
            {
                ExtraSearchMode.searchMode = SearchMode.SearchInIdeaDescription;
            }
            else if (InputController.instance.GetKeyDown(Key.F) && ExtraSearchMode.searchMode == SearchMode.SearchInIdeaDescription)
            {
                ExtraSearchMode.searchMode = SearchMode.SearchInIdeaName;
            }

            // When pressing right mouse button, put the current Cards name into the idea search bar
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

    public static class ExtraSearchMode
    {
        public static SearchMode searchMode = SearchMode.SearchInIdeaDescription;
    }
}