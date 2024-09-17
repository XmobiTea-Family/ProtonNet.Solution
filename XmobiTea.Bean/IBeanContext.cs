using System.Reflection;
using XmobiTea.Bean.Attributes;
using XmobiTea.Bean.Factory;
using XmobiTea.Bean.Support;
using XmobiTea.Linq;

namespace XmobiTea.Bean
{
    /// <summary>
    /// Defines methods for managing singleton instances in the context.
    /// </summary>
    public interface ISingletonContext
    {
        /// <summary>Gets a singleton instance of the specified type.</summary>
        /// <typeparam name="T">The type of the singleton.</typeparam>
        /// <returns>The singleton instance.</returns>
        T GetSingleton<T>();

        /// <summary>Gets a singleton instance of the specified type.</summary>
        /// <param name="type">The type of the singleton.</param>
        /// <returns>The singleton instance.</returns>
        object GetSingleton(System.Type type);

        /// <summary>Creates a singleton instance of the specified type.</summary>
        /// <typeparam name="T">The type of the singleton.</typeparam>
        /// <param name="args">The arguments for the constructor.</param>
        /// <returns>The created singleton instance.</returns>
        T CreateSingleton<T>(params object[] args);

        /// <summary>Creates a singleton instance of the specified type.</summary>
        /// <param name="type">The type of the singleton.</param>
        /// <param name="args">The arguments for the constructor.</param>
        /// <returns>The created singleton instance.</returns>
        object CreateSingleton(System.Type type, params object[] args);

        /// <summary>Sets a singleton instance for the specified type.</summary>
        /// <param name="singletonObj">The singleton instance.</param>
        /// <param name="type">The type to associate with the singleton.</param>
        void SetSingleton(object singletonObj, System.Type type);

        /// <summary>Sets a singleton instance for the specified type.</summary>
        /// <typeparam name="T">The type to associate with the singleton.</typeparam>
        /// <param name="singletonObj">The singleton instance.</param>
        void SetSingleton<T>(T singletonObj);

        /// <summary>Gets all singleton instances in the context.</summary>
        /// <returns>An enumeration of all singleton instances.</returns>
        System.Collections.Generic.IEnumerable<object> GetSingletons();
    }

    /// <summary>
    /// Defines methods for automatically binding dependencies in singleton instances.
    /// </summary>
    public interface IAutoBindingContext
    {
        /// <summary>Automatically binds dependencies to the specified singleton object.</summary>
        /// <param name="singletonObj">The singleton object to bind.</param>
        void AutoBind(object singletonObj);
    }

    /// <summary>
    /// Defines methods for scanning and retrieving types within a given namespace or assembly.
    /// </summary>
    public interface IClassScannerContext
    {
        /// <summary>Scans and retrieves types within the specified namespace.</summary>
        /// <param name="prefixNamespace">The namespace to scan.</param>
        /// <returns>An enumeration of types within the specified namespace.</returns>
        System.Collections.Generic.IEnumerable<System.Type> ScanClass(string prefixNamespace);

        /// <summary>Scans and retrieves types within the specified namespace and assemblies.</summary>
        /// <param name="prefixNamespace">The namespace to scan.</param>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns>An enumeration of types within the specified namespace and assemblies.</returns>
        System.Collections.Generic.IEnumerable<System.Type> ScanClass(string prefixNamespace, System.Collections.Generic.IEnumerable<Assembly> assemblies);

        /// <summary>Scans and retrieves types that have the specified custom attribute.</summary>
        /// <param name="type">The custom attribute type to scan for.</param>
        /// <param name="inherit">Whether to scan for inherited attributes.</param>
        /// <returns>An enumeration of types that have the specified custom attribute.</returns>
        System.Collections.Generic.IEnumerable<System.Type> ScanClassHasCustomAttribute(System.Type type, bool inherit);

        /// <summary>Scans and retrieves types that have the specified custom attribute within the specified assemblies.</summary>
        /// <param name="type">The custom attribute type to scan for.</param>
        /// <param name="inherit">Whether to scan for inherited attributes.</param>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns>An enumeration of types that have the specified custom attribute within the specified assemblies.</returns>
        System.Collections.Generic.IEnumerable<System.Type> ScanClassHasCustomAttribute(System.Type type, bool inherit, System.Collections.Generic.IEnumerable<Assembly> assemblies);

        /// <summary>Scans and retrieves types that are assignable to the specified type.</summary>
        /// <param name="type">The type to scan for assignable types.</param>
        /// <returns>An enumeration of types that are assignable to the specified type.</returns>
        System.Collections.Generic.IEnumerable<System.Type> ScanClassFromAssignable(System.Type type);

