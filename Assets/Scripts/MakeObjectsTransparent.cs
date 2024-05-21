using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Profiling;
using UnityEngine;

public class MakeObjectsTransparent : MonoBehaviour
{
    private Transform player;

    private List<ObjectFader> currentlyInTheWay = new List<ObjectFader>();
    private List<ObjectFader> currentlyTransparent = new List<ObjectFader>();

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void FixedUpdate()
    {
        GetAllObjectsInTheWay();

        UnFadeObjects();
        FadeObjects();
    }

    private void GetAllObjectsInTheWay()
    {
        currentlyInTheWay.Clear();

        float cameraDistanceFromPlayer = Vector3.Magnitude(transform.position - player.position);

        Ray rayForward = new Ray(transform.position, player.position - transform.position);
        Ray rayBackward = new Ray(player.position, transform.position - player.position);

        var hitsForward = Physics.RaycastAll(rayForward, cameraDistanceFromPlayer);
        var hitsBackward = Physics.RaycastAll(rayBackward, cameraDistanceFromPlayer);

        foreach (var hit in hitsForward)
        {
            if (hit.collider.gameObject.TryGetComponent(out ObjectFader fader))
            {
                if (!currentlyInTheWay.Contains(fader))
                {
                    currentlyInTheWay.Add(fader);
                }
            }
        }

        foreach (var hit in hitsBackward)
        {
            if (hit.collider.gameObject.TryGetComponent(out ObjectFader fader))
            {
                if (!currentlyInTheWay.Contains(fader))
                {
                    currentlyInTheWay.Add(fader);
                }
            }
        }
    }

    private void FadeObjects()
    {
        foreach (var fader in currentlyInTheWay)
        {
            if (!currentlyTransparent.Contains(fader))
            {
                fader.Fade();
                currentlyTransparent.Add(fader);
            }
        }
    }

    private void UnFadeObjects()
    {
        for (int i = currentlyTransparent.Count - 1; i >= 0; i--)
        {
            ObjectFader fader = currentlyTransparent[i];

            if (!currentlyInTheWay.Contains(fader))
            {
                fader.UnFade();
                currentlyTransparent.Remove(fader);
            }
        }
    }
}