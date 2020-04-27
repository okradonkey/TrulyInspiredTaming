using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace TrulyInspiredTaming
{
    // Colonists with Inspired Taming can now tame animals beyond their current skill level.
    // Mod settings provide three options to determine how far beyond:
    //   - Tame any animal
    //   - Boost by percentage
    //   - Boost by levels
    //
    // This does not change a colonist's actual Animals skill level.

    // Strategy: Postfix WorkGiver_InteractAnimal.CanInteractWithAnimal

    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            Harmony harmonyInstance = new Harmony(id: "RimWorld.OkraDonkey.TrulyInspiredTaming.main");

            harmonyInstance.Patch(AccessTools.Method(typeof(WorkGiver_InteractAnimal), "CanInteractWithAnimal"),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(RecheckTaming)));
        }

        public static void RecheckTaming(ref bool __result, Pawn pawn, Pawn animal, bool forced)
        {
            bool newResult = false;

            // We're only interested in reversing a failure to interact
            if (!__result)
            {
                // If the reason for failure is AnimalsSkillTooLow
                int animalSkillReq = TrainableUtility.MinimumHandlingSkill(animal);
                if (JobFailReason.Reason == "AnimalsSkillTooLow".Translate(animalSkillReq))
                {
                    // If the colonist has inspired taming
                    if (pawn.InspirationDef == InspirationDefOf.Inspired_Taming)
                    {
                        // Get colonist's current Animal skill level
                        float skillAdj = 0f;
                        int curSkill = pawn.skills.GetSkill(SkillDefOf.Animals).Level;
                        switch (BoostSettings.Boost)
                        {
                            case BoostSettings.BoostType.Unlimited:
                                // Boost skill to max
                                skillAdj = 20 - curSkill;
                                break;
                            case BoostSettings.BoostType.Percentage:
                                // Boost skill by percentage
                                float BoostPerc = BoostSettings.BoostPercentage;
                                skillAdj = curSkill * BoostPerc;
                                break;
                            case BoostSettings.BoostType.Levels:
                                // Boost skill by number of levels
                                float BoostLevels = BoostSettings.BoostLevels;
                                skillAdj = BoostLevels;
                                break;
                        }
                        // Round down the boost
                        int intSkillAdj = (int)Math.Floor(skillAdj);
                        if ( (curSkill + intSkillAdj) >= animalSkillReq)
                        {
                            newResult = true;
                        }
                        else
                        {
                            JobFailReason.Is("TIT_AnimalSkillStillTooLow".Translate(animalSkillReq, curSkill, intSkillAdj), null);
                            newResult = false;
                        }
                    }
                }
            }
            __result = __result || newResult;
        }
    }
}
