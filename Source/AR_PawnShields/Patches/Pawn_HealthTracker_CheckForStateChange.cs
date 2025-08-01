using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AR_PawnShields;

[HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.CheckForStateChange))]
public static class Pawn_HealthTracker_CheckForStateChange
{
    public static void Postfix(Pawn_HealthTracker __instance, Pawn ___pawn)
    {
        if (__instance.Downed || __instance.capacities.CapableOf(PawnCapacityDefOf.Manipulation) ||
            ___pawn.GetShield() is not { } shield)
        {
            return;
        }

        if (___pawn.kindDef.destroyGearOnDrop)
        {
            ___pawn.equipment.DestroyEquipment(shield);
        }
        else if (___pawn.InContainerEnclosed)
        {
            ___pawn.equipment.TryTransferEquipmentToContainer(shield, ___pawn.holdingOwner);
        }
        else if (___pawn.SpawnedOrAnyParentSpawned)
        {
            ___pawn.equipment.TryDropEquipment(shield, out _, ___pawn.PositionHeld);
        }
        else if (___pawn.IsCaravanMember())
        {
            ___pawn.equipment.Remove(shield);
            if (!___pawn.inventory.innerContainer.TryAdd(shield))
            {
                shield.Destroy();
            }
        }
        else
        {
            ___pawn.equipment.DestroyEquipment(shield);
        }
    }
}