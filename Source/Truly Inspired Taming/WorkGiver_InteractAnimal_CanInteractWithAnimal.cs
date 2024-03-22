using System;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace TrulyInspiredTaming;

[HarmonyPatch(typeof(WorkGiver_InteractAnimal), "CanInteractWithAnimal", typeof(Pawn), typeof(Pawn), typeof(bool))]
public class WorkGiver_InteractAnimal_CanInteractWithAnimal
{
    public static void Postfix(ref bool __result, Pawn pawn, Pawn animal)
    {
        // We're only interested in reversing a failure to interact
        if (__result)
        {
            return;
        }

        // If the reason for failure is AnimalsSkillTooLow
        var animalSkillReq = TrainableUtility.MinimumHandlingSkill(animal);
        if (JobFailReason.Reason != "AnimalsSkillTooLow".Translate(animalSkillReq))
        {
            return;
        }

        // If the colonist has Inspired Taming
        if (pawn.InspirationDef != InspirationDefOf.Inspired_Taming)
        {
            return;
        }

        // Get colonist's current Animal skill level
        var curSkill = pawn.skills.GetSkill(SkillDefOf.Animals).Level;

        // How much skill boost should be added by Inspired Taming
        var skillBoost = 0;
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

        if (curSkill + skillBoost >= animalSkillReq)
        {
            // Allow the colonist to tame the animal
            __result = true;
        }
        else
        {
            // The colonist's skill is still too low to tame the animal
            JobFailReason.Is(
                "TIT_AnimalSkillStillTooLow".Translate(animalSkillReq, curSkill, skillBoost));
        }
    }
}