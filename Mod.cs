using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;

namespace example_modNS
{
    public class example_mod : Mod
    {
        public override void Ready()
        {
            Logger.Log("Ready!");
        }
    }
}