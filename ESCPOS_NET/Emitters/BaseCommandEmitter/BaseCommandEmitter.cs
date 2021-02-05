namespace ESCPOS_NET.Emitters
{
    public abstract partial class BaseCommandEmitter<TCommandValues> : ICommandEmitter where TCommandValues : ICommandValues, new()
    {
        TCommandValues Values = new TCommandValues();
    }
}
