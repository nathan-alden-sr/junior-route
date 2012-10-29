using System;

namespace Junior.Route.AutoRouting.IdMappers
{
	public class IdResult
	{
		private readonly Guid? _id;
		private readonly IdResultType _resultType;

		private IdResult(IdResultType resultType, Guid? id)
		{
			_resultType = resultType;
			_id = id;
		}

		public IdResultType ResultType
		{
			get
			{
				return _resultType;
			}
		}

		public Guid? Id
		{
			get
			{
				return _id;
			}
		}

		public static IdResult IdMapped(Guid id)
		{
			return new IdResult(IdResultType.IdMapped, id);
		}

		public static IdResult IdNotMapped()
		{
			return new IdResult(IdResultType.IdNotMapped, null);
		}
	}
}