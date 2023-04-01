using System;
using System.Diagnostics;

namespace MbyronModsCommon {
    public static class DebugUtils {
        public static void TimeCalculater(Action action, string tag = "", int loop = 1) {
            new TimeCalculater().AddMethod(action).InvokeMethod(out string time, loop);
            ExternalLogger.Log(tag + time);
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



