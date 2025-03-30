﻿using HarmonyLib;
using System.Text;
using UnityEngine;

namespace CookedInfo
{
    internal static class DescriptionBuilder
    {
        static readonly string red = "#4D0000";
        static readonly string green = "#003300";
        static readonly string yellow = "#CC7F00";
        static readonly string light_blue = "#2861FC";
        static readonly string med_blue = "#1C3270";
        static readonly string dark_blue = "#071336";

        public static void BurnableDescription(ref string description, float amount)
        {
            if (amount > 0f && amount < 1f)
                description = $"{BuildDescription(yellow, description, amount)}";

            if (amount >= 1f && amount < 1.25f)
                description = $"{BuildDescription(green, description, amount)}";

            if (amount >= 1.25f)
                description = $"{BuildDescription(red, $"overcooked {description.Replace("cooked ", "")}", amount)}";
        }

        public static void NonBurnableDescription(ref string description, float amount)
        {
            if (amount > 0f && amount < 1f)
                description = $"{BuildDescription(yellow, description, amount)}";

            if (amount >= 1f)
                description = $"{BuildDescription(green, description, amount)}";
        }        

        private static string BuildDescription(string color, string desc, float amount)
        {
            if (!Plugin.configColoredText.Value)
                return $"{desc}{CookedPercent(amount)}{CookingBar(amount)}";

            return $"{desc}<color={color}>{CookedPercent(amount)}{CookingBar(amount)}</color>";
        }

        private static string CookedPercent(float amount)
        {
            if (!Plugin.configCookingPercent.Value) return "";

            return $"\n<b>{Mathf.RoundToInt(amount * 100)}%</b>";
        }

        private static string CookingBar(float amount)
        {
            if (!Plugin.configCookingBar.Value) return "";

            return $"{ProgressBar(amount)}";
        }

        public static void Freshness(ref string description, float spoiled)
        {
            if (!Plugin.configFreshnessBar.Value) return;
            if (spoiled < 0.33f)
                description = $"{FreshnessBar(light_blue, description, spoiled)}";
            if (spoiled >= 0.33f && spoiled < 0.66f)
                description = $"{FreshnessBar(med_blue, description, spoiled)}";
            if (spoiled >= 0.66f)
                description = $"{FreshnessBar(dark_blue, description, spoiled)}";
        }

        private static string FreshnessBar(string color, string foodDescr, float amount)
        {
            if (!Plugin.configColoredText.Value)
                return $"{foodDescr}{ProgressBar(1f - amount)}"; ;

            return $"{foodDescr}<color={color}>{ProgressBar(1f - amount)}</color>";
        }

        private static string ProgressBar(float amount)
        {   //possibly useful ASCII characters: ████░░░░░░ ████▒▒▒▒ ■□□□□ ▄▄▄... ▀▀    ═══───
            int barLength = 300; // Total length of the loading bar
            int filledLength = (int)(amount * barLength); // Calculate the number of filled characters

            if (filledLength <= 0 || filledLength >= barLength) return ""; //empty string if raw or fully cooked already.

            StringBuilder bar = new StringBuilder();
            for (int i = 0; i < barLength; i++)
            {
                if (i < filledLength)
                {
                    bar.Append("█"); // Filled character
                }
                else
                {
                    bar.Append("░"); // Empty character
                }
            }

            return $"\n<size=3%>{bar}</size>";
        }
    }
}
