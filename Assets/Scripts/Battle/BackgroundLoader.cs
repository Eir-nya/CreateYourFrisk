﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Extremely lazy background loader which is only slightly better than not having a background.
/// Currently attempts to load the 'bg' file from the Sprites folder, otherwise does nothing.
/// Attached to the Background object in the Battle scene.
/// </summary>
public class BackgroundLoader : MonoBehaviour {
    Image bgImage; // Background image.

    // Use this for initialization
    private void Start() {
        bgImage = GetComponent<Image>();
        try {
            // Tries to set the background up.
            Sprite bg = SpriteUtil.FromFile(FileLoader.pathToModFile("Sprites/bg.png"));
            if (bg != null) {
                bg.texture.filterMode = FilterMode.Point;
                bgImage.sprite = bg;
                bgImage.color = Color.white;
            }
        } catch {
            // Background failed loading, no need to do anything.
            UnitaleUtil.WriteInLogAndDebugger("[WARN]No background file found. Using empty background.");
        }
    }
}