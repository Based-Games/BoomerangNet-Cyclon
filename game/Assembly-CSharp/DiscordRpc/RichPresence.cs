using System;
using System.Runtime.InteropServices;

namespace DiscordRpc
{
	// Token: 0x02000441 RID: 1089
	public struct RichPresence
	{
		// Token: 0x04001C09 RID: 7177
		[MarshalAs(UnmanagedType.LPStr)]
		public string state;

		// Token: 0x04001C0A RID: 7178
		[MarshalAs(UnmanagedType.LPStr)]
		public string details;

		// Token: 0x04001C0B RID: 7179
		public long startTimestamp;

		// Token: 0x04001C0C RID: 7180
		public long endTimestamp;

		// Token: 0x04001C0D RID: 7181
		[MarshalAs(UnmanagedType.LPStr)]
		public string largeImageKey;

		// Token: 0x04001C0E RID: 7182
		[MarshalAs(UnmanagedType.LPStr)]
		public string largeImageText;

		// Token: 0x04001C0F RID: 7183
		[MarshalAs(UnmanagedType.LPStr)]
		public string smallImageKey;

		// Token: 0x04001C10 RID: 7184
		[MarshalAs(UnmanagedType.LPStr)]
		public string smallImageText;

		// Token: 0x04001C11 RID: 7185
		[MarshalAs(UnmanagedType.LPStr)]
		public string partyId;

		// Token: 0x04001C12 RID: 7186
		public int partySize;

		// Token: 0x04001C13 RID: 7187
		public int partyMax;

		// Token: 0x04001C14 RID: 7188
		public int partyPrivacy;

		// Token: 0x04001C15 RID: 7189
		[MarshalAs(UnmanagedType.LPStr)]
		public string matchSecret;

		// Token: 0x04001C16 RID: 7190
		[MarshalAs(UnmanagedType.LPStr)]
		public string joinSecret;

		// Token: 0x04001C17 RID: 7191
		[MarshalAs(UnmanagedType.LPStr)]
		public string spectateSecret;

		// Token: 0x04001C18 RID: 7192
		public sbyte instance;
	}
}
