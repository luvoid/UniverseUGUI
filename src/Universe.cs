using HarmonyLib;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UniverseLib.Config;
using UniverseLib.Input;
using UniverseLib.Runtime;
using UniverseLib.UI;
#if INTEROP
using Il2CppInterop.Runtime;
using IL2CPPUtils = Il2CppInterop.Common.Il2CppInteropUtils;
#endif
#if UNHOLLOWER
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using IL2CPPUtils = UnhollowerBaseLib.UnhollowerUtils;
#endif

namespace UniverseLib
{
    public class Universe
    {
        public enum GlobalState
        {
            WaitingToSetup,
            SettingUp,
            SetupCompleted
        }

        public const string NAME = "UniverseUGUI";
        public const string VERSION = "1.6.3";
        public const string AUTHOR = "luvoid";
        public const string GUID = "luvoid.universeugui";

        /// <summary>The current runtime context (Mono or IL2CPP).</summary>
        public static RuntimeContext Context { get; } =
#if MONO
            RuntimeContext.Mono;
#else
            RuntimeContext.IL2CPP;
#endif

        /// <summary>The current setup state of UniverseLib.</summary>
        public static GlobalState CurrentGlobalState { get; private set; }

        internal static Harmony Harmony { get; } = new Harmony(GUID);

        static float startupDelay;
        static event Action OnInitialized;

        static bool hasLogHandler;
        internal static UniverseLogger Logger = new UniverseLogger(new UniverseLogHandler(new DebugLogHandler(), true));
        internal static UniverseLogger Debug  = new UniverseLogger(Logger.logHandler);


        /// <inheritdoc cref="Init(Action, ILogHandler)"/>
        [Obsolete($"Use Universe.Init(Action, ILogHandler) instead.")]
        public static void Init(Action onInitialized = null, Action<string, LogType> logHandler = null)
            => Init(1f, onInitialized, logHandler, default);

        /// <inheritdoc cref="Init(float, Action, ILogHandler, UniverseLibConfig)"/>
        [Obsolete($"Use Universe.Init(float, Action, ILogHandler, UniverseLibConfig) instead.")]
        public static void Init(float startupDelay, Action onInitialized, Action<string, LogType> logHandler, UniverseLibConfig config)
            => Init(startupDelay, onInitialized, new LegacyLogHandler(logHandler), config);

        /// <summary>
        /// Initialize UniverseLib with default settings, if you don't require any finer control over the startup process.
        /// </summary>
        /// <inheritdoc cref="Init(float, Action, ILogHandler, UniverseLibConfig)"/>
        public static void Init(Action onInitialized = null, ILogHandler logHandler = null)
            => Init(1f, onInitialized, logHandler, default);

        /// <summary>
        /// Initialize UniverseLib with the provided parameters.
        /// </summary>
        /// <param name="startupDelay">Will be used only if it is the highest value supplied to this method compared to other assemblies.
        /// If another assembly calls this Init method with a higher startupDelay, their value will be used instead.</param>
        /// <param name="onInitialized">Invoked after the <paramref name="startupDelay"/> and after UniverseLib has finished initializing.</param>
        /// <param name="logHandler">Should be used for printing UniverseLib's own internal logs. Your listener will only be used if no listener has 
        /// yet been provided to handle it. It is not required to implement this but it may be useful to diagnose internal errors.</param>
        /// <param name="config">Can be used to set certain values of UniverseLib's configuration. Null config values will be ignored.</param>
        public static void Init(float startupDelay, Action onInitialized, ILogHandler logHandler, UniverseLibConfig config)
        {
            // If already finished intializing, just return (and invoke onInitialized if supplied)
            if (CurrentGlobalState == GlobalState.SetupCompleted)
            {
                InvokeOnInitialized(onInitialized);
                return;
            }

            // Only use the provided startup delay if its higher than the current value
            if (startupDelay > Universe.startupDelay)
                Universe.startupDelay = startupDelay;

            ConfigManager.LoadConfig(config);

            OnInitialized += onInitialized;

            if (logHandler != null && !hasLogHandler)
            {
                Logger.logHandler = Debug.logHandler = new UniverseLogHandler(logHandler, false);
                hasLogHandler = true;
            }

            if (CurrentGlobalState == GlobalState.WaitingToSetup)
            {
                CurrentGlobalState = GlobalState.SettingUp;
                Debug.Log($"{NAME} {VERSION} initializing...");

                // Run immediate setups which don't require any delay
                InitialSetup();

                // Begin the startup delay coroutine
                RuntimeHelper.Instance.Internal_StartCoroutine(SetupCoroutine());

                Debug.Log($"Finished {NAME} initial setup.");
            }
        }

