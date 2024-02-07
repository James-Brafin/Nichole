﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using static CardBrowse;

namespace JamesBrafin.Nichole
{
    [HarmonyPatch]
    internal class StatusController
    {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin))]
        private static void DoEnflame(AAttack __instance, G g, State s, Combat c)
        {
            Ship target = (__instance.targetPlayer ? s.ship : c.otherShip);
            Ship source = (__instance.targetPlayer ? c.otherShip : s.ship);
            if (target == null || source == null || target.hull <= 0 || (__instance.fromDroneX.HasValue && !c.stuff.ContainsKey(__instance.fromDroneX.Value)))
            {
                return;
            }
            if (source.Get(ModEntry.Instance.Enflame.Status) <= 0) return;

            int? num = __instance.GetFromX(s, c);
            RaycastResult? raycastResult;
            if (__instance.fromDroneX.HasValue)
            {
                raycastResult = ((RaycastResult?)CombatUtils.RaycastGlobal(c, target, fromDrone: true, __instance.fromDroneX.Value));
            }
            else
            {
                raycastResult = ((num.HasValue ? CombatUtils.RaycastFromShipLocal(s, c, num.Value, __instance.targetPlayer) : null)); 
            }
            bool flag = true;
            if (target.Get(Status.autododgeLeft) > 0 || target.Get(Status.autododgeRight) > 0)
                flag = false;
            if (raycastResult != null && raycastResult.hitShip && flag)
            {
                c.QueueImmediate(new AStatus()
                {
                    status = Status.heat,
                    statusAmount = source.Get(ModEntry.Instance.Enflame.Status),
                    targetPlayer = __instance.targetPlayer,
                    omitFromTooltips = true,
                });
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin))]
        private static void DoAcidTip(AAttack __instance, G g, State s, Combat c)
        {
            Ship target = (__instance.targetPlayer ? s.ship : c.otherShip);
            Ship source = (__instance.targetPlayer ? c.otherShip : s.ship);
            if (target == null || source == null || target.hull <= 0 || (__instance.fromDroneX.HasValue && !c.stuff.ContainsKey(__instance.fromDroneX.Value)))
            {
                return;
            }
            if (source.Get(ModEntry.Instance.AcidTip.Status) <= 0) return;

            int? num = __instance.GetFromX(s, c);
            RaycastResult? raycastResult;
            if (__instance.fromDroneX.HasValue)
            {
                raycastResult = ((RaycastResult?)CombatUtils.RaycastGlobal(c, target, fromDrone: true, __instance.fromDroneX.Value));
            }
            else
            {
                raycastResult = ((num.HasValue ? CombatUtils.RaycastFromShipLocal(s, c, num.Value, __instance.targetPlayer) : null));
            }
            bool flag = true;
            if (target.Get(Status.autododgeLeft) > 0 || target.Get(Status.autododgeRight) > 0)
                flag = false;
            if (raycastResult != null && raycastResult.hitShip && flag)
            {
                c.QueueImmediate(new AStatus()
                {
                    status = Status.corrode,
                    statusAmount= source.Get(ModEntry.Instance.AcidTip.Status),
                    targetPlayer = __instance.targetPlayer,
                    omitFromTooltips = true,
                });
            }

            ClearStatus(source, ModEntry.Instance.AcidTip.Status);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin))]
        private static void DoAcidSource(AAttack __instance, G g, State s, Combat c)
        {
            Ship target = (__instance.targetPlayer ? s.ship : c.otherShip);
            Ship source = (__instance.targetPlayer ? c.otherShip : s.ship);
            if (target == null || source == null || target.hull <= 0 || (__instance.fromDroneX.HasValue && !c.stuff.ContainsKey(__instance.fromDroneX.Value)))
            {
                return;
            }
            if (source.Get(ModEntry.Instance.AcidSource.Status) <= 0) return;

            int? num = __instance.GetFromX(s, c);
            RaycastResult? raycastResult;
            if (__instance.fromDroneX.HasValue)
            {
                raycastResult = ((RaycastResult?)CombatUtils.RaycastGlobal(c, target, fromDrone: true, __instance.fromDroneX.Value));
            }
            else
            {
                raycastResult = ((num.HasValue ? CombatUtils.RaycastFromShipLocal(s, c, num.Value, __instance.targetPlayer) : null));
            }
            bool flag = true;
            if (target.Get(Status.autododgeLeft) > 0 || target.Get(Status.autododgeRight) > 0)
                flag = false;
            if (raycastResult != null && raycastResult.hitShip && flag)
            {
                c.QueueImmediate(new AStatus()
                {
                    status = Status.corrode,
                    statusAmount = source.Get(ModEntry.Instance.AcidSource.Status),
                    targetPlayer = __instance.targetPlayer,
                    omitFromTooltips = true,
                });
            }

            ClearStatus(source, ModEntry.Instance.AcidTip.Status);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin))]
        private static void DoSuperStun(AAttack __instance, G g, State s, Combat c)
        {
            Ship target = (__instance.targetPlayer ? s.ship : c.otherShip);
            Ship source = (__instance.targetPlayer ? c.otherShip : s.ship);
            if (target == null || source == null || target.hull <= 0 || (__instance.fromDroneX.HasValue && !c.stuff.ContainsKey(__instance.fromDroneX.Value)))
            {
                return;
            }
            if (source.Get(ModEntry.Instance.SuperStun.Status) <= 0) return;

            int? num = __instance.GetFromX(s, c);
            RaycastResult? raycastResult;
            if (__instance.fromDroneX.HasValue)
            {
                raycastResult = ((RaycastResult?)CombatUtils.RaycastGlobal(c, target, fromDrone: true, __instance.fromDroneX.Value));
            }
            else
            {
                raycastResult = ((num.HasValue ? CombatUtils.RaycastFromShipLocal(s, c, num.Value, __instance.targetPlayer) : null));
            }
            bool flag = true;
            if (target.Get(Status.autododgeLeft) > 0 || target.Get(Status.autododgeRight) > 0)
                flag = false;
            if (raycastResult != null && raycastResult.hitShip && flag)
            {
                __instance.stunEnemy = true;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin))]
        private static void DoCryo(AAttack __instance, G g, State s, Combat c)
        {
            Ship ship = (__instance.targetPlayer ? s.ship: c.otherShip);
            if (ship.Get(ModEntry.Instance.Cryo.Status) <= 0) return;

            var baseDamage = __instance.damage;
            __instance.damage = baseDamage + ship.Get(ModEntry.Instance.Cryo.Status);

            ClearStatus(ship, ModEntry.Instance.Cryo.Status);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Combat), nameof(Combat.TryPlayCard))]
        private static void savePotion(Combat __instance, State s, Card card, bool playNoMatterWhatForFree, bool exhaustNoMatterWhat)
        {
            Ship ship = s.ship;
            if (ship.Get(ModEntry.Instance.PotionSaver.Status) <= 0) return;

            if (card.GetMeta().deck == ModEntry.Instance.Potion_Deck.Deck && !exhaustNoMatterWhat)
            {
                Card newCard = card.CopyWithNewId();
                __instance.QueueImmediate(new AAddCard()
                {
                    card = newCard,
                    destination = CardDestination.Discard
                 });

                ship.Set(ModEntry.Instance.PotionSaver.Status, ship.Get(ModEntry.Instance.PotionSaver.Status) - 1);
            }
        }

        public static void ClearStatus(Ship ship, Status status)
        {
            var stacks = ship.Get(status);
            ship.Set(status, 0);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Ship), nameof(Ship.OnBeginTurn))]
        public static void HarmonyPostfix_Status_Cleanup(Ship __instance, State s, Combat c)
        {
            ClearStatus(__instance, ModEntry.Instance.Enflame.Status);
            ClearStatus(__instance, ModEntry.Instance.Cryo.Status);
            ClearStatus(__instance, ModEntry.Instance.AcidTip.Status);
            ClearStatus(__instance, ModEntry.Instance.AcidSource.Status);
            ClearStatus(__instance, ModEntry.Instance.SuperStun.Status);
        }
    }
}
