using System;
using System.Collections.Generic;
using System.Text;

#if VTC_PLUGIN
using uTank2.LootPlugins;
#endif

namespace VTClassic
{
    internal enum eUTLFileFeature
    {
        RuleExpression = 1,
        RequirementLengthCode = 2,
    }

    internal static class UTLVersionInfo
    {
        public const int MAX_PROFILE_VERSION = 1;

        public static bool VersionHasFeature(eUTLFileFeature feature, int version)
        {
            switch (version)
            {
                case 0:
                    return false;
                case 1:
                    if (feature == eUTLFileFeature.RequirementLengthCode) return true;
                    if (feature == eUTLFileFeature.RuleExpression) return true;
                    return false;
                default:
                    return false;
            }
        }
    }
}