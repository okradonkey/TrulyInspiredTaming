using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using SettingsHelper;

namespace TrulyInspiredTaming
{
    public class BoostSettings : ModSettings
    {
        public enum BoostType : byte
        {
            Unlimited = 0,
            Percentage = 1,
            Levels = 2,
        }

        #region Options
        public static BoostType Boost = BoostType.Unlimited;
        public static float BoostPercentage = 0.2f;
        public static float BoostLevels = 4f;
        #endregion

        public void DoWindowContents(Rect canvas)
        {
            Listing_Standard _Listing_Standard = new Listing_Standard();
            _Listing_Standard.ColumnWidth = 375;
            _Listing_Standard.Begin(canvas);

            Text.Font = GameFont.Medium;
            _Listing_Standard.Label("TIT_TamingSkillBoostType".Translate(), -1f, null);
            Text.Font = GameFont.Small;
            _Listing_Standard.Gap(12f);
            _Listing_Standard.Label("TIT_SettingsExplanation".Translate());
            _Listing_Standard.GapLine(12f); 
            _Listing_Standard.Label(string.Empty, -1f, null);
            if (_Listing_Standard.RadioButton("TIT_TameAnyAnimal".Translate(), Boost == BoostType.Unlimited, 0f, null))
            {
                Boost = BoostType.Unlimited;
            }
            if (_Listing_Standard.RadioButton("TIT_BoostByPercentage".Translate(), Boost == BoostType.Percentage, 0f, null))
            {
                Boost = BoostType.Percentage;
            }
            if (Boost == BoostType.Percentage)
            {
                _Listing_Standard.AddLabeledSlider("    +" + (100 * BoostPercentage).ToString() + "%", ref BoostPercentage, 0, 1, "+0%", "+100%", 0.1f, false);
            }
            if (_Listing_Standard.RadioButton("TIT_BoostByLevels".Translate(), Boost == BoostType.Levels, 0f, null))
            {
                Boost = BoostType.Levels;
            }
            if (Boost == BoostType.Levels)
            {
                _Listing_Standard.AddLabeledSlider("    +" + BoostLevels.ToString(), ref BoostLevels, 0, 20, "+0", "+20", 1, false);
            }
            _Listing_Standard.End();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref Boost, "Boost", BoostType.Unlimited, true);
            Scribe_Values.Look(ref BoostPercentage, "BoostPercentage", 0.2f, true);
            Scribe_Values.Look(ref BoostLevels, "BoostLevels", 4f, true);
        }
    }

    public class TrulyInspiredTaming : Mod
    {
        public TrulyInspiredTaming(ModContentPack content) : base(content)
        {
            GetSettings<BoostSettings>();
        }

        public override string SettingsCategory() => "TIT_Title".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<BoostSettings>().DoWindowContents(inRect);
        }
    }

}