using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
namespace Esim7.UNity
{
    public class Object_To_T
    {
        /// <summary>
        /// 将object对象转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="asObject">object对象</param>
        /// <returns></returns>
        public static  T ConvertObject<T>(object asObject) where T : new()
        {
            //创建实体对象实例
            var t = Activator.CreateInstance<T>();
            if (asObject != null)
            {
                Type type = asObject.GetType();
                //遍历实体对象属性
                foreach (var info in typeof(T).GetProperties())
                {
                    object obj = null;
                    //取得object对象中此属性的值
                    var val = type.GetProperty(info.Name)?.GetValue(asObject);
                    if (val != null)
                    {
                        //非泛型
                        if (!info.PropertyType.IsGenericType)
                            obj = Convert.ChangeType(val, info.PropertyType);
                        else//泛型Nullable<>
                        {
                            Type genericTypeDefinition = info.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                obj = Convert.ChangeType(val, Nullable.GetUnderlyingType(info.PropertyType));
                            }
                            else
                            {
                                obj = Convert.ChangeType(val, info.PropertyType);
                            }
                        }
                        info.SetValue(t, obj, null);
                    }
                }
            }
            return t;
        }




        // <summary>
        /// 将object对象转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="asObject">object对象</param>
        /// <returns></returns>
        //public static T ConvertObject2<T>(object asObject) where T : new()
        //{
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    //将object对象转换为json字符

        //    string json = json.Serialize(asObject);
        //    //将json字符转换为实体对象
        //  T t = serializer.Deserialize<T>(json);
        //    return t;
        //}


        /// <summary>
            /// 解析JSON字符串生成对象实体
            /// </summary>
            /// <typeparam name="T">对象类型</typeparam>
            /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
            /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {


            try
            {
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(json);
                object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));


                T t = o as T;
                return t;
            }
            catch (Exception ex)
            {


                return null;
            }


        }



    }
}