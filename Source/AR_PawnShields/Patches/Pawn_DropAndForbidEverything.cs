using HarmonyLib;
using Verse;

namespace AR_PawnShields;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.DropAndForbidEverything))]
public static class Pawn_DropAndForbidEverything
{
    public static void Postfix(Pawn __instance)
    {
        if (__instance.InContainerEnclosed && __instance.GetShield() is { } shield)
        {
            __instance.equipment.TryTransferEquipmentToContainer(shield, __instance.holdingOwner);
        }
    }
}