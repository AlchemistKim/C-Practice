using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
	class Program
	{
		static void Main(string[] args)
		{
			Reflection reflection = new Reflection();
			SetDefaultName(reflection);
			GetFields<Reflection>(reflection);
			int a = Get<int>(3);
			Func<double, double> square = x => x * x;
			double test = square(5);
			var b = square(3);

			string[] filename_array = System.IO.Directory.GetFiles("test", "add_skilllist*.json");
			List<string> filename_list = new List<string>(filename_array);
		}

		static void SetDefaultName(object myObject)
		{
			MethodInfo mi = myObject.GetType().GetMethod("MyMethod");
			PropertyInfo pi = myObject.GetType().GetProperty("name");

			if (mi != null)
			{
				// 만약 메서드가 있으면, 호출
				object[] obj = new object[2];
				obj[0] = 3;
				obj[1] = 4;
				mi.Invoke(myObject, obj);
			}
			else
			{
				Console.WriteLine(myObject.GetType().Name +
				 ": MyMethod not found");
			}
		}

		static public void GetFields<T>(T obj)
		{
			FieldInfo[] fields = typeof(T).GetFields();
			foreach (var field in fields)
			{
				Console.WriteLine(string.Format("{0}.{1} = {2}",
										  typeof(T).Name, field.Name, field.GetValue(obj)));
			}
		}


		static public T Get<T>(T a)
		{
			return a;
		}
	}

	public static class Scale<T>
	{
		public static Func<T, double, T> Do { get; private set; }

		static Scale()
		{
			var par1 = Expression.Parameter(typeof(T));
			var par2 = Expression.Parameter(typeof(double));

			try
			{
				Do = Expression
					.Lambda<Func<T, double, T>>(
						Expression.Multiply(par1, par2),
						par1, par2)
					.Compile();
			}
			catch
			{
				Do = Expression
					.Lambda<Func<T, double, T>>(
						Expression.Convert(
							Expression.Multiply(
								Expression.Convert(par1, typeof(double)),
								par2),
							typeof(T)),
						par1, par2)
					.Compile();
			}
		}
	}
}
