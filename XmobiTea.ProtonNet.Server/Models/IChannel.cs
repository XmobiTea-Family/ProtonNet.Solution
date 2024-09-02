using XmobiTea.Collections.Generic;
using XmobiTea.ProtonNet.Server.Services;

namespace XmobiTea.ProtonNet.Server.Models
{
    /// <summary>
    /// Defines methods for managing channel membership and operations.
    /// </summary>
    interface IChannelMediator
    {
        /// <summary>
        /// Handles a user joining the channel.
        /// </summary>
        /// <param name="userId">The ID of the user joining the channel.</param>
        /// <returns>True if the user successfully joined; otherwise, false.</returns>
        bool OnJoin(string userId);

        /// <summary>
        /// Handles a user leaving the channel.
        /// </summary>
        /// <param name="userId">The ID of the user leaving the channel.</param>
        /// <returns>True if the user successfully left; otherwise, false.</returns>
        bool OnLeave(string userId);

        /// <summary>
        /// Clears all users from the channel.
        /// </summary>
        void OnClear();

    }

    /// <summary>
    /// Defines methods for channel operations, including user management and properties.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// Gets the unique identifier of the channel.
        /// </summary>
        /// <returns>The channel ID.</returns>
        string GetId();

        /// <summary>
        /// Gets the number of users in the channel.
        /// </summary>
        /// <returns>The number of users.</returns>
        int Count();

        /// <summary>
        /// Adds a user to the channel.
        /// </summary>
        /// <param name="userId">The ID of the user to add.</param>
        /// <returns>True if the user was successfully added; otherwise, false.</returns>
        bool Join(string userId);

        /// <summary>
        /// Removes a user from the channel.
        /// </summary>
        /// <param name="userId">The ID of the user to remove.</param>
        /// <returns>True if the user was successfully removed; otherwise, false.</returns>
        bool Leave(string userId);

        /// <summary>
        /// Checks if the user is in the channel.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <returns>True if the user is in the channel; otherwise, false.</returns>
        bool Contains(string userId);

        /// <summary>
        /// Gets the IDs of all users in the channel.
        /// </summary>
        /// <returns>An enumerable collection of user IDs.</returns>
        System.Collections.Generic.IEnumerable<string> GetUserIds();

        /// <summary>
        /// Removes all users from the channel.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the properties associated with the channel.
        /// </summary>
        /// <returns>An <see cref="IProperty"/> representing the channel's properties.</returns>
        IProperty GetProperty();

    }

    /// <summary>
    /// Implements the channel interface and provides methods for managing users and channel properties.
    /// </summary>
    public class Channel : IChannel, IChannelMediator
    {
        private IChannelService channelService { get; }

        /// <summary>
        /// List of users currently in the channel.
        /// </summary>
        protected System.Collections.Generic.IList<string> userList { get; }

        /// <summary>
        /// Properties associated with the channel.
        /// </summary>
        protected IProperty property { get; }

        private string id { get; }

        /// <summary>
        /// Gets the unique identifier of the channel.
        /// </summary>
        /// <returns>The channel ID.</returns>
        public string GetId() => this.id;

        /// <summary>
        /// Gets the IDs of all users in the channel.
        /// </summary>
        /// <returns>An enumerable collection of user IDs.</returns>
        public System.Collections.Generic.IEnumerable<string> GetUserIds() => this.userList;

        /// <summary>
        /// Adds a user to the channel.
        /// </summary>
        /// <param name="userId">The ID of the user to add.</param>
        /// <returns>True if the user was successfully added; otherwise, false.</returns>
        public bool Join(string userId) => this.channelService.JoinChannel(userId, this.id);

        /// <summary>
        /// Handles a user joining the channel.
        /// </summary>
        /// <param name="userId">The ID of the user joining the channel.</param>
        /// <returns>True if the user successfully joined; otherwise, false.</returns>
        public bool OnJoin(string userId)
        {
            if (!this.userList.Contains(userId))
            {
                this.userList.Add(userId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a user from the channel.
        /// </summary>
        /// <param name="userId">The ID of the user to remove.</param>
        /// <returns>True if the user was successfully removed; otherwise, false.</returns>
        public bool Leave(string userId) => this.channelService.LeaveChannel(userId, this.id);

        /// <summary>
        /// Handles a user leaving the channel.
        /// </summary>
        /// <param name="userId">The ID of the user leaving the channel.</param>
        /// <returns>True if the user successfully left; otherwise, false.</returns>
        public bool OnLeave(string userId) => this.userList.Remove(userId);

        /// <summary>
        /// Gets the number of users in the channel.
        /// </summary>
        /// <returns>The number of users.</returns>
        public int Count() => this.userList.Count;

        /// <summary>
        /// Removes all users from the channel.
        /// </summary>
        public void Clear() => this.channelService.ClearChannel(this.id);

        /// <summary>
        /// Clears the list of users in the channel.
        /// </summary>
        public void OnClear() => this.userList.Clear();

        /// <summary>
        /// Gets the properties associated with the channel.
        /// </summary>
        /// <returns>An <see cref="IProperty"/> representing the channel's properties.</returns>
        public IProperty GetProperty() => this.property;

        /// <summary>
        /// Checks if the user is in the channel.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <returns>True if the user is in the channel; otherwise, false.</returns>
        public bool Contains(string userId) => this.userList.Contains(userId);

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the channel.</param>
        /// <param name="channelService">The service used to manage channel operations.</param>
        public Channel(string id, IChannelService channelService)
        {
            this.channelService = channelService;

            this.id = id;
            this.userList = new ThreadSafeList<string>();
            this.property = new Property();
        }

    }

}
