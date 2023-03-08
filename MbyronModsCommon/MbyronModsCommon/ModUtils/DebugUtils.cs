using System;
using System.Diagnostics;
using System.Text;

namespace MbyronModsCommon {
    public static class DebugUtils {
        public static void TimeCalculater(Action action, string tag = "", int loop = 1) {
            new TimeCalculater().AddMethod(action).InvokeMethod(out string time, loop);
            ModLogger.ModLog(tag + time);
        }

        public static void StackTrace(int frame = 1) {
            StackTrace stackTrace = new();
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"Frame = {frame}, ");
            Type type = stackTrace.GetFrame(frame).GetMethod().DeclaringType;
            string method = stackTrace.GetFrame(frame).GetMethod().ToString();
            if (type != null) {
                stringBuilder.Append($"class: {type}  ||  ");
            }
            if (method != null) {
                stringBuilder.Append($"method: {method}");
            }
            ModLogger.ModLog($"{stringBuilder}");
        }


    }
    public sealed class TimeCalculater {
        private Action action;
        public TimeCalculater InvokeMethod(out string time, int loop) {
            Stopwatch sw = new();
            sw.Start();
            for (int i = 0; i < loop; i++) {
                action.Invoke();
            }
            sw.Stop();
            time = $"{sw.Elapsed.TotalMilliseconds * 1000:n3}μs";
            return this;
        }

        public TimeCalculater AddMethod(Action method) {
            action += method;
            return this;
        }

    }

}