        /// <summary>Scans and retrieves types that are assignable to the specified type within the specified assemblies.</summary>
        /// <param name="type">The type to scan for assignable types.</param>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns>An enumeration of types that are assignable to the specified type within the specified assemblies.</returns>
        System.Collections.Generic.IEnumerable<System.Type> ScanClassFromAssignable(System.Type type, System.Collections.Generic.IEnumerable<Assembly> assemblies);
    }

    /// <summary>
    /// Combines singleton management, auto-binding, and class scanning functionalities.
    /// </summary>
    public interface IBeanContext : ISingletonContext, IAutoBindingContext, IClassScannerContext
    {
    }

    /// <summary>
    /// Implements the IBeanContext interface to manage singleton instances,
    /// handle auto-binding of dependencies, and scan classes in assemblies.
    /// </summary>
    public class BeanContext : IBeanContext
    {
        /// <summary>Gets the list of singleton instances managed by the context.</summary>
        private System.Collections.Generic.IList<object> singletonObjLst { get; }

        /// <summary>Gets the singleton factory responsible for creating and managing singleton instances.</summary>
        private ISingletonFactory singletonFactory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeanContext"/> class.
        /// </summary>
        public BeanContext()
        {
            this.singletonObjLst = new System.Collections.Generic.List<object>();
            this.singletonFactory = new SingletonFactory();
        }

        /// <summary>
        /// Creates a singleton instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the singleton to create.</typeparam>
        /// <param name="args">The arguments for the constructor.</param>
        /// <returns>The created singleton instance.</returns>
        public T CreateSingleton<T>(params object[] args) => (T)this.CreateSingleton(typeof(T), args);

        /// <summary>
        /// Creates a singleton instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the singleton to create.</param>
        /// <param name="args">The arguments for the constructor.</param>
        /// <returns>The created singleton instance.</returns>
        public object CreateSingleton(System.Type type, params object[] args)
        {
            var answer = this.singletonFactory.CreateSingleton(type, args);
            this.singletonObjLst.Add(answer);

            if (answer is IConstructorInvoke constructorInvoke)
            {
                constructorInvoke.OnConstructorInvoke();
            }

            return answer;
        }

        /// <summary>
        /// Gets a singleton instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the singleton.</typeparam>
        /// <returns>The singleton instance.</returns>
        public T GetSingleton<T>() => (T)this.GetSingleton(typeof(T));

        /// <summary>
        /// Gets a singleton instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the singleton.</param>
        /// <returns>The singleton instance.</returns>
        public object GetSingleton(System.Type type) => this.singletonFactory.GetSingleton(type);

        /// <summary>
        /// Sets a singleton instance for the specified type.
        /// </summary>
        /// <param name="singletonObj">The singleton instance.</param>
        /// <param name="type">The type to associate with the singleton.</param>
        public void SetSingleton(object singletonObj, System.Type type)
        {
            if (!this.singletonObjLst.Contains(singletonObj))
            {
                this.singletonObjLst.Add(singletonObj);
            }

            this.singletonFactory.SetSingleton(type, singletonObj);
        }

        /// <summary>
        /// Sets a singleton instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to associate with the singleton.</typeparam>
        /// <param name="singletonObj">The singleton instance.</param>
        public void SetSingleton<T>(T singletonObj) => this.SetSingleton(singletonObj, typeof(T));

        /// <summary>
        /// Gets all singleton instances in the context.
        /// </summary>
        /// <returns>An enumeration of all singleton instances.</returns>
        public System.Collections.Generic.IEnumerable<object> GetSingletons() => this.singletonObjLst;

        /// <summary>
        /// Automatically binds dependencies to the specified singleton object.
        /// </summary>
        /// <param name="singletonObj">The singleton object to bind.</param>
        public void AutoBind(object singletonObj)
        {
            if (singletonObj is IBeforeAutoBind beforeAutoBind)
            {
                beforeAutoBind.OnBeforeAutoBind();
            }

            var type = singletonObj.GetType();

            var fieldInfoLst = new System.Collections.Generic.List<FieldInfo>();
            var propertyInfoLst = new System.Collections.Generic.List<PropertyInfo>();

            var currentCls = type;

            while (true)
            {
                var allDeclaredFields = currentCls
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(field => field.GetCustomAttribute<AutoBindAttribute>(true) != null);

                foreach (var field in allDeclaredFields)
                {
                    if (!fieldInfoLst.Contains(field)) fieldInfoLst.Add(field);
                }

                var allDeclaredProperties = currentCls
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(field => field.GetCustomAttribute<AutoBindAttribute>(true) != null);

                foreach (var property in allDeclaredProperties)
                {
                    if (!propertyInfoLst.Contains(property)) propertyInfoLst.Add(property);
                }

                if (currentCls.BaseType == typeof(object))
                    break;

                currentCls = currentCls.BaseType;
            }

            foreach (var fieldInfo in fieldInfoLst)
            {
                var autoBindAttribute = fieldInfo.GetCustomAttribute<AutoBindAttribute>();
                var value = autoBindAttribute.Type != null ? this.GetSingleton(autoBindAttribute.Type) : this.GetSingleton(fieldInfo.FieldType);
                fieldInfo.SetValue(singletonObj, value);
            }

            foreach (var propertyInfo in propertyInfoLst)
            {
                var autoBindAttribute = propertyInfo.GetCustomAttribute<AutoBindAttribute>();
                var value = autoBindAttribute.Type != null ? this.GetSingleton(autoBindAttribute.Type) : this.GetSingleton(propertyInfo.PropertyType);
                propertyInfo.SetValue(singletonObj, value);
            }

            if (singletonObj is IAfterAutoBind afterAutoBind)
            {
                afterAutoBind.OnAfterAutoBind();
            }
        }

