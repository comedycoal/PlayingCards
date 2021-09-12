using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayingCards.Game
{
    class DefaultTimer : ITimer
    {
        private Stopwatch m_watch;
        public long TimeEllapsed => throw new NotImplementedException();

        public DefaultTimer()
        {
            m_watch = new Stopwatch();
        }

        public void Start()
        {
            m_watch.Start();
        }

        public void Stop()
        {
            m_watch.Stop();
        }

        public void Reset()
        {
            m_watch.Reset();
        }
    }
}
