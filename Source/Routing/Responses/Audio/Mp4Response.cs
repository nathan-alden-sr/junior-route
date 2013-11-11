using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Audio
{
	public class Mp4Response : ImmutableResponse
	{
		public Mp4Response(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMp4().Content(content), configurationDelegate)
		{
		}

		public Mp4Response(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMp4().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public Mp4Response(byte[] content, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMp4().Content(content), configurationDelegate)
		{
		}

		public Mp4Response(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMp4().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public Mp4Response(Action<Response> configurationDelegate = null)
			: base(new Response().AudioMp4(), configurationDelegate)
		{
		}

		public Mp4Response(Encoding encoding, Action<Response> configurationDelegate = null)
			: base(new Response().AudioMp4().ContentEncoding(encoding), configurationDelegate)
		{
		}
	}
}