using System;
using System.Reflection;


namespace iSpyApplication.Controls
{
	public class Reflector
	{
		#region variables

	    readonly string _mNs;
	    readonly Assembly _mAsmb;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ns">The namespace containing types to be used</param>
		public Reflector(string ns)
			: this(ns, ns)
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="an">A specific assembly name (used if the assembly name does not tie exactly with the namespace)</param>
		/// <param name="ns">The namespace containing types to be used</param>
		public Reflector(string an, string ns)
		{
			_mNs = ns;
			_mAsmb = null;
			foreach (AssemblyName aN in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
			{
				if (aN.FullName.StartsWith(an))
				{
					_mAsmb = Assembly.Load(aN);
					break;
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Return a Type instance for a type 'typeName'
		/// </summary>
		/// <param name="typeName">The name of the type</param>
		/// <returns>A type instance</returns>
		public Type GetType(string typeName)
		{
			Type type = null;
			string[] names = typeName.Split('.');

			if (names.Length > 0)
				type = _mAsmb.GetType(_mNs + "." + names[0]);

			for (int i = 1; i < names.Length; ++i)
			{
			    type = type?.GetNestedType(names[i], BindingFlags.NonPublic);
			}
		    return type;
		}

		/// <summary>
		/// Create a new object of a named type passing along any params
		/// </summary>
		/// <param name="name">The name of the type to create</param>
		/// <param name="parameters"></param>
		/// <returns>An instantiated type</returns>
		public object New(string name, params object[] parameters)
		{
			Type type = GetType(name);

			ConstructorInfo[] ctorInfos = type.GetConstructors();
			foreach (ConstructorInfo ci in ctorInfos) {
				try {
					return ci.Invoke(parameters);
				}
				catch
				{
				    // ignored
				}
			}

			return null;
		}

		/// <summary>
		/// Calls method 'func' on object 'obj' passing parameters 'parameters'
		/// </summary>
		/// <param name="obj">The object on which to excute function 'func'</param>
		/// <param name="func">The function to execute</param>
		/// <param name="parameters">The parameters to pass to function 'func'</param>
		/// <returns>The result of the function invocation</returns>
		public object Call(object obj, string func, params object[] parameters)
		{
			return Call2(obj, func, parameters);
		}

		/// <summary>
		/// Calls method 'func' on object 'obj' passing parameters 'parameters'
		/// </summary>
		/// <param name="obj">The object on which to excute function 'func'</param>
		/// <param name="func">The function to execute</param>
		/// <param name="parameters">The parameters to pass to function 'func'</param>
		/// <returns>The result of the function invocation</returns>
		public object Call2(object obj, string func, object[] parameters)
		{
			return CallAs2(obj.GetType(), obj, func, parameters);
		}

		/// <summary>
		/// Calls method 'func' on object 'obj' which is of type 'type' passing parameters 'parameters'
		/// </summary>
		/// <param name="type">The type of 'obj'</param>
		/// <param name="obj">The object on which to excute function 'func'</param>
		/// <param name="func">The function to execute</param>
		/// <param name="parameters">The parameters to pass to function 'func'</param>
		/// <returns>The result of the function invocation</returns>
		public object CallAs(Type type, object obj, string func, params object[] parameters)
		{
			return CallAs2(type, obj, func, parameters);
		}

		/// <summary>
		/// Calls method 'func' on object 'obj' which is of type 'type' passing parameters 'parameters'
		/// </summary>
		/// <param name="type">The type of 'obj'</param>
		/// <param name="obj">The object on which to excute function 'func'</param>
		/// <param name="func">The function to execute</param>
		/// <param name="parameters">The parameters to pass to function 'func'</param>
		/// <returns>The result of the function invocation</returns>
		public object CallAs2(Type type, object obj, string func, object[] parameters) {
			MethodInfo methInfo = type.GetMethod(func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			return methInfo.Invoke(obj, parameters);
		}

		/// <summary>
		/// Returns the value of property 'prop' of object 'obj'
		/// </summary>
		/// <param name="obj">The object containing 'prop'</param>
		/// <param name="prop">The property name</param>
		/// <returns>The property value</returns>
		public object Get(object obj, string prop)
		{
			return GetAs(obj.GetType(), obj, prop);
		}

		/// <summary>
		/// Returns the value of property 'prop' of object 'obj' which has type 'type'
		/// </summary>
		/// <param name="type">The type of 'obj'</param>
		/// <param name="obj">The object containing 'prop'</param>
		/// <param name="prop">The property name</param>
		/// <returns>The property value</returns>
		public object GetAs(Type type, object obj, string prop) {
			PropertyInfo propInfo = type.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			return propInfo.GetValue(obj, null);
		}

		/// <summary>
		/// Returns an enum value
		/// </summary>
		/// <param name="typeName">The name of enum type</param>
		/// <param name="name">The name of the value</param>
		/// <returns>The enum value</returns>
		public object GetEnum(string typeName, string name) {
			Type type = GetType(typeName);
			FieldInfo fieldInfo = type.GetField(name);
			return fieldInfo.GetValue(null);
		}

		#endregion

	}
}
