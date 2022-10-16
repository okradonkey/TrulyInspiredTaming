using Mlie;
using UnityEngine;
using Verse;

namespace TrulyInspiredTaming;

internal class TrulyInspiredTaming : Mod
{
    public static string currentVersion;

    public TrulyInspiredTaming(ModContentPack content) : base(content)
    {
        GetSettings<BoostSettings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.TrulyInspiredTaming"));
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