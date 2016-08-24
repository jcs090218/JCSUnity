using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Soomla.Singletons
{
    /// <summary>
    /// An internal class that servers as a base class for singletons
    /// </summary>
    public abstract class UnitySingleton : BaseBehaviour
    {
        #region Private Variables
        private static readonly Dictionary<Type, UnitySingleton> instances = new Dictionary<Type, UnitySingleton>();
        private static readonly Dictionary<Type, Dictionary<MonoBehaviour, Action<UnitySingleton>>> instanceListeners =
                            new Dictionary<Type, Dictionary<MonoBehaviour, Action<UnitySingleton>>>();
        #endregion

        #region Private Properties
        protected bool IsInstanceReady { get; private set; }
        #endregion

        #region Private Functions
        private void RegisterAsSingleInstanceAndInit()
        {
            instances.Add(GetType(), this);
            InnerInit();
        }

        private void InnerInit()
        {
            InitAfterRegisteringAsSingleInstance();

            if (DontDestroySingleton)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private static S GetOrCreateInstanceOnGameObject<S>(Type type) where S : CodeGeneratedSingleton
        {
            S instance = null;

            var prefab = Resources.Load<GameObject>(type.Name);

            if (prefab)
            {
                var instantiatedObject = Instantiate(prefab)
#if !UNITY_5
        as GameObject
#endif
                    ;

                if (!instantiatedObject)
                {
                    throw new Exception("Failed to instantiate prefab: " + type.Name);
                }

                instance = instantiatedObject.GetComponent<S>();

                if (!instance)
                {
                    instance = instantiatedObject.AddComponent<S>();
                }
            }

            if (!instance)
            {
                instance = new GameObject(type.Name).AddComponent<S>();
            }

            return instance;
        }

        private void NotifyInstanceListeners()
        {
            var type = GetType();

            // Checks if there are any registered listeners for this type of singleton
            if (instanceListeners.ContainsKey(type))
            {
                foreach (var actionWithSender in instanceListeners[type].ToArray())
                {
                    // If the sender is alive and has listeners - run its actions
                    if (actionWithSender.Key && actionWithSender.Value != null)
                    {
                        actionWithSender.Value(this);
                    }

                    // Either way - remove the sender + action from the collection
                    instanceListeners[type].Remove(actionWithSender.Key);
                }
            }
        }

        protected void DeclareAsReady()
        {
            IsInstanceReady = true;

            NotifyInstanceListeners();
        }
        #endregion

        #region Public Functions
        protected static S GetSynchronousCodeGeneratedInstance<S>() where S : CodeGeneratedSingleton
        {
            var type = typeof(S);

            S instance;

            // An instance of this type does not exist
            if (!instances.ContainsKey(type))
            {
                // Try to find an existing one in the scene
                instance = FindObjectOfType<S>();

                if (!instance)
                {
                    // Creating a new one
                    instance = GetOrCreateInstanceOnGameObject<S>(type);
                }

                instance.RegisterAsSingleInstanceAndInit();
            }
            // An instance of this type already exists
            else
            {
                instance = instances[type] as S;
            }

            if (!instance)
            {
                throw new Exception("No instance was created: " + type.Name);
            }

            instance.IsInstanceReady = true;

            return instance;
        }

        public static void DoWithCodeGeneratedInstance<C>(MonoBehaviour sender, Action<C> whatToDoWithInstanceWhenItsReady) where C : CodeGeneratedSingleton
        {
            // Make sure an instance exists (creating it if it doesn't)
            GetSynchronousCodeGeneratedInstance<C>();

            // Do the action with the existing instance or wait for it to be ready
            DoWithExistingInstance(sender, whatToDoWithInstanceWhenItsReady);
        }

        public static void DoWithSceneInstance<S>(MonoBehaviour sender, Action<S> whatToDoWithInstanceWhenItsReady) where S : SceneSingleton
        {
            DoWithExistingInstance(sender, whatToDoWithInstanceWhenItsReady);
        }

        /// <summary>
        /// Performs an action on an existing instance if and when it exists and ready
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="sender"></param>
        /// <param name="whatToDoWithInstanceWhenItsReady"></param>
        private static void DoWithExistingInstance<S>(MonoBehaviour sender, Action<S> whatToDoWithInstanceWhenItsReady) where S : UnitySingleton
        {
            var type = typeof(S);

            var isInstanceNotExistOrReady = true;

            // An instance of this type exists
            if (instances.ContainsKey(type))
            {
                var instance = instances[type] as S;

                if (instance && instance.IsInstanceReady)
                {
                    isInstanceNotExistOrReady = false;

                    // Call the action with the existing instance
                    whatToDoWithInstanceWhenItsReady(instance);
                }
            }

            // An instance of this type does not exist, we have to wait for it to initialize
            if (isInstanceNotExistOrReady)
            {
                if (!instanceListeners.ContainsKey(type))
                {
                    instanceListeners.Add(type, new Dictionary<MonoBehaviour, Action<UnitySingleton>>());
                }

                if (!instanceListeners[type].ContainsKey(sender))
                {
                    instanceListeners[type].Add(sender, null);
                }

                instanceListeners[type][sender] += singleton => whatToDoWithInstanceWhenItsReady(singleton as S);
            }
        }
        #endregion

        #region Unity Functions
        protected sealed override void Start()
        {
            base.Start();

            var type = GetType();

            var needToDestroy = false;

            // There's already an instance of my type
            if (instances.ContainsKey(type))
            {
                // The existing instance is not this instance so we've got a conflict (There can only be one!)
                if (instances[type] != this)
                {
                    if (this is CodeGeneratedSingleton)
                    {
                        throw new Exception("There's already an instance for " + type.Name);
                    }

                    if (this is SceneSingleton)
                    {
                        if (DontDestroySingleton)
                        {
                            // [this] is not the single instance (Singleton) so we actually DO need to destroy it
                            needToDestroy = true;
                        }
                    }
                }
            }
            // There's no instance of my type and I'm a CodeGeneratedSingleton
            // It should have been created via code so if I don't exist in the instance collection it means I was created on a scene
            else if (this is CodeGeneratedSingleton)
            {
                throw new NotSupportedException(string.Format("{0} is a {1} and needs to be created via code, and not placed on a scene!", type.Name, typeof(CodeGeneratedSingleton).Name));
            }

            if (needToDestroy)
            {
				Debug.LogWarning(string.Format("There's already a {0} instance on the current scene, there's no point in staying.. goodbye.. I'm gonna go now :(", type.Name));
				
                Destroy(this);
            }
            else if (this is SceneSingleton)
            {
                RegisterAsSingleInstanceAndInit();

                SetReadyAndNotifyAfterRegistering();
            }
        }

        /// <summary>
        /// Override this if the singleton won't be ready immediately after registering as single instance
        /// </summary>
        protected virtual void SetReadyAndNotifyAfterRegistering()
        {
            DeclareAsReady();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            var type = GetType();

            // There's already an instance of my type but it's me - remove me
            if (instances.ContainsKey(type) && instances[type] == this)
            {
                instances.Remove(type);
            }
        }
        #endregion

        #region Virtuals
        protected virtual void InitAfterRegisteringAsSingleInstance() { }
        protected virtual bool DontDestroySingleton { get { return false; } }
        #endregion
    } 
}