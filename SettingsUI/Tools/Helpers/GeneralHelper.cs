using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Reflection;
using Windows.Networking.Connectivity;
using Windows.UI;

namespace SettingsUI.Helpers
{
    public static class GeneralHelper
    {
        public static void EnableSound(ElementSoundPlayerState elementSoundPlayerState = ElementSoundPlayerState.On, bool withSpatial = false)
        {
            ElementSoundPlayer.State = elementSoundPlayerState;

            if (!withSpatial)
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
            else
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
        }

        public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }

        public static int GetThemeIndex(ElementTheme elementTheme)
        {
            switch (elementTheme)
            {
                case ElementTheme.Default:
                    return 0;
                case ElementTheme.Light:
                    return 1;
                case ElementTheme.Dark:
                    return 2;
                default:
                    return 0;
            }
        }
        public static ElementTheme GetElementThemeEnum(int themeIndex)
        {
            switch (themeIndex)
            {
                case 0:
                    return ElementTheme.Default;
                case 1:
                    return ElementTheme.Light;
                case 2:
                    return ElementTheme.Dark;
                default:
                    return ElementTheme.Default;
            }
        }
        public static bool IsNetworkAvailable()
        {
            return NetworkInformation.GetInternetConnectionProfile()?.NetworkAdapter != null;
        }
        public static Geometry GetGeometry(string key)
        {
            return (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), (string)Application.Current.Resources[key]);
        }
        public static Color GetColorFromHex(string hexaColor)
        {
            return
                Color.FromArgb(
                  Convert.ToByte(hexaColor.Substring(1, 2), 16),
                    Convert.ToByte(hexaColor.Substring(3, 2), 16),
                    Convert.ToByte(hexaColor.Substring(5, 2), 16),
                    Convert.ToByte(hexaColor.Substring(7, 2), 16)
                );
        }
    }
}
