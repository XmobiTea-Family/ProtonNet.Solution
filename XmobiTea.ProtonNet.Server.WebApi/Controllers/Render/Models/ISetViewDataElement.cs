namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for an element that sets view data, 
    /// including its name and value.
    /// </summary>
    interface ISetViewDataElement
    {
        /// <summary>
        /// Gets the name of the view data element.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the value of the view data element.
        /// </summary>
        object Value { get; }

    }

    /// <summary>
    /// Represents the implementation of <see cref="ISetViewDataElement"/>, 
    /// holding the name and value of the view data element.
    /// </summary>
    class SetViewDataElement : ISetViewDataElement
    {
        /// <summary>
        /// Gets the name of the view data element.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value of the view data element.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetViewDataElement"/> class 
        /// with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the view data element.</param>
        /// <param name="value">The value of the view data element.</param>
        public SetViewDataElement(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

    }

}
