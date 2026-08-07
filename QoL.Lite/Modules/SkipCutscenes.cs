using System;
using System.Collections;
using System.Linq;
using GlobalEnums;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using JetBrains.Annotations;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vasi;
using UObject = UnityEngine.Object;

namespace QoL.Modules
{
  [UsedImplicitly]
  public class SkipCutscenes : FauxMod
  {
    public override void Initialize()
    {
      On.CinematicSequence.Begin += CinematicBegin;
      On.FadeSequence.Begin += FadeBegin;
      On.AnimatorSequence.Begin += AnimatorBegin;
      On.InputHandler.SetSkipMode += OnSetSkip;
      On.GameManager.BeginSceneTransitionRoutine += OnBeginSceneTransition;
      On.HutongGames.PlayMaker.Actions.EaseColor.OnEnter += FastEaseColor;
      On.GameManager.FadeSceneInWithDelay += NoFade;
      ModHooks.NewGameHook += OnNewGame;
      UnityEngine.SceneManagement.SceneManager.activeSceneChanged += FsmSkips;
    }
    public override void Unload()
    {
      On.CinematicSequence.Begin -= CinematicBegin;
      On.FadeSequence.Begin -= FadeBegin;
      On.AnimatorSequence.Begin -= AnimatorBegin;
      On.InputHandler.SetSkipMode -= OnSetSkip;
      On.GameManager.BeginSceneTransitionRoutine -= OnBeginSceneTransition;
      On.HutongGames.PlayMaker.Actions.EaseColor.OnEnter -= FastEaseColor;
      On.GameManager.FadeSceneInWithDelay -= NoFade;
      UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= FsmSkips;
      ModHooks.NewGameHook -= OnNewGame;
    }

