using NoshScript;
using NoshScript.Nosh.Collections;
using NoshScript.Nosh.Collections.NoshPackage;
using System;

namespace array
{
    public class array
    {
        public static void main()
        {
            Package array = null;
            PackageManager manager = new PackageManager();
            loadArray(manager);
            array = manager.Create("array");
            manager.Dispose();
            PackageList.AddPackage(array);
        }

        public static void loadArray(PackageManager manager) {
            manager.AddFuntion(new Funtion("array", new Func<int, object[]>(delegate (int length)
            {
                return new object[length];
            })));

            manager.AddFuntion(new Funtion("array", new Action<object[],object, int>(delegate (object[] array,object value,int index)
            {
                array[index] = value;
            })));

            manager.AddFuntion(new Funtion("array", new Func<object[], int,object>(delegate (object[] array, int index)
            {
               return array[index];
            })));

            manager.AddFuntion(new Funtion("length", new Func<object[], int>(delegate (object[] array)
            {
                return array.Length;
            })));
        }
    }
}
