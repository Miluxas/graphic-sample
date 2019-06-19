using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xPFT.GraphicEngine
{
    public enum WINDOWS_NAME
    {
        WIN81 = 63,
        WIN8 = 62,
        WIN7 = 61,
        WINVista = 60,
        WINXP64_Bit = 52,
        WINXP = 51,
        WIN2000 = 50
    }


    public class OSInfo
    {
        static System.OperatingSystem opeatingSys = System.Environment.OSVersion;
        public static WINDOWS_NAME WIN_TYPE
        {

            get
            {
                int major = opeatingSys.Version.Major;
                int minor = opeatingSys.Version.Minor;
                switch (major)
                {
                    case 5:
                        if (minor == 0)
                            return WINDOWS_NAME.WIN2000;
                        else if (minor == 1)
                            return WINDOWS_NAME.WINXP;
                        else
                            return WINDOWS_NAME.WINXP64_Bit;
                    case 6:
                        if (minor == 0)
                            return WINDOWS_NAME.WINVista;
                        else if (minor == 1)
                            return WINDOWS_NAME.WIN7;
                        else if (minor == 2)
                            return WINDOWS_NAME.WIN8;
                        else
                            return WINDOWS_NAME.WIN81;
                }
                return WINDOWS_NAME.WINXP;
            }
        }


       public static string GetGraphicCardFamilyName()
        {
            System.Management.ManagementObjectSearcher searcher
      = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");

            string graphicsCard = string.Empty;
            foreach (System.Management.ManagementObject mo in searcher.Get())
            {
                foreach (System.Management.PropertyData property in mo.Properties)
                {
                    if (property.Name == "Description")
                    {
                        graphicsCard = property.Value.ToString();
                    }
                }
            }
            return graphicsCard;
        }
    }
}
