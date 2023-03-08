using System.Reflection;

namespace MbyronModsCommon {
    public static class UnityHelper {
        public static void SetFieldValue<T>(object obj, string fieldName, BindingFlags bindingFlags, T value) => obj.GetType().GetField(fieldName, bindingFlags).SetValue(obj, value);
        public static void SetFieldValue(object obj, string fieldName, BindingFlags bindingFlags, object value) => obj.GetType().GetField(fieldName, bindingFlags).SetValue(obj, value);

        public static T GetFieldValue<T>(object obj, string fieldName) => (T)(obj.GetType().GetField(fieldName)).GetValue(obj);
        public static object GetFieldValue(object obj, string fieldName) => obj.GetType().GetField(fieldName).GetValue(obj);

        public static MethodInfo GetMethodInfo(object obj, string methodName, BindingFlags bindingFlags) => obj.GetType().GetMethod(methodName, bindingFlags);
    }

    
}
