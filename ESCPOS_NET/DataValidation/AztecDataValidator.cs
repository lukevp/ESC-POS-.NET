using ESCPOS_NET.Emitters;
using System;
using System.Collections.Generic;

namespace ESCPOS_NET.DataValidation
{
    public static class AztecDataValidator
    {
        private static Dictionary<ModeTypeAztecCode, AztecDataConstraint> _constraints = new Dictionary<ModeTypeAztecCode, AztecDataConstraint>
        {
            { ModeTypeAztecCode.FULL_RANGE, new AztecDataConstraint { MinLayers = 4, MaxLayers = 32, MinModuleSize = 2, MaxModuleSize = 16, MinCorrectionLevel = 5, MaxCorrectionLevel = 95 } },
            { ModeTypeAztecCode.COMPACT, new AztecDataConstraint { MinLayers = 1, MaxLayers = 4, MinModuleSize = 2, MaxModuleSize = 16, MinCorrectionLevel = 5, MaxCorrectionLevel = 95 } }
        };

        public static void ValidateAztecCode(ModeTypeAztecCode type, byte[] data, int moduleSize, int correctionLevel, int numberOfDataLayers)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            // Validate constraints on aztec code.
            _constraints.TryGetValue(type, out var constraints);
            if (constraints is null)
            {
                return;
            }

            // Check data layers. 0 = automatic processing.
            if (numberOfDataLayers != 0)
            {
                if (numberOfDataLayers < constraints.MinLayers)
                {
                    throw new ArgumentException($"Number of data layers '{numberOfDataLayers}' is lower than the minimum {constraints.MinLayers} for {type}.");
                }
                else if (constraints.MaxLayers < numberOfDataLayers)
                {
                    throw new ArgumentException($"Number of data layers '{numberOfDataLayers}' is higher than the maximum {constraints.MaxLayers} for {type}.");
                }
            }

            // Check module size
            if (moduleSize < constraints.MinModuleSize)
            {
                throw new ArgumentException($"Module size '{moduleSize}' is lower than the minimum {constraints.MinModuleSize} for {type}.");
            }
            else if (constraints.MaxModuleSize < moduleSize)
            {
                throw new ArgumentException($"Module size '{moduleSize}' is higher than the minimum {constraints.MaxModuleSize} for {type}.");
            }

            // Check correction level
            if (correctionLevel < constraints.MinCorrectionLevel)
            {
                throw new ArgumentException($"Correction level '{correctionLevel}' is lower than the minimum {constraints.MinCorrectionLevel} for {type}.");
            }
            else if (constraints.MaxCorrectionLevel < correctionLevel)
            {
                throw new ArgumentException($"Correction level '{correctionLevel}' is higher than the minimum {constraints.MaxCorrectionLevel} for {type}.");
            }
        }
    }
}
