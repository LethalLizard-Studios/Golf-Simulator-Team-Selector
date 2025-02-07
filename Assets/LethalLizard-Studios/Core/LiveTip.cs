public static class LiveTip
{
    public static void SpeakTip(string username, bool isRightHanded, float offline, int carry)
    {
        if (carry < 20)
        {
            WindowsVoice.speak($"Remember {username} keep your eye on the ball");
            return;
        }

        bool isSlice = (isRightHanded && offline > 18) || (!isRightHanded && offline < -18);
        bool isHook = (isRightHanded && offline < -18) || (!isRightHanded && offline > 18);

        if (isSlice)
        {
            WindowsVoice.speak($"Big slice but nice carry {username}");
        }
        else if (isHook)
        {
            WindowsVoice.speak($"Nice carry {username} but you hit a hook");
        }
        else
        {
            WindowsVoice.speak($"Wow {username}, straight as a nail");
        }
    }
}
