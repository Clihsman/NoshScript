using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoshScript.Nosh.Native.Condicionales
{
    class Try
    {
        private Script script;
        private Script m_catch;
        private Script m_finally;

        public Try(Script script,Script m_catch,Script m_finally)
        {
            this.script = script;
            this.m_catch = m_catch;
            this.m_finally = m_finally;
        }

        public object execute()
        {
            object result = null;

            try
            {
                result = script.executeNoTry();
            }
            catch (Exception ex)
            {
                if (m_catch != null)
                {
                    if (m_catch.argsCount() == 1)
                    {
                        m_catch.setIn(0, ex.ToString());
                        result = m_catch.execute();
                    }
                    else
                        result = m_catch.execute();
                }
            }
            finally {
                if(m_finally != null)
                    result = m_finally.execute();
            }

            cleanUp();
            return result;
        }

        private void cleanUp() {
            if (script != null)
                script.finish();

            if (m_catch != null)
                m_catch.finish();

            if (m_finally != null)
                m_finally.finish();
        }
    }
}
