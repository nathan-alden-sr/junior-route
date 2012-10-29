using System;
using System.IO;
using System.Reflection;
using System.Web;

using Junior.Common;

using Newtonsoft.Json;

namespace Junior.Route.AutoRouting.ParameterMappers
{
	public class JsonModelMapper : IParameterMapper
	{
		private readonly DataConversionErrorHandling _errorHandling;
		private readonly Func<Type, bool> _parameterTypeMatchDelegate;
		private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
			{
				DateFormatHandling = DateFormatHandling.IsoDateFormat
			};

		public JsonModelMapper(Func<Type, bool> parameterTypeMatchDelegate, JsonSerializerSettings serializerSettings, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
		{
			parameterTypeMatchDelegate.ThrowIfNull("parameterTypeMatchDelegate");
			serializerSettings.ThrowIfNull("serializerSettings");

			_parameterTypeMatchDelegate = parameterTypeMatchDelegate;
			_serializerSettings = serializerSettings;
			_errorHandling = errorHandling;
		}

		public JsonModelMapper(Func<Type, bool> parameterTypeMatchDelegate, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
			: this(parameterTypeMatchDelegate, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat })
		{
		}

		public JsonModelMapper(JsonSerializerSettings serializerSettings, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
			: this(type => type.Name.EndsWith("Model"), serializerSettings, errorHandling)
		{
		}

		public JsonModelMapper(DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
			: this(type => type.Name.EndsWith("Model"), new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat })
		{
		}

		public bool CanMapType(HttpRequestBase request, Type parameterType)
		{
			request.ThrowIfNull("request");
			parameterType.ThrowIfNull("parameterType");

			return request.Headers["Content-Type"] == "application/json" && _parameterTypeMatchDelegate(parameterType);
		}

		public MapResult Map(HttpRequestBase request, Type type, MethodInfo method, ParameterInfo parameter)
		{
			request.ThrowIfNull("request");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			parameter.ThrowIfNull("parameter");

			Type parameterType = parameter.ParameterType;
			var reader = new StreamReader(request.InputStream, request.ContentEncoding);
			string json = reader.ReadToEnd();
			object jsonModel;

			try
			{
				jsonModel = JsonConvert.DeserializeObject(json, parameterType, _serializerSettings);
			}
			catch (Exception exception)
			{
				if (_errorHandling == DataConversionErrorHandling.ThrowException)
				{
					throw new ApplicationException(String.Format("Request content could not be deserialized to '{0}'.", parameterType.FullName), exception);
				}
				jsonModel = parameterType.GetDefaultValue();
			}

			return MapResult.ValueMapped(jsonModel);
		}
	}
}