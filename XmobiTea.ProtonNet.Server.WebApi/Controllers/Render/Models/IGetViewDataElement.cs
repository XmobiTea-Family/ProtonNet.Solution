namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for an element that retrieves view data by name.
    /// </summary>
    public interface IGetViewDataElement
    {
        /// <summary>
        /// Gets the name of the view data element.
        /// </summary>
        string Name { get; }

    }

    /// <summary>
    /// Represents an implementation of <see cref="IGetViewDataElement"/>, 
    /// used to retrieve a specific view data element by its name.
    /// </summary>
    public class GetViewDataElement : IGetViewDataElement
    {
        /// <summary>
        /// Gets the name of the view data element.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetViewDataElement"/> class 
        /// with the specified name.
        /// </summary>
        /// <param name="name">The name of the view data element to retrieve.</param>
        public GetViewDataElement(string name) => this.Name = name;

    }

}
