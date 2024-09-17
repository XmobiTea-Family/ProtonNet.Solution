namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for an initialization element that contains
    /// a collection of set view data elements.
    /// </summary>
    interface IPinitElement
    {
        /// <summary>
        /// Gets an array of elements that set view data during initialization.
        /// </summary>
        ISetViewDataElement[] SetViewDataElements { get; }

    }

    /// <summary>
    /// Represents the implementation of <see cref="IPinitElement"/>, holding
    /// an array of elements used to set view data during initialization.
    /// </summary>
    class PinitElement : IPinitElement
    {
        /// <summary>
        /// Gets the array of elements that set view data during initialization.
        /// </summary>
        public ISetViewDataElement[] SetViewDataElements { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinitElement"/> class with the specified
        /// set view data elements.
        /// </summary>
        /// <param name="setViewDataElements">The array of elements that set view data.</param>
        public PinitElement(ISetViewDataElement[] setViewDataElements) => this.SetViewDataElements = setViewDataElements;

    }

}
