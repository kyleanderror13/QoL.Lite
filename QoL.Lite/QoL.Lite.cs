using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Modding;
using MonoMod.ModInterop;
using QoL.Modules;
using Vasi;

namespace QoL
{
  [UsedImplicitly]
  public class QoLLite : Mod, ITogglableMod
  {
    public override string GetVersion() => VersionUtil.GetVersion<QoLLite>();

    public override void Initialize()
    {
      foreach (Type t in Assembly.GetAssembly(typeof(QoLLite)).GetTypes().Where(x => x.IsSubclassOf(typeof(FauxMod))))
      {
        var fm = (FauxMod)Activator.CreateInstance(t);

        fm.Initialize();
        fm.IsLoaded = true;

        _FauxMods.Add(fm);
      }
    }
    public void Unload()
    {
      foreach (FauxMod fm in _FauxMods.Where(x => x.IsLoaded))
      {
        fm.Unload();
      }
    }

    private static readonly List<FauxMod> _FauxMods = new();
  }
}