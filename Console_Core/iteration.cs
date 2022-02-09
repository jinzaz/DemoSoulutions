using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Console_Core
{
    public class iteration:IEnumerable,IEnumerator
    {
        #region IEnumerable成员

        public IEnumerator GetEnumerator()
        {
            return this;
        }
        #endregion
        #region IEnumerator成员
        int curIndex = -1;
        public void Reset()
        {
            curIndex = -1;
        }

        public object Current
        {
            get {
                if (curIndex < this.allFiles.Count)
                {
                    return this.allFiles[curIndex];
                }
                throw new Exception("f");
            }
        }

        public bool MoveNext()
        {
            if (this.allFiles.Count == 0)
                return false;
            curIndex += 1;
            if (curIndex == this.allFiles.Count)
            {
                return false;
            }
            return true;
        }
        #endregion 

        private System.Collections.Specialized.StringCollection allFiles = null;
        public iteration GetFiles(string dirPath)
        {
            allFiles = new System.Collections.Specialized.StringCollection();
            allFiles.Clear();
            foreach (var f in System.IO.Directory.GetFiles(dirPath))
            {
                allFiles.Add(f);
            }
            return this;
        }

        public IEnumerable<String> yieldGetFiles(string dir)
        {
            foreach (string file in System.IO.Directory.GetFiles(dir))
            {
                yield return file;
            }
            foreach (string dir1 in System.IO.Directory.GetDirectories(dir))
            {
                foreach (string ff in System.IO.Directory.GetFiles(dir1))
                {
                    yield return ff;
                }
            }
        }
    }
}
