using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppDevTest
{
    public class AppDevReflection
    {
        public static void ShowExecutingAssembly()
        {
            //Assembly currentAssembly = Assembly.GetCallingAssembly();
            //Assembly currentAssembly = Assembly.Load("mscorlib.dll");
            Assembly currentAssembly1 = Assembly.ReflectionOnlyLoad("mscorlib.dll");
            Assembly currentAssembly = Assembly.ReflectionOnlyLoadFrom(@"C:\Users\Peter Strong\Dropbox\Projects\SP2010Test\SP2010Core\bin\Debug\sp2010core.dll");
            Console.WriteLine(currentAssembly.FullName);

            Type[] types = currentAssembly.GetTypes();

            for (int i = 0; i < types.Count(); i++)
            {
                Console.WriteLine(types[i].Name);
                Console.WriteLine(types[i].FullName);
                Console.WriteLine(types[i].AssemblyQualifiedName);
                Console.WriteLine(String.Empty);
            }

        }

        public static void ReflectStringBuilder()
        {
            // Create our type
            Type type = typeof (StringBuilder);

            // Create a constructorInfo object that we will use to instantiate the type. Any required parameter types should be passed as a type array
            ConstructorInfo ci = type.GetConstructor(new Type[] {typeof (string)});

            // Call ConstructorInfo.Invoke to instantiate the type. Any required parameters should be passed in as an object array 
            Object sb = ci.Invoke(new object[] {"Hello"});

            // Create a MethodInfo object to create and use a method of the type.  Any required parameter types should be passed as a type array.
            MethodInfo append = type.GetMethod("Append", new Type[] {typeof (string)});

            // Call MethodInfo.invoke to call our method.  Any required parameters should be passed in as an object array
            Object result = append.Invoke(sb, new object[] {"World"});

            Console.WriteLine(result);

            byte[] il = append.GetMethodBody().GetILAsByteArray();

            for (int i=0; i< il.Length; i++)
            {
                Console.Write(il[i]);
            }
        }
    }
}
