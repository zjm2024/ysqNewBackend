using System;
using System.Collections.Generic;
using System.Text;

namespace SPLibrary.CoreFramework.BO
{
    public class SingletonProvider<T> where T : new()
    {
        SingletonProvider() { }

        public static T Instance
        {
            get { return SingletonCreator.instance; }
        }

        private class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly T instance = new T();
        }
    }

}
