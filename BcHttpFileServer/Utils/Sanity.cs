namespace BcHttpFileServer.Utils
{
    public static class Sanity
    {
        public static O? SafeCast<I, O>(this I value, Func<I,O> conversion)
        {
            try
            {
                return conversion(value);
            }
            catch(Exception ex)
            {
                return default;
            }
        }
    }
}
