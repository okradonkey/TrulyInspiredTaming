using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace Truly_Inspired_Taming
{




    // example settings usage in main mod
    //
    // LoadedModManager.GetMod<Truly_Inspired_Taming>().GetSettings<BoostSettings>().BoostLevels
    //
    // var settings = LoadedModManager.GetMod<Truly_Inspired_Taming>().GetSettings<BoostSettings>();
    // if (!settings.BoostLevels.Contains(name))
    // settings.BoostLevels.Add("c");
    // settings.Write();







    // Colonists with Inspired Taming can tame animals beyond their current skill level.
    //
    // In the mod options you can:
    //   - totally ignore minimum skill requirements for inspired taming, or
    //   - give the inspired colonist an temporary adjustable skill level boost for inspired taming.
    //
    // This does not change a colonist's actual Animals skill level.

    // Strategy: Postfix WorkGiver_InteractAnimal.CanInteractWithAnimal

    // TO DO: 
    // Add skill boost settings
    // Change override to evaluate skill boost

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            // The Harmony_id is only used by other patches that might want to load before/after this one
            Harmony harmonyInstance = new Harmony(id: "RimWorld.OkraDonkey.Truly_Inspired_Taming.main");
            harmonyInstance.PatchAll();
        }
    }

    [HarmonyPatch(typeof(WorkGiver_InteractAnimal), "CanInteractWithAnimal")]
    public static class TamingBoostPatch
    {
        // Original:
        // protected virtual bool CanInteractWithAnimal(Pawn pawn, Pawn animal, bool forced)
        public static void PostFix(ref bool __result, Pawn pawn, Pawn animal, bool forced)
        {
            bool newResult = false;

            // We're only interested in reversing a failure to interact
            if (!__result)
            {
                // If the reason for failure is AnimalsSkillTooLow
                int num = TrainableUtility.MinimumHandlingSkill(animal);
                if (JobFailReason.Reason == "AnimalsSkillTooLow".Translate(num))
                {
                    // If our Interactor has inspired taming
                    if (pawn.InspirationDef == InspirationDefOf.Inspired_Taming)
                    {
                        // override and unfail it
                        newResult = true;

                        // Later: allow smaller skill boost
                        // num adjSkill = pawn.skills.GetSkill(SkillDefOf.Animals).Level + intTrulyInspiredTamingBoost    
                        // if (num <= adjSkill) {newResult = true;}
                    }
                }
            }
            __result = __result || newResult;

        }
    }
}
