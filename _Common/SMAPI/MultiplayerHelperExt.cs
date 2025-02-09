﻿using Shockah.CommonModCode.Stardew;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Linq;

namespace Shockah.CommonModCode.SMAPI
{
	public static class MultiplayerHelperExt
	{
		public static void SendMessage<TMessage>(this IMultiplayerHelper self, TMessage message, string[]? modIDs = null, long[]? playerIDs = null)
			where TMessage : notnull
			=> self.SendMessage(message, typeof(TMessage).GetBestName(), modIDs, playerIDs);

		public static void SendMessageInMultiplayer<TMessage>(this IMultiplayerHelper self, Func<TMessage> message, string type, string[]? modIDs = null, long[]? playerIDs = null)
			where TMessage : notnull
		{
			if (GameExt.GetMultiplayerMode() != MultiplayerMode.SinglePlayer)
				self.SendMessage(message(), type, modIDs, playerIDs);
		}

		public static void SendMessageInMultiplayer<TMessage>(this IMultiplayerHelper self, Func<TMessage> message, string[]? modIDs = null, long[]? playerIDs = null)
			where TMessage : notnull
			=> self.SendMessageInMultiplayer(message, typeof(TMessage).GetBestName(), modIDs, playerIDs);

		public static Farmer GetPlayer(this IMultiplayerPeer peer)
			=> Game1.getAllFarmers().First(p => p.UniqueMultiplayerID == peer.PlayerID);

		public static Farmer GetPlayer(this ModMessageReceivedEventArgs args)
			=> Game1.getAllFarmers().First(p => p.UniqueMultiplayerID == args.FromPlayerID);
	}
}