using HarmonyLib;
using RimWorld;
using Verse;

namespace AR_PawnShields;

[HarmonyPatch(typeof(Pawn_EquipmentTracker), nameof(Pawn_EquipmentTracker.MakeRoomFor),
    [typeof(ThingWithComps), typeof(ThingWithComps)], [ArgumentType.Normal, ArgumentType.Out])]
public static class Pawn_EquipmentTracker_MakeRoomFor
{
    public static void Postfix(Pawn_EquipmentTracker __instance, Pawn ___pawn,
        ThingWithComps eq)
    {
        var shieldComp = eq.GetCompShield();
        if (shieldComp == null)
        {
            return;
        }

        //Unequip any existing shield.
        var shield = __instance.GetShield();
        if (shield == null)
        {
            return;
        }

        if (__instance.TryDropEquipment(shield, out var thingWithComps, ___pawn.Position))
        {
            thingWithComps?.SetForbidden(false);
        }
        else
        {
            Log.Error($"{___pawn} couldn't make room for shield {eq}");
        }
    }
}