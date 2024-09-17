using System;
using System.Collections.Concurrent;
using System.IO;
using System.Web;
using XmobiTea.ProtonNetCommon.Extensions;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers
{
    /// <summary>
    /// Delegate to handle the buffer file operations.
    /// </summary>
    /// <param name="key">The key associated with the file.</param>
    /// <param name="prefix">The prefix used in the file key.</param>
    /// <param name="filePath">The path of the file.</param>
    /// <param name="fileBuffer">The buffer containing file data.</param>
    /// <returns>The processed file buffer.</returns>
    public delegate byte[] GetBufferFileHandler(string key, string prefix, string filePath, byte[] fileBuffer);

    /// <summary>
    /// Interface for a cache that handles key-value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <typeparam name="TValue">The type of the cache value.</typeparam>
    interface ICache<TKey, TValue>
    {
        /// <summary>
        /// Adds an item to the cache.
        /// </summary>
        /// <param name="key">The key associated with the item.</param>
        /// <param name="value">The item to be added.</param>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">The key associated with the item to be removed.</param>
        void Remove(TKey key);

        /// <summary>
        /// Tries to get an item from the cache.
        /// </summary>
        /// <param name="key">The key associated with the item.</param>
        /// <param name="value">The retrieved item if found.</param>
        /// <returns>True if the item is found; otherwise, false.</returns>
        bool TryGet(TKey key, out TValue value);

    }

    /// <summary>
    /// Interface for a buffer cache that stores byte arrays.
    /// </summary>
    interface IBufferCache : ICache<string, byte[]> { }

    /// <summary>
    /// Implementation of a buffer cache that stores byte arrays using a thread-safe dictionary.
    /// </summary>
    class BufferCache : IBufferCache
    {
        /// <summary>
        /// The thread-safe dictionary used to store the buffer cache items.
        /// </summary>
        private ConcurrentDictionary<string, byte[]> bufferCache { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferCache"/> class.
        /// </summary>
        public BufferCache() => this.bufferCache = new ConcurrentDictionary<string, byte[]>();

        /// <summary>
        /// Adds or updates an item in the buffer cache.
        /// </summary>
        /// <param name="key">The key associated with the item.</param>
        /// <param name="value">The byte array to be added or updated.</param>
        public void Add(string key, byte[] value) => this.bufferCache[key] = value;

        /// <summary>
        /// Removes an item from the buffer cache.
        /// </summary>
        /// <param name="key">The key associated with the item to be removed.</param>
        public void Remove(string key) => this.bufferCache.TryRemove(key, out var _);

        /// <summary>
        /// Tries to get an item from the buffer cache.
        /// </summary>
        /// <param name="key">The key associated with the item.</param>
        /// <param name="value">The byte array retrieved from the cache if found.</param>
        /// <returns>True if the item is found; otherwise, false.</returns>
        public bool TryGet(string key, out byte[] value) => this.bufferCache.TryGetValue(key, out value);

    }

    /// <summary>
    /// Interface for a watch cache that stores instances of <see cref="AbstractWatch"/> objects.
    /// </summary>
    interface IWatchCache : ICache<string, AbstractWatch> { }

    /// <summary>
    /// Implementation of a watch cache that stores instances of <see cref="AbstractWatch"/> using a thread-safe dictionary.
    /// </summary>
    class WatchCache : IWatchCache
    {
        /// <summary>
        /// The thread-safe dictionary used to store the watch cache items.
        /// </summary>
        private ConcurrentDictionary<string, AbstractWatch> watchCache { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatchCache"/> class.
        /// </summary>
        public WatchCache() => this.watchCache = new ConcurrentDictionary<string, AbstractWatch>();

        /// <summary>
        /// Adds or updates an item in the watch cache.
        /// </summary>
        /// <param name="key">The key associated with the item.</param>
        /// <param name="watch">The <see cref="AbstractWatch"/> to be added or updated.</param>
        public void Add(string key, AbstractWatch watch) => this.watchCache[key] = watch;

        /// <summary>
        /// Removes an item from the watch cache.
        /// </summary>
        /// <param name="key">The key associated with the item to be removed.</param>
        public void Remove(string key)
        {
            if (this.watchCache.TryRemove(key, out var watch))
                watch.Dispose();
        }

        /// <summary>
        /// Tries to get an item from the watch cache.
        /// </summary>
        /// <param name="key">The key associated with the item.</param>
        /// <param name="value">The <see cref="AbstractWatch"/> retrieved from the cache if found.</param>
        /// <returns>True if the item is found; otherwise, false.</returns>
        public bool TryGet(string key, out AbstractWatch value) => this.watchCache.TryGetValue(key, out value);

    }

    /// <summary>
    /// Provides a base class for file system watching with support for resource disposal.
    /// </summary>
    abstract class AbstractWatch : IDisposable
    {
        /// <summary>
        /// Indicates whether the instance has been disposed.
        /// </summary>
        private bool disposed { get; set; }

        /// <summary>
        /// The <see cref="FileSystemWatcher"/> used to watch file system changes.
        /// </summary>
        protected FileSystemWatcher watcher { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractWatch"/> class and creates a new <see cref="FileSystemWatcher"/>.
        /// </summary>
        public AbstractWatch() => this.watcher = new FileSystemWatcher();

        /// <summary>
        /// Disposes of the resources used by the <see cref="AbstractWatch"/> class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="AbstractWatch"/> class and optionally releases managed resources.
        /// </summary>
        /// <param name="disposingManagedResources">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposingManagedResources)
        {
            if (!this.disposed)
            {
                if (disposingManagedResources)
                    this.watcher.Dispose();

                this.disposed = true;
            }
        }

    }

    /// <summary>
    /// Monitors changes to a specific file and updates the cache accordingly.
    /// </summary>
    class FileCacheWatch : AbstractWatch
    {
        /// <summary>
        /// The cache instance that stores file data.
        /// </summary>
        private StaticCache cache { get; }

        /// <summary>
        /// The prefix used for caching keys.
        /// </summary>
        private string prefix { get; }

        /// <summary>
        /// The path of the file being watched.
        /// </summary>
        private string filePath { get; }

        /// <summary>
        /// The handler used to process file buffer changes.
        /// </summary>
        private GetBufferFileHandler handler { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCacheWatch"/> class and starts monitoring the specified file.
        /// </summary>
        /// <param name="cache">The cache instance used to store file data.</param>
        /// <param name="prefix">The prefix used for caching keys.</param>
        /// <param name="filePath">The path of the file being watched.</param>
        /// <param name="handler">The handler used to process file buffer changes.</param>
        public FileCacheWatch(StaticCache cache, string prefix, string filePath, GetBufferFileHandler handler) : base()
        {
            this.cache = cache;
            this.prefix = prefix;
            this.filePath = filePath;
            this.handler = handler;

            this.StartWatch();
        }

        /// <summary>
        /// Configures the <see cref="FileSystemWatcher"/> to monitor changes to the specified file.
        /// </summary>
        void StartWatch()
        {
            this.watcher.Path = this.filePath;

            this.watcher.Changed += this.OnChanged;
            this.watcher.Deleted += this.OnDeleted;
            this.watcher.Renamed += this.OnRenamed;

            this.watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            this.watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Handles the event when the watched file is changed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            if (this.filePath == filePath)
                this.cache.AddFileInternal(this.prefix, this.filePath, false, this.handler);
        }

        /// <summary>
        /// Handles the event when the watched file is deleted.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            if (this.filePath == filePath)
                this.cache.RemoveFileInternal(this.prefix, this.filePath);
        }

        /// <summary>
        /// Handles the event when the watched file is renamed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            var oldFilePath = e.OldFullPath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);
            if (this.filePath == oldFilePath)
            {
                var filePath = e.FullPath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);
                this.cache.AddFileInternal(this.prefix, filePath, true, this.handler);

                this.cache.RemoveFileInternal(this.prefix, oldFilePath);
            }
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="FileCacheWatch"/> class.
        /// </summary>
        /// <param name="disposingManagedResources">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposingManagedResources)
        {
            this.watcher.Changed -= this.OnChanged;
            this.watcher.Deleted -= this.OnDeleted;
            this.watcher.Renamed -= this.OnRenamed;

            base.Dispose(disposingManagedResources);
        }

    }

    /// <summary>
    /// Monitors changes to files and folders within a specified directory and updates the cache accordingly.
    /// </summary>
    class FolderCacheWatch : AbstractWatch
    {
        /// <summary>
        /// The cache instance that stores file and folder data.
        /// </summary>
        private StaticCache cache { get; }

        /// <summary>
        /// The prefix used for caching keys.
        /// </summary>
        private string prefix { get; }

        /// <summary>
        /// The path of the directory being watched.
        /// </summary>
        private string path { get; }

        /// <summary>
        /// The filter used to specify which files to monitor.
        /// </summary>
        private string filter { get; }

        /// <summary>
        /// The handler used to process file buffer changes.
        /// </summary>
        private GetBufferFileHandler handler { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderCacheWatch"/> class and starts monitoring the specified directory.
        /// </summary>
        /// <param name="cache">The cache instance used to store file and folder data.</param>
        /// <param name="prefix">The prefix used for caching keys.</param>
        /// <param name="path">The path of the directory being watched.</param>
        /// <param name="filter">The filter used to specify which files to monitor.</param>
        /// <param name="handler">The handler used to process file buffer changes.</param>
        public FolderCacheWatch(StaticCache cache, string prefix, string path, string filter, GetBufferFileHandler handler) : base()
        {
            this.cache = cache;
            this.prefix = prefix;
            this.path = path;
            this.filter = filter;
            this.handler = handler;

            this.StartWatch();
        }

        /// <summary>
        /// Configures the <see cref="FileSystemWatcher"/> to monitor changes to the specified directory.
        /// </summary>
        void StartWatch()
        {
            this.watcher.Path = this.path;
            this.watcher.Filter = this.filter;

            this.watcher.Changed += this.OnChanged;
            this.watcher.Deleted += this.OnDeleted;
            this.watcher.Created += this.OnCreated;
            this.watcher.Renamed += this.OnRenamed;

            this.watcher.IncludeSubdirectories = true;
            this.watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;
            this.watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Handles the event when a file or directory in the watched folder is changed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            if (File.Exists(filePath) && !this.IsDirectory(filePath))
                this.cache.AddFileInFolderInternal(this.prefix, this.path, filePath, this.handler);
        }

        /// <summary>
        /// Handles the event when a file or directory in the watched folder is deleted.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            if (this.IsDirectory(filePath))
                this.cache.RemoveFolderInternal(this.prefix, filePath);
            else if (File.Exists(filePath))
                this.cache.RemoveFileInFolderInternal(this.prefix, this.path, filePath);
        }

        /// <summary>
        /// Handles the event when a file or directory in the watched folder is created.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            if (this.IsDirectory(filePath))
                this.cache.AddFolderInternal(this.prefix, filePath, this.filter, false, this.handler);
            else if (File.Exists(filePath))
                this.cache.AddFileInFolderInternal(this.prefix, this.path, filePath, this.handler);
        }

        /// <summary>
        /// Handles the event when a file or directory in the watched folder is renamed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            var filePath = e.FullPath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);
            var oldFilePath = e.OldFullPath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            if (this.IsDirectory(filePath))
            {
                this.cache.RemoveFolderInternal(this.prefix, oldFilePath);
                this.cache.AddFolderInternal(this.prefix, filePath, this.filter, false, this.handler);
            }
            else if (File.Exists(filePath))
            {
                this.cache.RemoveFileInFolderInternal(this.prefix, this.path, oldFilePath);
                this.cache.AddFileInFolderInternal(this.prefix, this.path, filePath, this.handler);
            }
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="FolderCacheWatch"/> class.
        /// </summary>
        /// <param name="disposingManagedResources">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposingManagedResources)
        {
            this.watcher.Changed -= this.OnChanged;
            this.watcher.Deleted -= this.OnDeleted;
            this.watcher.Created -= this.OnCreated;
            this.watcher.Renamed -= this.OnRenamed;

            base.Dispose(disposingManagedResources);
        }

        /// <summary>
        /// Determines whether the specified path is a directory.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>True if the path is a directory; otherwise, false.</returns>
        private bool IsDirectory(string path) => !File.Exists(path) && Directory.Exists(path);

    }

    /// <summary>
    /// Defines the contract for a static cache that stores files and folders and provides methods to manage and access them.
    /// </summary>
    public interface IStaticCache
    {
        /// <summary>
        /// Adds a file to the cache with the specified prefix and file path.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="filePath">The path of the file to add.</param>
        /// <param name="watchAndAutoUpdate">Indicates whether to monitor the file for changes and automatically update the cache.</param>
        /// <param name="handler">Optional handler for processing file buffer changes.</param>
        /// <returns>True if the file was added successfully; otherwise, false.</returns>
        bool AddFile(string prefix, string filePath, bool watchAndAutoUpdate, GetBufferFileHandler handler = null);

        /// <summary>
        /// Removes a file from the cache with the specified prefix and file path.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="filePath">The path of the file to remove.</param>
        /// <returns>True if the file was removed successfully; otherwise, false.</returns>
        bool RemoveFile(string prefix, string filePath);

        /// <summary>
        /// Adds a folder to the cache with the specified prefix, path, and filter.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="path">The path of the folder to add.</param>
        /// <param name="filter">The filter used to specify which files to monitor within the folder.</param>
        /// <param name="watchAndAutoUpdate">Indicates whether to monitor the folder for changes and automatically update the cache.</param>
        /// <param name="handler">Optional handler for processing file buffer changes.</param>
        /// <returns>True if the folder was added successfully; otherwise, false.</returns>
        bool AddFolder(string prefix, string path, string filter, bool watchAndAutoUpdate, GetBufferFileHandler handler = null);

        /// <summary>
        /// Removes a folder from the cache with the specified prefix and path.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="path">The path of the folder to remove.</param>
        /// <returns>True if the folder was removed successfully; otherwise, false.</returns>
        bool RemoveFolder(string prefix, string path);

        /// <summary>
        /// Tries to retrieve the buffer for the specified query path from the cache.
        /// </summary>
        /// <param name="queryPath">The path to query in the cache.</param>
        /// <param name="buffer">The buffer containing the file data if the query was successful; otherwise, null.</param>
        /// <returns>True if the buffer was found; otherwise, false.</returns>
        bool TryGet(string queryPath, out byte[] buffer);

    }

    /// <summary>
    /// Implements a static cache for managing files and folders with optional watching and auto-updating capabilities.
    /// </summary>
    class StaticCache : IStaticCache
    {
        private IBufferCache bufferCache { get; }
        private IWatchCache watchCache { get; }

        public StaticCache()
        {
            this.bufferCache = new BufferCache();
            this.watchCache = new WatchCache();
        }

        private byte[] DefaultGetBufferFileHandler(string key, string prefix, string filePath, byte[] fileBuffer) => fileBuffer;

        /// <summary>
        /// Adds a file to the cache and optionally sets up a watcher for automatic updates.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="filePath">The path of the file to add.</param>
        /// <param name="watchAndAutoUpdate">Indicates whether to monitor the file for changes and automatically update the cache.</param>
        /// <param name="handler">Optional handler for processing file buffer changes.</param>
        /// <returns>True if the file was added successfully; otherwise, false.</returns>
        internal bool AddFileInternal(string prefix, string filePath, bool watchAndAutoUpdate, GetBufferFileHandler handler)
        {
            if (!File.Exists(filePath)) return false;

            var key = prefix + "/" + HttpUtility.UrlDecode(Path.GetFileName(filePath));
            key = key.Replace("//", "/");

            var finalBuffer = handler?.Invoke(key, prefix, filePath, File.ReadAllBytes(filePath));

            if (finalBuffer == null) return false;

            this.bufferCache.Add(key, finalBuffer);

            if (watchAndAutoUpdate) this.watchCache.Add(key, new FileCacheWatch(this, prefix, filePath, handler));

            return true;
        }

        /// <summary>
        /// Removes a file from the cache and stops watching it if necessary.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="filePath">The path of the file to remove.</param>
        /// <returns>True if the file was removed successfully; otherwise, false.</returns>
        internal bool RemoveFileInternal(string prefix, string filePath)
        {
            var key = prefix + "/" + HttpUtility.UrlDecode(Path.GetFileName(filePath));
            key = key.Replace("//", "/");

            this.bufferCache.Remove(key);
            this.watchCache.Remove(key);

            return true;
        }

        /// <summary>
        /// Adds a file within a folder to the cache.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="path">The path of the folder containing the file.</param>
        /// <param name="filePath">The path of the file to add.</param>
        /// <param name="handler">Optional handler for processing file buffer changes.</param>
        /// <returns>True if the file was added successfully; otherwise, false.</returns>
        internal bool AddFileInFolderInternal(string prefix, string path, string filePath, GetBufferFileHandler handler)
        {
            if (!File.Exists(filePath)) return false;

            var key = prefix + "/" + HttpUtility.UrlDecode(Path.GetFileName(filePath).Replace(path, string.Empty));
            key = key.Replace("//", "/");

            var finalBuffer = handler?.Invoke(key, prefix, filePath, File.ReadAllBytes(filePath));

            if (finalBuffer == null) return false;

            this.bufferCache.Add(key, finalBuffer);

            return true;
        }

        /// <summary>
        /// Removes a file within a folder from the cache.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="path">The path of the folder containing the file.</param>
        /// <param name="filePath">The path of the file to remove.</param>
        /// <returns>True if the file was removed successfully; otherwise, false.</returns>
        internal bool RemoveFileInFolderInternal(string prefix, string path, string filePath)
        {
            var key = prefix + "/" + HttpUtility.UrlDecode(Path.GetFileName(filePath).Replace(path, string.Empty));
            key = key.Replace("//", "/");

            this.bufferCache.Remove(key);

            return true;
        }

        /// <summary>
        /// Adds a folder to the cache, including its files and subdirectories, and optionally sets up a watcher for automatic updates.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="path">The path of the folder to add.</param>
        /// <param name="filter">The filter used to specify which files to monitor within the folder.</param>
        /// <param name="watchAndAutoUpdate">Indicates whether to monitor the folder for changes and automatically update the cache.</param>
        /// <param name="handler">Optional handler for processing file buffer changes.</param>
        /// <returns>True if the folder was added successfully; otherwise, false.</returns>
        internal bool AddFolderInternal(string prefix, string path, string filter, bool watchAndAutoUpdate, GetBufferFileHandler handler)
        {
            if (!Directory.Exists(path)) return false;

            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                var subPath = directory.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);
                this.AddFolderInternal(prefix, subPath, filter, false, handler);
            }

            var files = Directory.GetFiles(path, filter);
            foreach (var file in files)
            {
                var filePath = file.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);
                this.AddFileInFolderInternal(prefix, path, filePath, handler);
            }

            if (watchAndAutoUpdate)
            {
                var key = prefix + "/" + HttpUtility.UrlDecode(Path.GetFileName(path));
                key = key.Replace("//", "/");

                this.watchCache.Add(key, new FolderCacheWatch(this, prefix, path, filter, handler));
            }

            return true;
        }

        /// <summary>
        /// Removes a folder from the cache, including its files and subdirectories.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="path">The path of the folder to remove.</param>
        /// <returns>True if the folder was removed successfully; otherwise, false.</returns>
        internal bool RemoveFolderInternal(string prefix, string path)
        {
            if (!Directory.Exists(path)) return false;

            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                var subPath = directory.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);
                this.RemoveFolderInternal(prefix, subPath);
            }

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var filePath = file.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);
                this.RemoveFileInFolderInternal(prefix, path, filePath);
            }

            var key = prefix + "/" + HttpUtility.UrlDecode(Path.GetFileName(path));
            key = key.Replace("//", "/");

            this.watchCache.Remove(key);

            return true;
        }

        /// <summary>
        /// Adds a file to the cache with optional watching and auto-updating capabilities.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="filePath">The path of the file to add.</param>
        /// <param name="watchAndAutoUpdate">Indicates whether to monitor the file for changes and automatically update the cache.</param>
        /// <param name="handler">Optional handler for processing file buffer changes.</param>
        /// <returns>True if the file was added successfully; otherwise, false.</returns>
        public bool AddFile(string prefix, string filePath, bool watchAndAutoUpdate, GetBufferFileHandler handler = null)
        {
            handler = handler ?? this.DefaultGetBufferFileHandler;

            filePath = filePath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            return this.AddFileInternal(prefix, filePath, watchAndAutoUpdate, handler);
        }

        /// <summary>
        /// Removes a file from the cache.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="filePath">The path of the file to remove.</param>
        /// <returns>True if the file was removed successfully; otherwise, false.</returns>
        public bool RemoveFile(string prefix, string filePath) => this.RemoveFileInternal(prefix, filePath);

        /// <summary>
        /// Adds a folder to the cache with optional watching and auto-updating capabilities.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="path">The path of the folder to add.</param>
        /// <param name="filter">The filter used to specify which files to monitor within the folder.</param>
        /// <param name="watchAndAutoUpdate">Indicates whether to monitor the folder for changes and automatically update the cache.</param>
        /// <param name="handler">Optional handler for processing file buffer changes.</param>
        /// <returns>True if the folder was added successfully; otherwise, false.</returns>
        public bool AddFolder(string prefix, string path, string filter, bool watchAndAutoUpdate, GetBufferFileHandler handler = null)
        {
            handler = handler ?? this.DefaultGetBufferFileHandler;

            path = path.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            return this.AddFolderInternal(prefix, path, filter, watchAndAutoUpdate, handler);
        }

        /// <summary>
        /// Removes a folder from the cache.
        /// </summary>
        /// <param name="prefix">The prefix used for the cache key.</param>
        /// <param name="path">The path of the folder to remove.</param>
        /// <returns>True if the folder was removed successfully; otherwise, false.</returns>
        public bool RemoveFolder(string prefix, string path) => this.RemoveFolderInternal(prefix, path);

        /// <summary>
        /// Tries to retrieve the buffer for the specified query path from the cache.
        /// </summary>
        /// <param name="queryPath">The path to query in the cache.</param>
        /// <param name="buffer">The buffer containing the file data if the query was successful; otherwise, null.</param>
        /// <returns>True if the buffer was found; otherwise, false.</returns>
        public bool TryGet(string queryPath, out byte[] buffer) => this.bufferCache.TryGet(queryPath, out buffer);

    }

}
