namespace ESCPOS_NET
{
    public interface IPrinter
    {

        void Write(byte[] bytes);
    }
}