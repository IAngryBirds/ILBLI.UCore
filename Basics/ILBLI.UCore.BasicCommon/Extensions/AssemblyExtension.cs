using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace ILBLI.UCore.BasicCommon
{
    /// <summary>
    /// Assembly Util Class
    /// </summary>
    public static class AssemblyExtension
    {
        /// <summary>
        /// 根据类名创建实例【适用无参构造函数】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">类型名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string type)
        {
            return CreateInstance<T>(type, new object[0]);
        }

        /// <summary>
        /// 根据类型与参数创建实例【适用有参数的构造函数】
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">类型名</param>
        /// <param name="parameters">参数信息</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string type, object[] parameters)
        {
            Type instanceType = null;
            var result = default(T);

            instanceType = Type.GetType(type, true);

            if (instanceType == null)
                throw new Exception(string.Format("The type '{0}' was not found!", type));

            object instance = Activator.CreateInstance(instanceType, parameters);
            result = (T)instance;
            return result;
        }

        /// <summary>
        /// 按全名获取类型，还返回匹配的泛型类型，而不检查名称中的泛型类型参数
        /// </summary>
        /// <param name="fullTypeName">类型的全名.</param>
        /// <param name="throwOnError">是否需要将可能产生的异常抛出</param>
        /// <param name="ignoreCase">是否忽略类型的大小写比较</param>
        /// <returns></returns> 
        public static Type GetType(string fullTypeName, bool throwOnError, bool ignoreCase)
        {
            var targetType = Type.GetType(fullTypeName, false, ignoreCase);

            if (targetType != null)
                return targetType;

            var names = fullTypeName.Split(',');
            var assemblyName = names[1].Trim();

            try
            {
                //加载程序集
                var assembly = Assembly.Load(assemblyName);

                var typeNamePrefix = names[0].Trim() + "`";
                //获取在此程序集中定义的在程序集外部可见的公共类型 //IsGenericType判断是否是泛型类型
                var matchedTypes = assembly.GetExportedTypes()
                                           .Where(t => t.IsGenericType && t.FullName.StartsWith(typeNamePrefix, ignoreCase, CultureInfo.InvariantCulture)).ToArray();

                if (matchedTypes.Length != 1)
                    return null;

                return matchedTypes[0];
            }
            catch (Exception e)
            {
                if (throwOnError)
                    throw e;

                return null;
            }
        }
        
        /// <summary>
        ///从程序集获取实现类型
        /// </summary>
        /// <typeparam name="TBaseType">The type of the base type.</typeparam>
        /// <param name="assembly">要查找的程序集</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetImplementTypes<TBaseType>(this Assembly assembly)
        {
            return assembly.GetExportedTypes().Where(t =>
                t.IsSubclassOf(typeof(TBaseType)) && t.IsClass && !t.IsAbstract);
        }

        /// <summary>
        /// 按某个接口获取程序集中实现的对象
        /// </summary>
        /// <typeparam name="TBaseInterface">要查找的接口</typeparam>
        /// <param name="assembly">要查找的程序集</param>
        /// <returns></returns>
        public static IEnumerable<TBaseInterface> GetImplementedObjectsByInterface<TBaseInterface>(this Assembly assembly)
            where TBaseInterface : class
        {
            return GetImplementedObjectsByInterface<TBaseInterface>(assembly, typeof(TBaseInterface));
        }

        /// <summary>
        /// 按某个接口（继承自目标类）获取程序集中实现的对象
        /// </summary>
        /// <typeparam name="TBaseInterface">要查找的接口</typeparam>
        /// <param name="assembly">要查找的程序集</param>
        /// <param name="targetType">继承自的目标类</param>
        /// <returns></returns>
        public static IEnumerable<TBaseInterface> GetImplementedObjectsByInterface<TBaseInterface>(this Assembly assembly, Type targetType)
            where TBaseInterface : class
        {
            Type[] arrType = assembly.GetExportedTypes();

            var result = new List<TBaseInterface>();

            for (int i = 0; i < arrType.Length; i++)
            {
                var currentImplementType = arrType[i];

                if (currentImplementType.IsAbstract)
                    continue;

                if (!targetType.IsAssignableFrom(currentImplementType))
                    continue;

                result.Add((TBaseInterface)Activator.CreateInstance(currentImplementType));
            }

            return result;
        }

        /// <summary>
        /// 以二进制格式克隆对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">目标类</param>
        /// <returns></returns>
        public static T BinaryClone<T>(this T target)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, target);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 将一个对象的属性复制到另一个对象.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static T CopyPropertiesTo<T>(this T source, T target)
        {
            return source.CopyPropertiesTo(p => true, target);
        }

        /// <summary>
        /// 将一个对象的属性复制到另一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predict">The properties predict.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static T CopyPropertiesTo<T>(this T source, Predicate<PropertyInfo> predict, T target)
        {
            PropertyInfo[] properties = source.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            Dictionary<string, PropertyInfo> sourcePropertiesDict = properties.ToDictionary(p => p.Name);

            PropertyInfo[] targetProperties = target.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
                .Where(p => predict(p)).ToArray();

            for (int i = 0; i < targetProperties.Length; i++)
            {
                var p = targetProperties[i];
                PropertyInfo sourceProperty;

                if (sourcePropertiesDict.TryGetValue(p.Name, out sourceProperty))
                {
                    if (sourceProperty.PropertyType != p.PropertyType)
                        continue;

                    if (!sourceProperty.PropertyType.IsSerializable)
                        continue;

                    p.SetValue(target, sourceProperty.GetValue(source, null), null);
                }
            }

            return target;
        }



        /// <summary>
        /// 从字符串获取程序集
        /// </summary>
        /// <param name="assemblyDef">程序集字符串【，/；作为分隔符】</param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssembliesFromString(string assemblyDef)
        {
            return GetAssembliesFromStrings(assemblyDef.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// 从字符串集合获取程序集
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssembliesFromStrings(string[] assemblies)
        {
            List<Assembly> result = new List<Assembly>(assemblies.Length);

            foreach (var a in assemblies)
            {
                result.Add(Assembly.Load(a));
            }

            return result;
        }
    }
}
