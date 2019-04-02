using FishDex.Components;
using FishDex.Helpers;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System.Linq;

namespace FishDex
{
	/// <summary>The mod entry point.</summary>
	public class ModEntry : Mod
	{
		/*********
		** Fields
		*********/
		/****
        ** Configuration
        ****/
		/// <summary>The mod configuration.</summary>
		private ModConfig Config;

		/*********
        ** Public methods
        *********/
		/// <summary>The mod entry point, called after the mod is first loaded.</summary>
		/// <param name="helper">Provides simplified APIs for writing mods.</param>
		public override void Entry(IModHelper helper)
		{
			// load config
			this.Config = this.Helper.ReadConfig<ModConfig>();

			// hook up events
			helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
			helper.Events.Input.ButtonPressed += this.OnButtonPressed;
		}

		/*********
		** Private methods
		*********/
		/****
        ** Event handlers
        ****/
		/// <summary>The method invoked on the first update tick, once all mods are initialised.</summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
		{
			// initialise functionality
		}

		/// <summary>The method invoked when the player presses a button.</summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
		{
			this.Monitor.InterceptErrors("handling your input", $"handling input '{e.Button}'", () =>
			{
				var controls = this.Config.Controls;

				if (controls.ToggleFishMenu.Contains(e.Button))
					this.ToggleFishMenu();
				else if (controls.ScrollUp.Contains(e.Button))
					(Game1.activeClickableMenu as FishMenu)?.ScrollUp();
				else if (controls.ScrollDown.Contains(e.Button))
					(Game1.activeClickableMenu as FishMenu)?.ScrollDown();
			});
		}

		/****
		** Helpers
		****/
		/// <summary>Show the fish menu.</summary>
		private void ToggleFishMenu()
		{
			if (Game1.activeClickableMenu is FishMenu)
				this.HideFishMenu();
			else
			{
				this.Monitor.InterceptErrors("opening fish menu", () =>
				{
					this.Monitor.Log($"Opening fish menu", LogLevel.Trace);
				});
			}
		}

		/// <summary>Hide the fish menu.</summary>
		private void HideFishMenu()
		{
			this.Monitor.InterceptErrors("closing the menu", () =>
			{
				if (Game1.activeClickableMenu is FishMenu)
				{
					Game1.playSound("bigDeSelect"); // match default behaviour when closing a menu
					Game1.activeClickableMenu = null;
				}
			});
		}

		/// <summary>Get whether a given menu should be restored.</summary>
		/// <param name="menu">The menu to check.</param>
		private bool ShouldRestoreMenu(IClickableMenu menu)
		{
			// no menu
			if (menu == null)
				return false;

			return true;
		}
	}
}