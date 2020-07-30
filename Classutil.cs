using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Tomflection
{
    public static class Classutil
    {
        
        public static bool IsTypeOf(this Type desc, Type ancestor)
        {
            if (!desc.IsClass)
                return false;

            if ( ancestor == typeof(object))
                return true;
            
            Type baseType = desc;



            while (baseType != ancestor)
            {
                if ( baseType == null ||  baseType == typeof(object) )
                    return false;

                baseType = baseType.BaseType;
            }

            return true;
        }


        public static IEnumerable<TypeN_Attr<AT>> 
            getTypeN_Attrs <AT, CLASS_T> (
            this Assembly assm, string MatchNamespace)
        {
            var els = from t in assm.GetTypes()
                      where t.IsClass
                      && t.Namespace != null
                      && t.Namespace == MatchNamespace
                      && t.IsTypeOf(typeof(CLASS_T))
                      select t;

            var v = from t in els
                    let a = t.GetCustomAttributes(typeof(AT), false).FirstOrDefault()
                    where a != null
                    select new TypeN_Attr<AT>(t, (AT) a );

            return v;
        }


        /// <summary>
        /// get the calling method /property name  by reflection
        /// name with get_ / set_ prefix for properties 
        /// 
        /// use the name as key to verify existence of entry in PropStore dict
        /// make sure non nullable instance of type T object in the dict
        /// ensuring by adding new instance from invoking TargetGetter
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="PropStore"></param>
        /// <param name="TargetGetter"></param>
        /// <returns></returns>
        public static T readProp_obj<T>(this IPropertyStorable PropStore, 
            Func< T > TargetGetter) where T:class
        {
            StackTrace st = new StackTrace();
            string callingName = st.GetFrame(1).GetMethod().Name;

            T targetObj;
            var storage = PropStore.PropertyStorage;

            if ( storage.ContainsKey(callingName))
            {
                targetObj = storage[callingName] as T;
                if (targetObj == null)
                {
                    targetObj = TargetGetter();
                    storage[callingName] = targetObj;
                }
            }
            else
            {
                targetObj = TargetGetter();
                storage.Add(callingName, targetObj);
            }
            return targetObj;
        
        }


        public static T readProp_value<T>(this IPropertyStorable PropStore,
            Func<T> TargetGetter) where T : struct
        {
            StackTrace st = new StackTrace();
            string callingName = st.GetFrame(1).GetMethod().Name;

            T targetObj;
            var storage = PropStore.PropertyStorage;

            if (storage.ContainsKey(callingName))
            {
                targetObj = (T)storage[callingName] ;
            }
            else
            {
                targetObj = TargetGetter();
                storage.Add(callingName, targetObj);
            }
            return targetObj;

        }

    }
}
