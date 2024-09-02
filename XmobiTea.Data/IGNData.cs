namespace XmobiTea.Data
{
    /// <summary>
    /// Defines a contract for classes that can convert their data into a specific format.
    /// </summary>
    public interface IGNData
    {
        /// <summary>
        /// Converts the implementing object to a data representation.
        /// </summary>
        /// <returns>
        /// An object representing the data of the implementing class. 
        /// The exact type of this object will depend on the implementation.
        /// </returns>
        object ToData();

    }

}
