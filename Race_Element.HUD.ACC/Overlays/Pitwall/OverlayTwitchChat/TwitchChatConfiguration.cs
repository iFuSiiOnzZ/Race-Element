﻿using RaceElement.HUD.Overlay.Configuration;
using System.Drawing;

namespace RaceElement.HUD.ACC.Overlays.Pitwall.OverlayTwitchChat;

internal class TwitchChatConfiguration : OverlayConfiguration
{
    public TwitchChatConfiguration() => AllowRescale = true;

    [ConfigGrouping("Connection", "Set up the username and O Auth token")]
    public CredentialsGrouping Credentials { get; init; } = new();
    public class CredentialsGrouping
    {
        public string TwitchUser { get; init; } = "";

        [ToolTip("Create an O Auth token at https://twitchapps.com/tmi/ and copy/paste the entire result in here.")]
        [StringOptions(isPassword: true)]
        public string OAuthToken { get; init; } = "";
    }

    [ConfigGrouping("Shape", "Adjust the size of the twitch chat box")]
    public ShapeGrouping Shape { get; init; } = new();
    public class ShapeGrouping
    {
        [IntRange(100, 500, 2)]
        public int Width { get; init; } = 400;

        [IntRange(100, 400, 2)]
        public int Height { get; init; } = 150;
    }

    [ConfigGrouping("Colors", "Adjust the colors of the text and the background")]
    public ColorGrouping Colors { get; init; } = new();
    public class ColorGrouping
    {
        public Color TextColor { get; init; } = Color.FromArgb(255, 255, 255, 255);

        public Color BackgroundColor { get; init; } = Color.FromArgb(170, 0, 0, 0);

        [IntRange(0, 255, 1)]
        public int BackgroundOpacity { get; init; } = 170;

    }
}
