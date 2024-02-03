using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

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

            int? num = __instance.fromX;
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
                    targetPlayer = __instance.targetPlayer,
                    omitFromTooltips = true,
                });
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin))]
        private static void DoCryo(AAttack __instance, G g, State s, Combat c)
        {
            Ship ship = (__instance.targetPlayer ? c.otherShip : s.ship);
            if (ModEntry.Instance.Cryo.Status <= 0) return;

            var baseDamage = __instance.damage;
            __instance.damage = baseDamage + ship.Get(ModEntry.Instance.Cryo.Status);

            ClearStatus(ship, ModEntry.Instance.Cryo.Status);
        }

        public static void ClearStatus(Ship ship, Status status)
        {
            var stacks = ship.Get(status);
            ship.Set(status, 0);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Ship), nameof(Ship.OnBeginTurn))]
        public static void HarmonyPostfix_Enflame_Cleanup(Ship __instance, State s, Combat c)
        {
            ClearStatus(__instance, ModEntry.Instance.Enflame.Status);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Ship), nameof(Ship.OnBeginTurn))]
        public static void HarmonyPostfix_Cryo_Cleanup(Ship __instance, State s, Combat c)
        {
            ClearStatus(__instance, ModEntry.Instance.Cryo.Status);
        }
    }
}