		private static bool _didInitialSetup = false;
		/// <summary>
		/// Run immediate setups which don't require any delays.
		/// This will not fully initialize until <see cref="Init"/> is called.
		/// </summary>
		internal static void InitialSetup()
		{
            if (_didInitialSetup) return;

			UniversalBehaviour.Setup();
			ReflectionUtility.Init();
			RuntimeHelper.Init();

			_didInitialSetup = true;
		}

        internal static void Update()
        {
            UniversalUI.Update();
        }

        private static IEnumerator SetupCoroutine()
        {
            // Always yield at least one frame, otherwise if the first startupDelay is 0f this would run immediately and
            // not allow other Init calls to set a higher delay.
            yield return null;

            Stopwatch sw = new();
            sw.Start();
            while (ReflectionUtility.Initializing || sw.ElapsedMilliseconds * 0.001f < startupDelay)
                yield return null;

            // Initialize late startup processes
            InputManager.Init();
            UniversalUI.Init();

            Logger.Log($"{NAME} {VERSION} initialized.");
            CurrentGlobalState = GlobalState.SetupCompleted;

            InvokeOnInitialized(OnInitialized);
        }

        private static void InvokeOnInitialized(Action onInitialized)
        {
            if (onInitialized == null)
                return;

            foreach (Delegate listener in onInitialized.GetInvocationList())
            {
                try
                {
                    listener.DynamicInvoke();
                }
                catch (Exception ex)
                {
                    Logger.LogException($"Exception invoking onInitialized callback!", ex);
                }
            }
        }

        // UniverseLib internal logging. These are assumed to be handled by a logHandler supplied to Init().
        // Not for external use.

        [Obsolete]
        internal static void Log(object message)
            => Log(message, LogType.Log);

        [Obsolete]
        internal static void LogWarning(object message)
            => Log(message, LogType.Warning);

        [Obsolete]
        internal static void LogError(object message)
            => Log(message, LogType.Error);

        [Obsolete]
        static void Log(object message, LogType logType)
        {
            Debug.Log(logType, message);
        }

        // Patching helpers

        internal static bool Patch(Type type, string methodName, MethodType methodType, Type[] arguments = null,
            MethodInfo prefix = null, MethodInfo postfix = null, MethodInfo finalizer = null)
        {
            try
            {
                string namePrefix = methodType switch
                {
                    MethodType.Getter => "get_",
                    MethodType.Setter => "set_",
                    _ => string.Empty
                };

                MethodInfo target;
                if (arguments != null)
                    target = type.GetMethod($"{namePrefix}{methodName}", AccessTools.all, null, arguments, null);
                else
                    target = type.GetMethod($"{namePrefix}{methodName}", AccessTools.all);

                if (target == null)
                {
                    // LogWarning($"\t Couldn't find any method on type {type.FullName} called {methodName}!");
                    return false;
                }

#if CPP
                // if this is an IL2CPP type, ensure method wasn't stripped.
                if (Il2CppType.From(type, false) != null
                    && IL2CPPUtils.GetIl2CppMethodInfoPointerFieldForGeneratedMethod(target) == null)
                {
                    Log($"\t IL2CPP method has no corresponding pointer, aborting patch of {type.FullName}.{methodName}");
                    return false;
                }
#endif

                PatchProcessor processor = Harmony.CreateProcessor(target);

                if (prefix != null)
                    processor.AddPrefix(new HarmonyMethod(prefix));
                if (postfix != null)
                    processor.AddPostfix(new HarmonyMethod(postfix));
                if (finalizer != null)
                    processor.AddFinalizer(new HarmonyMethod(finalizer));

                processor.Patch();

                // Log($"\t Successfully patched {type.FullName}.{methodName}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"\t Exception patching {type.FullName}.{methodName}: {ex}");
                return false;
            }
        }

        internal static bool Patch(Type type, string[] possibleNames, MethodType methodType, Type[] arguments = null,
            MethodInfo prefix = null, MethodInfo postfix = null, MethodInfo finalizer = null)
        {
            foreach (var name in possibleNames)
            {
                if (Patch(type, name, methodType, arguments, prefix, postfix, finalizer))
                    return true;
            }
            return false;
        }

        internal static bool Patch(Type type, string[] possibleNames, MethodType methodType, Type[][] possibleArguments,
            MethodInfo prefix = null, MethodInfo postfix = null, MethodInfo finalizer = null)
        {
            foreach (var name in possibleNames)
            {
                foreach (var arguments in possibleArguments)
                {
                    if (Patch(type, name, methodType, arguments, prefix, postfix, finalizer))
                        return true;
                }
            }
            return false;
        }

        internal static bool Patch(Type type, string methodName, MethodType methodType, Type[][] possibleArguments,
            MethodInfo prefix = null, MethodInfo postfix = null, MethodInfo finalizer = null)
        {
            foreach (var arguments in possibleArguments)
            {
                if (Patch(type, methodName, methodType, arguments, prefix, postfix, finalizer))
                    return true;
            }
            return false;
        }
    }
}
