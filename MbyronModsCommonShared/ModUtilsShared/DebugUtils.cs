using System;
using System.Diagnostics;

namespace MbyronModsCommon {
    public sealed class TimeCalculater {
        private Action action;

        public static void DebugTimeCalculater(Action action, int loop = 1, string tag = "") {
            new TimeCalculater().AddMethod(action).InvokeMethod(out long time, loop);
            ModLogger.ModLog(tag + time.ToString());
        }

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
