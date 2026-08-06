using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace QoL.Modules
{
  [UsedImplicitly]
  public class FastMenus : FauxMod
  {
    public override void Initialize()
    {
      UIManager.instance.MENU_FADE_SPEED = 14f;
    }

    public override void Unload()
    {
      UIManager.instance.MENU_FADE_SPEED = 3.2f;
    }
  }
}