using System;

namespace PlateGetter.Helpers
{
	public sealed class Country
	{
		/// <summary>Возвращает полное наименование страны.</summary>
		public string FullName { get; private set; }

		/// <summary>Возвращает инициалы страны на номерном знаке.</summary>
		public string PlateName { get; private set; }

		/// <summary>
		/// Создаёт страну.
		/// </summary>
		/// <param name="fullName">Полное наименование страны.</param>
		/// <param name="plateName">Инициалы страны на номерном знаке.</param>
		public Country(string fullName, string plateName)
		{
			FullName = fullName;

			PlateName = plateName;
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}