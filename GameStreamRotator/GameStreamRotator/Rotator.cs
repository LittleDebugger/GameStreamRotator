using System;
using System.Runtime.InteropServices;

namespace GameStreamRotator
{
    // Code borrowed from Stefan Wick
    // Published here: https://msdn.microsoft.com/en-us/library/ms812499.aspx
    public class Rotator
    {
        private static int dmDisplayOrientation;
        private DEVMODE dm;

        public Rotator()
        {
            dm = new DEVMODE();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);

            NativeMethods.EnumDisplaySettings(
                null,
                NativeMethods.ENUM_CURRENT_SETTINGS,
                ref dm);

            dmDisplayOrientation = dm.dmDisplayOrientation;
        }

        public void Rotate()
        {
            // swap width and height
            int temp = dm.dmPelsHeight;
            dm.dmPelsHeight = dm.dmPelsWidth;
            dm.dmPelsWidth = temp;

            if (dm.dmDisplayOrientation != dmDisplayOrientation)
            {
                dm.dmDisplayOrientation = dmDisplayOrientation;
            }
            else
            {
                dm.dmDisplayOrientation = NativeMethods.DMDO_DEFAULT;
            }

            NativeMethods.ChangeDisplaySettings(ref dm, 0);
        }
    }

    public class NativeMethods
    {
        // PInvoke declaration for EnumDisplaySettings Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int EnumDisplaySettings(
             string lpszDeviceName,
             int iModeNum,
             ref DEVMODE lpDevMode);

        // PInvoke declaration for ChangeDisplaySettings Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int ChangeDisplaySettings(
             ref DEVMODE lpDevMode,
             int dwFlags);

        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int DMDO_DEFAULT = 0;
        public const int DMDO_90 = 1;
        public const int DMDO_180 = 2;
        public const int DMDO_270 = 3;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;

        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;

        public short dmLogPixels;
        public short dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    }
}
