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
            Ship ship = (__instance.targetPlayer ? c.otherShip : s.ship);
            if (ship.Get((Status)MainManifest.statuses["enflame"].Id) <= 0) return;

            c.QueueImmediate(new AStatus
            {
                targetPlayer = !__instance.targetPlayer,
                status = Status.Heat,
                statusAmount = ship.Get((Status)MainManifest.statuses["enflame"].Id)
            });
        }

        public static void ClearStatus(Ship ship, Status status)
        {
            var stacks = ship.Get(status);
            ship.Set(status, 0);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Ship), nameof(Ship.OnBeginTurn))]
        public static void HarmonyPostfix_VowOfCourage_Cleanup(Ship __instance, State s, Combat c)
        {
            ClearStatus(__instance, (Status)MainManifest.statuses["enflame"].Id);
        }
    }
}
