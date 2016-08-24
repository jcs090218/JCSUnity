namespace Soomla.Singletons
{
#pragma warning disable 618
    /// <summary>
    /// A Singleton for use when your component needs to be blaced in a scene on design time.
    /// To use, override <see cref="UnitySingleton.InitAfterRegisteringAsSingleInstance"/> and write your initialization logic.
    /// If your singleton shouldn't be destroyed when moving between scenes (<see cref="UnityEngine.Object.DontDestroyOnLoad"/>),
    /// Override <see cref="UnitySingleton.DontDestroySingleton"/> and return true.
	/// Like this:
    /// 
    ///     protected override bool DontDestroySingleton
    ///     {
    ///          get { return true; }
    ///     }
    /// 
    /// </summary>
    public abstract class SceneSingleton : UnitySingleton
    {
    } 
}