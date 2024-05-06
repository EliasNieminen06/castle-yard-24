using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExpPickupManager
{
    private static int maxPickups = 100;
    private static List<ExpPickup> expPickups = new List<ExpPickup>();

    public static void AddExpPickup(ExpPickup expPickup)
    {
        if (expPickups.Count >= maxPickups)
        {
            Debug.Log("MaxPickups Reached Absorbing Farthest");
            ExpPickup farthestPickup = FarthestExpPickupFromPlayer;
            expPickup.Absorb(farthestPickup);
        }

        expPickups.Add(expPickup);
    }

    public static void RemoveExpPickup(ExpPickup expPickup)
    {
        expPickups.Remove(expPickup);
    }

    private static ExpPickup ClosestExpPickupToPlayer
    {
        get
        {
            float lowestDist = Mathf.Infinity;
            ExpPickup closestPickup = expPickups[0];

            foreach (var pickup in expPickups)
            {
                float distFromPlayer = pickup.DistanceToPlayer;

                if (distFromPlayer < lowestDist)
                {
                    lowestDist = distFromPlayer;
                    closestPickup = pickup;
                }
            }

            return closestPickup;
        }
    }
    private static ExpPickup FarthestExpPickupFromPlayer
    {
        get
        {
            float highestDist = 0f;
            ExpPickup farthestPickup = expPickups[0];

            foreach (var pickup in expPickups)
            {
                float distFromPlayer = pickup.DistanceToPlayer;

                if (distFromPlayer > highestDist)
                {
                    highestDist = distFromPlayer;
                    farthestPickup = pickup;
                }
            }

            return farthestPickup;
        }
    }
}
