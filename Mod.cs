using HarmonyLib;
using System;
using System.Collections;
using TMPro;
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

    [HarmonyPatch(typeof(GameScreen), "Awake")]
    public class SearchModeButtonHarmonyPatches
    {
        public static void Postfix()
        {
            GameObject searchModeBtnObj = GameObject.Instantiate(GameScreen.instance.IdeasButton.gameObject);
            CustomButton searchModeBtn = searchModeBtnObj.GetComponent<CustomButton>();

            // Put the button below the searchfield
            searchModeBtnObj.transform.SetParent(GameScreen.instance.IdeaSearchField.transform.parent.parent);
            searchModeBtnObj.transform.SetSiblingIndex(1);


            searchModeBtn.Image.color = ColorManager.instance.DisabledColor;
            searchModeBtn.TooltipText = "Changes the search mode to either title or description search.";

            // Adding toggel functionality to the Clicked Event
            searchModeBtn.Clicked += delegate
            {
                if (ExtraSearchMode.searchMode == SearchMode.SearchInIdeaName)
                {
                    searchModeBtn.TextMeshPro.text = "Description Search";
                    ExtraSearchMode.searchMode = SearchMode.SearchInIdeaDescription;
                }
                else if (ExtraSearchMode.searchMode == SearchMode.SearchInIdeaDescription)
                {
                    searchModeBtn.TextMeshPro.text = "Title Search";
                    ExtraSearchMode.searchMode = SearchMode.SearchInIdeaName;
                }
                GameScreen.instance.UpdateIdeasLog();
            };

            // save the button as a static ref
            ExtraSearchMode.searchModeButton = searchModeBtn;
        }
    }

    [HarmonyPatch(typeof(GameScreen), "Update")]
    public class ChangeSearchModeButtonTextHarmonyPatches
    {
        public static void Postfix()
        {
            // Need to change the searchmode button text later than Awake or else the text doesnt change
            if (ExtraSearchMode.searchModeButton.TextMeshPro.text == "Ideas")
            {
                ExtraSearchMode.searchModeButton.HardSetText("Description Search");
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
        // need the button on a static level so i can use it in the ChangeSearchModeButtonTextHarmonyPatches class
        public static CustomButton searchModeButton = new CustomButton();
    }
}