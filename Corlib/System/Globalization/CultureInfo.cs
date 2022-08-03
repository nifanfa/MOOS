namespace System.Globalization
{

    /// <summary>
    /// Provides information about a specific culture (called a locale for unmanaged code development).
    /// The information includes the names for the culture, the writing system, the calendar used, and formatting for dates and sort strings.
    /// </summary>
    [Serializable]
    public class CultureInfo : ICloneable, IFormatProvider
    {
        // TODO
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public object GetFormat(Type formatType)
        {
            throw new NotImplementedException();
        }
    }

}