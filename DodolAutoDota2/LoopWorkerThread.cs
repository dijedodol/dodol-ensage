using System;
using System.Threading;

namespace DodolAutoDota2
{
    public class LoopWorkerThread
    {
        internal delegate void Loop();

        private Object mutex = new object();
        private bool _virgin = true;
        private bool _running = false;
        private Thread _thread;

        internal LoopWorkerThread(Loop loop)
        {
            _thread = new Thread(() =>
            {
                while (_running)
                {
                    loop();
                }
            });
        }

        public void Start()
        {
            lock (mutex)
            {
                if (!_virgin || _running) return;

                _virgin = false;
                _running = true;
                _thread.Start();
            }
        }

        public void Stop()
        {
            lock (mutex)
            {
                _running = false;
            }
        }
    }
}