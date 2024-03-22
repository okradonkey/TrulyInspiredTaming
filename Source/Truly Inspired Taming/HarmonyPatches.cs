using System.Reflection;
using HarmonyLib;
using Verse;

// Colonists with Inspired Taming can now tame animals beyond their current skill level.
// Mod settings provide three options to determine how far beyond:
//   - Tame any animal
//   - Boost by percentage
//   - Boost by levels
//
// This does not change a colonist's actual Animals skill level.

// Strategy: Postfix WorkGiver_InteractAnimal.CanInteractWithAnimal

namespace TrulyInspiredTaming;

[StaticConstructorOnStartup]
internal static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("RimWorld.OkraDonkey.TrulyInspiredTaming.main").PatchAll(Assembly.GetExecutingAssembly());
    }
}