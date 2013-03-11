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
			: this(parameterTypeMatchDelegate, new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat }, errorHandling)
		{
		}

		public JsonModelMapper(JsonSerializerSettings serializerSettings, DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
			: this(type => type.Name.EndsWith("Model"), serializerSettings, errorHandling)
		{
		}

		public JsonModelMapper(DataConversionErrorHandling errorHandling = DataConversionErrorHandling.UseDefaultValue)
			: this(type => type.Name.EndsWith("Model"), new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat }, errorHandling)
		{
		}

		public bool CanMapType(HttpContextBase context, Type parameterType)
		{
			context.ThrowIfNull("context");
			parameterType.ThrowIfNull("parameterType");

			return context.Request.ContentType == "application/json" && _parameterTypeMatchDelegate(parameterType);
		}

		public MapResult Map(HttpContextBase context, Type type, MethodInfo method, ParameterInfo parameter)
		{
			context.ThrowIfNull("request");
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");
			parameter.ThrowIfNull("parameter");

			Type parameterType = parameter.ParameterType;
			var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
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