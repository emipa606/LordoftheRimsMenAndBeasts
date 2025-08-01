using System;
using System.Reflection;
using System.Text;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AR_PawnShields;

/// <summary>
///     Harmony patches, these aren't pretty.
/// </summary>
[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    public static readonly FieldInfo pawnEquipmentField = AccessTools.Field(typeof(Pawn), nameof(Pawn.equipment));
    public static readonly FieldInfo statWorkerStatField = AccessTools.Field(typeof(StatWorker), "stat");

    private static readonly Func<ThingDef, StatDef, bool> GearAffectsStats =
        (Func<ThingDef, StatDef, bool>)AccessTools.Method(typeof(StatWorker), "GearAffectsStat")
            .CreateDelegate(typeof(Func<ThingDef, StatDef, bool>));

    private static readonly Func<Thing, StatDef, string> InfoTextLineFromGear =
        (Func<Thing, StatDef, string>)AccessTools.Method(typeof(StatWorker), "InfoTextLineFromGear")
            .CreateDelegate(typeof(Func<Thing, StatDef, string>));

    static HarmonyPatches()
    {
        new Harmony("AR_jecstools.chjees.shields").PatchAll(Assembly.GetExecutingAssembly());
    }

    public static void StatWorkerInjection_AddShieldValue(ref float result, Pawn_EquipmentTracker equipment,
        StatDef stat)
    {
        var shield = equipment.GetShield();
        if (shield != null)
        {
            result += shield.def.equippedStatOffsets.GetStatOffsetFromList(stat);
        }
    }

    public static void StatWorkerInjection_BuildShieldString(StringBuilder stringBuilder,
        Pawn_EquipmentTracker equipment, StatDef stat)
    {
        var shield = equipment.GetShield();
        if (shield != null && GearAffectsStats(shield.def, stat))
        {
            stringBuilder.AppendLine(InfoTextLineFromGear(shield, stat));
        }
    }
}