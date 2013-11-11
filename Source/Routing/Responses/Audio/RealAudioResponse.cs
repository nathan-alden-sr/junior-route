using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Audio
{
	public class RealAudioResponse : ImmutableResponse
	{
		public RealAudioResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioRealAudio().Content(content), configurationDelegate)
		{
		}

		public RealAudioResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioRealAudio().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RealAudioResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioRealAudio().Content(content), configurationDelegate)
		{
		}

		public RealAudioResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioRealAudio().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public RealAudioResponse(Action<Response> configurationDelegate = null)
			: base(new Response().AudioRealAudio(), configurationDelegate)
		{
		}

		public RealAudioResponse(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioRealAudio().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}