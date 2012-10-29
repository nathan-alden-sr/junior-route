using System;
using System.Text;

namespace Junior.Route.Routing.Responses.Model
{
	public class MeshResponse : ImmutableResponse
	{
		public MeshResponse(Func<byte[]> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelMesh().Content(content), configurationDelegate)
		{
		}

		public MeshResponse(Func<byte[]> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelMesh().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MeshResponse(Func<string> content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelMesh().Content(content), configurationDelegate)
		{
		}

		public MeshResponse(Func<string> content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelMesh().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MeshResponse(byte[] content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelMesh().Content(content), configurationDelegate)
		{
		}

		public MeshResponse(byte[] content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelMesh().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}

		public MeshResponse(string content, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelMesh().Content(content), configurationDelegate)
		{
		}

		public MeshResponse(string content, Encoding encoding, Action<Response> configurationDelegate = null)
			: base(Response.OK().ModelMesh().ContentEncoding(encoding).Content(content), configurationDelegate)
		{
		}
	}
}