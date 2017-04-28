//  <copyright file="AppServiceLocator.cs" company="Microsoft">
//      © Vcyber - All rights reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Vcyber.BLMS.Common
{
    /// <summary>
    /// An implementation of a Service Locator for use in CMS applications.
    /// </summary>
    public class AppServiceLocator
    {
        #region Private Variables

        /// <summary>
        /// A UnityContainer object for managing service instances.
        /// </summary>
        private IUnityContainer container;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="AppServiceLocator" /> class.
        /// </summary>
        static AppServiceLocator()
        {
            Instance = new AppServiceLocator();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceLocator"/> class.
        /// </summary>
        public AppServiceLocator()
            : this(new UnityContainer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceLocator"/> class.
        /// </summary>
        /// <param name="container">A UnityContainer to be used for managing service instances</param>
        public AppServiceLocator(IUnityContainer container)
        {
            this.container = container;

            // Load the default container configuration
            this.container.LoadConfiguration();
        }

        #endregion

        /// <summary>
        /// Gets an instance of AppServiceLocator
        /// </summary>
        public static AppServiceLocator Instance { get; private set; }

        #region Public Methods

        /// <summary>
        /// Indicates whether a type has been registered.
        /// </summary>
        /// <typeparam name="T">Type to check</typeparam>
        /// <returns>True if the type has been registered.</returns>
        public bool IsRegistered<T>()
        {
            return this.container.IsRegistered<T>();
        }

        /// <summary>
        /// Register a Type and resolve an instance.
        /// </summary>
        /// <typeparam name="TFrom">Interface Type</typeparam>
        /// <typeparam name="TTo">Implementation Type</typeparam>
        public void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            this.container.RegisterType<TFrom, TTo>();
        }

        /// <summary>
        /// Register a Type and resolve an instance.
        /// </summary>
        /// <typeparam name="TFrom">Interface Type</typeparam>
        /// <typeparam name="TTo">Implementation Type</typeparam>
        /// <param name="injectionMembers">Array of injection members</param>
        public void RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            this.container.RegisterType<TFrom, TTo>(injectionMembers);
        }

        /// <summary>
        /// Register a Type and resolve an instance.
        /// </summary>
        /// <typeparam name="TFrom">Interface Type</typeparam>
        /// <typeparam name="TTo">Implementation Type</typeparam>
        /// <param name="name">The name of the item to register.</param>
        /// <param name="injectionMembers">Array of injection members</param>
        public void RegisterType<TFrom, TTo>(string name, params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            this.container.RegisterType<TFrom, TTo>(name, injectionMembers);
        }

        /// <summary>
        /// Register an instance of a Type
        /// </summary>
        /// <typeparam name="TInterface">Type to be registered</typeparam>
        /// <param name="instance">Instance to be registered.</param>
        public void RegisterInstance<TInterface>(TInterface instance)
        {
            this.container.RegisterInstance<TInterface>(instance);
        }

        /// <summary>
        /// Register an instance of a Type
        /// </summary>
        /// <typeparam name="TInterface">Type to be registered</typeparam>
        /// <param name="name">The name of the item to register.</param>
        /// <param name="instance">Instance to be registered.</param>
        public void RegisterInstance<TInterface>(string name, TInterface instance)
        {
            this.container.RegisterInstance<TInterface>(name, instance);
        }

        /// <summary>
        /// Retrieve an instance of the requested service.
        /// </summary>
        /// <typeparam name="T">Type of instance requested.</typeparam>
        /// <returns>A resolved instance of the requested service.</returns>
        public T GetInstance<T>()
        {
            return this.container.Resolve<T>();
        }

        /// <summary>
        /// Retrieve an instance of the requested service.
        /// </summary>
        /// <typeparam name="T">Type of instance requested.</typeparam>
        /// <param name="key">Name of the specific instance of registered service requested.  May be null.</param>
        /// <returns>A resolved instance of the requested service.</returns>
        public T GetInstance<T>(string key)
        {
            return this.container.Resolve<T>(key);
        }

        /// <summary>
        /// Tries to get an Instance and returns false when not found rather than throw an exception
        /// </summary>
        /// <typeparam name="T">The type of Instance to get</typeparam>
        /// <param name="key">The key of the Instace to get</param>
        /// <param name="result">The result to assign the Instance to</param>
        /// <returns>A true if the get succeeds, otherwise false</returns>
        public bool TryGetInstance<T>(string key, out T result)
        {
            result = default(T); 
            try
            {
                result = this.container.Resolve<T>(key);
            }
            catch
            {
                return false; 
            }

            return true;
        }

        /// <summary>
        /// Retrieve an instance of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <returns>A resolved instance of the requested service.</returns>
        public object GetInstance(Type serviceType)
        {
            return this.container.Resolve(serviceType);
        }

        /// <summary>
        /// Retrieve an instance of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of the specific instance of registered service requested.  May be null.</param>
        /// <returns>A resolved instance of the requested service.</returns>
        public object GetInstance(Type serviceType, string key)
        {
            return this.container.Resolve(serviceType, key);
        }

        /// <summary>
        /// Retrieves all instances of the requested service.
        /// </summary>
        /// <typeparam name="T">Type of the service requested.</typeparam>
        /// <returns>A collection of all the resolved instances of the requested service.</returns>
        public IEnumerable<T> GetAllInstances<T>()
        {
            return this.container.ResolveAll<T>();
        }
        
        /// <summary>
        /// Retrieves all instances of the requested service.
        /// </summary>
        /// <param name="serviceType">Type of the service requested.</param>
        /// <returns>A collection of all the resolved instances of the requested service.</returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.container.ResolveAll(serviceType);
        }

        #endregion
    }
}
