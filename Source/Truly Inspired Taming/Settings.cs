using SettingsHelper;
using UnityEngine;
using Verse;

namespace TrulyInspiredTaming
{
    internal class BoostSettings : ModSettings
    {
        public enum BoostType : byte
        {
            Unlimited = 0,
            Percentage = 1,
            Levels = 2
        }

        public static BoostType Boost = BoostType.Unlimited;
        public static float BoostPercentage = 0.2f;
        public static float BoostLevels = 4f;

        public void DoWindowContents(Rect canvas)
        {
            var _Listing_Standard = new Listing_Standard
            {
                ColumnWidth = 375
            };
            _Listing_Standard.Begin(canvas);

            Text.Font = GameFont.Medium;
            _Listing_Standard.Label("TIT_TamingSkillBoostType".Translate());
            Text.Font = GameFont.Small;
            _Listing_Standard.Gap();
            _Listing_Standard.Label("TIT_SettingsExplanation".Translate());
            _Listing_Standard.GapLine();
            _Listing_Standard.Label(string.Empty);
            if (_Listing_Standard.RadioButton("TIT_TameAnyAnimal".Translate(), Boost == BoostType.Unlimited))
            {
                Boost = BoostType.Unlimited;
            }

            if (_Listing_Standard.RadioButton("TIT_BoostByPercentage".Translate(), Boost == BoostType.Percentage))
            {
                Boost = BoostType.Percentage;
            }

            if (Boost == BoostType.Percentage)
            {
                _Listing_Standard.AddLabeledSlider($"    +{100 * BoostPercentage}%", ref BoostPercentage, 0, 1, "+0%",
                    "+100%", 0.1f);
            }

            if (_Listing_Standard.RadioButton("TIT_BoostByLevels".Translate(), Boost == BoostType.Levels))
            {
                Boost = BoostType.Levels;
            }

            if (Boost == BoostType.Levels)
            {
                _Listing_Standard.AddLabeledSlider($"    +{BoostLevels}", ref BoostLevels, 0, 20, "+0", "+20", 1);
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

    internal class TrulyInspiredTaming : Mod
    {
        public TrulyInspiredTaming(ModContentPack content) : base(content)
        {
            GetSettings<BoostSettings>();
        }

        public override string SettingsCategory()
        {
            return "TIT_Title".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<BoostSettings>().DoWindowContents(inRect);
        }
    }
}