        /// <summary>
        /// Scans and retrieves types within the specified namespace.
        /// </summary>
        /// <param name="prefixNamespace">The namespace to scan.</param>
        /// <returns>An enumeration of types within the specified namespace.</returns>
        public System.Collections.Generic.IEnumerable<System.Type> ScanClass(string prefixNamespace) =>
            this.ScanClass(prefixNamespace, System.AppDomain.CurrentDomain.GetAssemblies());

        /// <summary>
        /// Scans and retrieves types within the specified namespace and assemblies.
        /// </summary>
        /// <param name="prefixNamespace">The namespace to scan.</param>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns>An enumeration of types within the specified namespace and assemblies.</returns>
        public System.Collections.Generic.IEnumerable<System.Type> ScanClass(string prefixNamespace, System.Collections.Generic.IEnumerable<Assembly> assemblies)
        {
            var answer = new System.Collections.Generic.List<System.Type>();

            foreach (var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                answer.AddRange(assemblyTypes.Where(p => p.FullName.StartsWith(prefixNamespace)));
            }

            return answer;
        }

        /// <summary>
        /// Scans and retrieves types that have the specified custom attribute.
        /// </summary>
        /// <param name="type">The custom attribute type to scan for.</param>
        /// <param name="inherit">Whether to scan for inherited attributes.</param>
        /// <returns>An enumeration of types that have the specified custom attribute.</returns>
        public System.Collections.Generic.IEnumerable<System.Type> ScanClassHasCustomAttribute(System.Type type, bool inherit) =>
            this.ScanClassHasCustomAttribute(type, inherit, System.AppDomain.CurrentDomain.GetAssemblies());

        /// <summary>
        /// Scans and retrieves types that have the specified custom attribute within the specified assemblies.
        /// </summary>
        /// <param name="type">The custom attribute type to scan for.</param>
        /// <param name="inherit">Whether to scan for inherited attributes.</param>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns>An enumeration of types that have the specified custom attribute within the specified assemblies.</returns>
        public System.Collections.Generic.IEnumerable<System.Type> ScanClassHasCustomAttribute(System.Type type, bool inherit, System.Collections.Generic.IEnumerable<Assembly> assemblies)
        {
            var answer = new System.Collections.Generic.List<System.Type>();

            foreach (var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                answer.AddRange(assemblyTypes.Where(p => p.GetCustomAttribute(type, inherit) != null));
            }

            return answer;
        }

        /// <summary>
        /// Scans and retrieves types that are assignable to the specified type.
        /// </summary>
        /// <param name="type">The type to scan for assignable types.</param>
        /// <returns>An enumeration of types that are assignable to the specified type.</returns>
        public System.Collections.Generic.IEnumerable<System.Type> ScanClassFromAssignable(System.Type type) =>
            this.ScanClassFromAssignable(type, System.AppDomain.CurrentDomain.GetAssemblies());

        /// <summary>
        /// Scans and retrieves types that are assignable to the specified type within the specified assemblies.
        /// </summary>
        /// <param name="type">The type to scan for assignable types.</param>
        /// <param name="assemblies">The assemblies to scan.</param>
        /// <returns>An enumeration of types that are assignable to the specified type within the specified assemblies.</returns>
        public System.Collections.Generic.IEnumerable<System.Type> ScanClassFromAssignable(System.Type type, System.Collections.Generic.IEnumerable<Assembly> assemblies)
        {
            var answer = new System.Collections.Generic.List<System.Type>();

            foreach (var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                answer.AddRange(assemblyTypes.Where(p => type.IsAssignableFrom(p)));
            }

            return answer;
        }

    }

}
