using System;
using System.Diagnostics;
using System.Text;

namespace MbyronModsCommon {
    public static class DebugUtils {
        public static void TimeCalculater(Action action, int loop = 1, string tag = "") {
            new TimeCalculater().AddMethod(action).InvokeMethod(out long time, loop);
            ModLogger.ModLog(tag + time.ToString());
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
        public TimeCalculater InvokeMethod(out long timer, int loop) {
            Stopwatch sw = new();
            sw.Start();
            for (int i = 0; i < loop; i++) {
                action.Invoke();
            }
            sw.Stop();
            timer = sw.ElapsedMilliseconds;
            return this;
        }

        public TimeCalculater AddMethod(Action method) {
            action += method;
            return this;
        }

    }

}
