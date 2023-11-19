namespace ESCPOS_NET.DataValidation
{
    public class AztecDataConstraint
    {
        public int MinLayers { get; set; }

        public int MaxLayers { get; set; }

        public int MinModuleSize { get; set; }

        public int MaxModuleSize { get; set; }

        public int MinCorrectionLevel { get; set; }

        public int MaxCorrectionLevel { get; set; }

        public int MinDataSize { get; set; }

        public int MaxDataSize { get; set; }
    }
}
