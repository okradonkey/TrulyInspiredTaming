using System;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

// Colonists with Inspired Taming can now tame animals beyond their current skill level.
// Mod settings provide three options to determine how far beyond:
//   - Tame any animal
//   - Boost by percentage
//   - Boost by levels
//
// This does not change a colonist's actual Animals skill level.

// Strategy: Postfix WorkGiver_InteractAnimal.CanInteractWithAnimal

namespace TrulyInspiredTaming
{
    [StaticConstructorOnStartup]
    internal static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            Harmony harmonyInstance = new Harmony(id: "RimWorld.OkraDonkey.TrulyInspiredTaming.main");

            harmonyInstance.Patch(AccessTools.Method(typeof(WorkGiver_InteractAnimal), "CanInteractWithAnimal"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(RecheckTaming)));
        }

        private static void RecheckTaming(ref bool __result, Pawn pawn, Pawn animal)
        {
            bool newResult = false;

            // We're only interested in reversing a failure to interact
            if (!__result)
            {
                // If the reason for failure is AnimalsSkillTooLow
                int animalSkillReq = TrainableUtility.MinimumHandlingSkill(animal);
                if (JobFailReason.Reason == "AnimalsSkillTooLow".Translate(animalSkillReq))
                {
                    // If the colonist has Inspired Taming
                    if (pawn.InspirationDef == InspirationDefOf.Inspired_Taming)
                    {
                        // Get colonist's current Animal skill level
                        int curSkill = pawn.skills.GetSkill(SkillDefOf.Animals).Level;

                        // How much skill boost should be added by Inspired Taming
                        int skillBoost = 0;
                        switch (BoostSettings.Boost)
                        {
                            case BoostSettings.BoostType.Unlimited:
                                // Boost skill to max
                                skillBoost = 20 - curSkill;
                                break;
                            case BoostSettings.BoostType.Percentage:
                                // Boost skill by percentage
                                skillBoost = (int)Math.Floor(curSkill * BoostSettings.BoostPercentage);
                                break;
                            case BoostSettings.BoostType.Levels:
                                // Boost skill by number of levels
                                skillBoost = (int)BoostSettings.BoostLevels;
                                break;
                        }
                        if ((curSkill + skillBoost) >= animalSkillReq)
                        {
                            // Allow the colonist to tame the animal
                            newResult = true;
                        }
                        else
                        {
                            // The colonist's skill is still too low to tame the animal
                            JobFailReason.Is("TIT_AnimalSkillStillTooLow".Translate(animalSkillReq, curSkill, skillBoost), null);
                            newResult = false;
                        }
                    }
                }
            }
            // Pass the original result, but allow Inspired Taming to override it if applicable
            __result = __result || newResult;
        }
    }
}
