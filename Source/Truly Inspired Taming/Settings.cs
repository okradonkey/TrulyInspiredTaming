using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using SettingsHelper;

namespace Truly_Inspired_Taming
{
    public class BoostSettings : ModSettings
    {
        #region BoostTypes
        public static string BoostType = "Tame any animal";
        public static string[] BoostTypes = {"Tame any animal", "Double current skill level", "Add skill levels"};
        public static float BoostLevels = 4;
        #endregion


#if DEBUG
        [TweakValue("AAA_TrulyInspiredTaming")]
#endif        
        private static float yPos = 290f;

        // Value to modify when adding new settings, pushing the scrollview down.
#if DEBUG
        [TweakValue("AAA_TrulyInspiredTaming", max: 500f)]
#endif        
        private static float moreOptionsRecty = 270f;

#if DEBUG
        [TweakValue("AAA_TrulyInspiredTaming")]
#endif        
        private static float widthFiddler = 9f;

        //value to modify when adding more settings to the scrollview.
#if DEBUG
        [TweakValue("AAA_TrulyInspiredTaming", 0, 1200f)]
#endif        
        private static float viewHeight = 650f;

        //Value where the rect stops.
#if DEBUG
        [TweakValue("AAA_TrulyInspiredTaming", 0, 1200f)]
#endif        
        private static float yMax = 620;

        //Do not touch.
#if DEBUG
        [TweakValue("AAA_TrulyInspiredTaming", 0, 1200f)]
#endif        
        private static float height = 640;

        private static Vector2 scrollVector2;


        public void DoWindowContents(Rect wrect)
        {
            Color defaultColor = GUI.color;
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;

            Listing_Standard moreOptions = new Listing_Standard();
            Rect moreOptionsRect = wrect;

            moreOptions.Begin(moreOptionsRect);
            GUI.color = defaultColor;
            moreOptions.AddLabeledRadioList("Inspired Taming Skill Level Boost Method:", BoostTypes, ref BoostType);
            if (BoostType != "Add skill levels")
            {
                GUI.color = Color.grey;
            }
            moreOptions.AddLabeledSlider("Boost Levels", ref BoostLevels, 0, 20, "0", "20", 1, true);
            GUI.color = defaultColor;
            moreOptions.GapLine();
            moreOptions.End();
        }

        // ----------------------------------------
        // Ignore all this - I just some stuff I wanted to keep for reference
        //
        // options.GapLine();
        // options.Gap();
        // options.End();

        //Text.Font = GameFont.Medium;
        //Text.Anchor = TextAnchor.MiddleCenter;
        //GUI.color = Color.yellow;
        //GUI.color = defaultColor;
        // ----------------------------------------



        public override void ExposeData()
        {
            Scribe_Values.Look(ref BoostType, "BoostType", "Unlimited");
            Scribe_Values.Look(ref BoostLevels, "BoostLevels", 4);

        }

    }

    public class Truly_Inspired_Taming : Mod
    {

        public Truly_Inspired_Taming(ModContentPack content) : base(content)
        {
            GetSettings<BoostSettings>();
        }

        public override string SettingsCategory() => "Truly Inspired Taming";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<BoostSettings>().DoWindowContents(inRect);
        }

        // use strings like M4_SettingExtendUndraftTimeBy".Translate() to facilitate translations

        // Box
        // Header                 Truly Inspired Taming
        // Title                  Boost Type:
        // ListSeparator          ---------------------------
        // Radio                  O Unlimited
        // Radio                  O Double Skill
        // Radio with tickbox     O Add ^5v Skill Levels

    }
}