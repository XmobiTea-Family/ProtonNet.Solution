using XmobiTea.Collections.Generic;
using XmobiTea.ProtonNet.Server.Models;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Provides methods for managing channels and user memberships in channels.
    /// </summary>
    public interface IChannelService
    {
        /// <summary>
        /// Gets the user IDs in a specific channel.
        /// </summary>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns>A collection of user IDs in the specified channel.</returns>
        System.Collections.Generic.IEnumerable<string> GetUserIdsInChannel(string channelId);

        /// <summary>
        /// Checks if a user is in a specific channel.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns><c>true</c> if the user is in the channel; otherwise, <c>false</c>.</returns>
        bool InChannel(string userId, string channelId);

        /// <summary>
        /// Adds a user to a specific channel.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns><c>true</c> if the user joined the channel successfully; otherwise, <c>false</c>.</returns>
        bool JoinChannel(string userId, string channelId);

        /// <summary>
        /// Removes a user from a specific channel.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns><c>true</c> if the user left the channel successfully; otherwise, <c>false</c>.</returns>
        bool LeaveChannel(string userId, string channelId);

        /// <summary>
        /// Removes a user from all channels.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        void LeaveAllChannel(string userId);

        /// <summary>
        /// Clears all users from a specific channel.
        /// </summary>
        /// <param name="channelId">The ID of the channel.</param>
        void ClearChannel(string channelId);

        /// <summary>
        /// Gets all channels a user is a member of.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A collection of channel IDs that the user is a member of.</returns>
        System.Collections.Generic.IEnumerable<string> GetChannels(string userId);

    }

    /// <summary>
    /// Implements <see cref="IChannelService"/> to manage channels and user memberships.
    /// </summary>
    public class ChannelService : IChannelService
    {
        /// <summary>
        /// Gets a dictionary that maps channel IDs to their corresponding <see cref="IChannel"/> instances.
        /// </summary>
        protected System.Collections.Generic.IDictionary<string, IChannel> channelIdWithChannelDict { get; }

        /// <summary>
        /// Gets a dictionary that maps user IDs to lists of <see cref="IChannel"/> instances they are associated with.
        /// </summary>
        protected System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<IChannel>> userIdWithChannelsDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelService"/> class.
        /// </summary>
        public ChannelService()
        {
            this.channelIdWithChannelDict = new ThreadSafeDictionary<string, IChannel>();
            this.userIdWithChannelsDict = new ThreadSafeDictionary<string, System.Collections.Generic.IList<IChannel>>();
        }

        /// <summary>
        /// Gets the user IDs in a specific channel.
        /// </summary>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns>A collection of user IDs in the specified channel.</returns>
        public System.Collections.Generic.IEnumerable<string> GetUserIdsInChannel(string channelId)
        {
            var channel = this.GetChannel(channelId, false);
            if (channel == null) return new string[0];
            return channel.GetUserIds();
        }

        private IChannel GetChannel(string channelId, bool createIfNotExists = true)
        {
            if (!this.channelIdWithChannelDict.TryGetValue(channelId, out var channel))
            {
                if (createIfNotExists)
                {
                    var newChannel = new Channel(channelId, this);
                    this.channelIdWithChannelDict[channelId] = newChannel;
                    channel = newChannel;
                }
            }

            return channel;
        }

        /// <summary>
        /// Checks if a user is in a specific channel.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns><c>true</c> if the user is in the channel; otherwise, <c>false</c>.</returns>
        public bool InChannel(string userId, string channelId)
        {
            var channel = this.GetChannel(channelId);
            return channel != null && channel.Contains(userId);
        }

        /// <summary>
        /// Adds a user to a specific channel.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns><c>true</c> if the user joined the channel successfully; otherwise, <c>false</c>.</returns>
        public bool JoinChannel(string userId, string channelId)
        {
            var channel = this.GetChannel(channelId);

            if (((IChannelMediator)channel).OnJoin(userId))
            {
                if (!this.userIdWithChannelsDict.TryGetValue(userId, out var channels))
                {
                    channels = new ThreadSafeList<IChannel>();
                    this.userIdWithChannelsDict[userId] = channels;
                }

                if (!channels.Contains(channel)) channels.Add(channel);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a user from a specific channel.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns><c>true</c> if the user left the channel successfully; otherwise, <c>false</c>.</returns>
        public bool LeaveChannel(string userId, string channelId)
        {
            var channel = this.GetChannel(channelId);

            if (channel != null)
            {
                ((IChannelMediator)channel).OnLeave(userId);

                if (this.userIdWithChannelsDict.TryGetValue(userId, out var channels))
                {
                    channels.Remove(channel);

                    if (channels.Count == 0) this.userIdWithChannelsDict.Remove(userId);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a user from all channels.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        public void LeaveAllChannel(string userId)
        {
            if (this.userIdWithChannelsDict.TryGetValue(userId, out var channels))
            {
                foreach (var channel in channels)
                {
                    channel.Leave(userId);
                }

                this.userIdWithChannelsDict.Remove(userId);
            }
        }

        /// <summary>
        /// Clears all users from a specific channel.
        /// </summary>
        /// <param name="channelId">The ID of the channel.</param>
        public void ClearChannel(string channelId)
        {
            var channel = this.GetChannel(channelId);

            if (channel != null)
            {
                var userIds = channel.GetUserIds();

                foreach (var userId in userIds)
                {
                    if (this.userIdWithChannelsDict.TryGetValue(userId, out var channels))
                    {
                        channels.Remove(channel);

                        if (channels.Count == 0) this.userIdWithChannelsDict.Remove(userId);
                    }
                }

                ((IChannelMediator)channel).OnClear();
            }
        }

        /// <summary>
        /// Gets all channels a user is a member of.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A collection of channel IDs that the user is a member of.</returns>
        public System.Collections.Generic.IEnumerable<string> GetChannels(string userId)
        {
            if (this.userIdWithChannelsDict.TryGetValue(userId, out var channels))
            {
                var answer = new string[channels.Count];

                for (var i = 0; i < channels.Count; i++)
                {
                    answer[i] = channels[i].GetId();
                }

                return answer;
            }

            return new string[0];
        }

    }

}