    private static void OnNewGame()
    {
      PlayerData.instance.SetBool(nameof(PlayerData.hasCharm), true);
      PlayerData.instance.SetBool(nameof(PlayerData.enteredGGAtrium), true);
    }
    private static IEnumerator NoFade(On.GameManager.orig_FadeSceneInWithDelay orig, GameManager self, float delay)
    {
      yield return orig(self, 0);
    }
    private static void FastEaseColor(On.HutongGames.PlayMaker.Actions.EaseColor.orig_OnEnter orig, EaseColor self)
    {
      if (self.Owner.name == "Blanker White" && Math.Abs(self.time.Value - 0.3) < .05)
        self.time.Value = 0.066f;

      orig(self);
    }
    private static void FsmSkips(Scene arg0, Scene arg1)
    {
      var hc = HeroController.instance;

      if (hc == null) return;

      foreach (var Coroutine in new[] { StatueWait, StagCutscene, AbyssShriekPickup, KingsBrand, BlackEgg })
        hc.StartCoroutine(Coroutine(arg1));
    }
    private static IEnumerator StagCutscene(Scene _)
    {
      yield return null;

      GameObject stag = GameObject.Find("Stag");

      if (stag == null)
        yield break;

      PlayMakerFSM ctrl = stag.LocateMyFSM("Stag Control");

      // Remove the wait on the stag animation start, and before being able to interact.
      ctrl.GetState("Arrive Pause").RemoveAction<Wait>();
      ctrl.GetState("Activate").RemoveAction<Wait>();

      var anim = stag.GetComponent<tk2dSpriteAnimator>();

      // Speed up the actual arrival animation.
      anim.GetClipByName("Arrive").fps = 600;

      // Speed up the grate coming up, mostly a thing because of randomizer.
      GameObject grate = ctrl.GetState("Open Grate").GetAction<Tk2dPlayAnimationWithEvents>().gameObject.GameObject.Value;

      if (grate == null)
        yield break;

      var grate_anim = grate.GetComponent<tk2dSpriteAnimator>();

      grate_anim.GetClipByName("Grate_disappear").fps = 600;
    }
    private static IEnumerator StatueWait(Scene arg1)
    {
      if (arg1.name != "GG_Workshop") yield break;

      foreach (PlayMakerFSM fsm in UObject.FindObjectsOfType<PlayMakerFSM>().Where(x => x.FsmName == "GG Boss UI"))
      {
        fsm.GetState("On Left").ChangeTransition("FINISHED", "Dream Box Down");
        fsm.GetState("On Right").ChangeTransition("FINISHED", "Dream Box Down");

        fsm.GetState("Dream Box Down").InsertAction(0, fsm.GetAction<SetPlayerDataString>("Impact"));
      }
    }
    private static IEnumerator AbyssShriekPickup(Scene arg1)
    {
      if (arg1.name != "Abyss_12") yield break;

      yield return null;

      PlayMakerFSM shriek = GameObject.Find("Scream 2 Get").LocateMyFSM("Scream Get");
      shriek.GetState("Move To").ChangeTransition("FINISHED", "Get");
      shriek.GetState("Get Pause").RemoveAllOfType<Wait>();
      shriek.GetState("Land").RemoveAllOfType<Wait>();
    }
    private static IEnumerator KingsBrand(Scene arg1)
    {
      yield return KingsBrandHornet(arg1);
      yield return KingsBrandAvalanche(arg1);
    }
    private static IEnumerator KingsBrandHornet(Scene arg1)
    {
      if (arg1.name != "Deepnest_East_12") yield break;

      yield return null;

      PlayMakerFSM hornet = GameObject.Find("Hornet Blizzard Return Scene").LocateMyFSM("Control");

      hornet.GetState("Fade Pause").RemoveAllOfType<Wait>();
      hornet.GetState("Fade In").RemoveAllOfType<Wait>();
      hornet.GetState("Land").RemoveAllOfType<Wait>();
    }
    private static IEnumerator KingsBrandAvalanche(Scene arg1)
    {
      if (arg1.name != "Room_Wyrm") yield break;

      yield return null;

      PlayMakerFSM avalanche = GameObject.Find("Avalanche End").LocateMyFSM("Control");
      avalanche.GetState("Fade").GetAction<Wait>().time = 1;
    }
    private static IEnumerator BlackEgg(Scene arg1)
    {
      if (arg1.name != "Room_temple") yield break;

      yield return null;

      PlayMakerFSM door = GameObject.Find("Final Boss Door").LocateMyFSM("Control");

      door.GetState("Take Control").RemoveAllOfType<Wait>();
      door.GetState("Shake").GetAction<Wait>().time = 1;
      door.GetState("Barrier Flash").RemoveAllOfType<Wait>();
      door.GetState("Blow").RemoveAllOfType<Wait>();
      door.GetState("Door Off").RemoveAllOfType<Wait>();
      door.GetState("Roar").RemoveAllOfType<Wait>();
      door.GetState("Roar End").GetAction<Wait>().time = 1;
    }
    private static IEnumerator OnBeginSceneTransition(On.GameManager.orig_BeginSceneTransitionRoutine orig, GameManager self, GameManager.SceneLoadInfo info)
    {
      info.EntryDelay = 0f;

      yield return orig(self, info);

      yield break;
    }
    private static void OnSetSkip(On.InputHandler.orig_SetSkipMode orig, InputHandler self, SkipPromptMode newmode)
    {
      if (newmode == SkipPromptMode.SKIP_PROMPT)
      {
        newmode = SkipPromptMode.SKIP_INSTANT;
      }

      orig(self, newmode);
    }
    private static void AnimatorBegin(On.AnimatorSequence.orig_Begin orig, AnimatorSequence self)
    {
      self.Skip();
    }
    private static void FadeBegin(On.FadeSequence.orig_Begin orig, FadeSequence self)
    {
      self.Skip();
    }
    private static void CinematicBegin(On.CinematicSequence.orig_Begin orig, CinematicSequence self)
    {
      self.Skip();
    }
  }
}