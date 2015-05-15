using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TypeC
{
    public class ProbingAssemblyDirectory
    {
        private string _directoryName;
        private List<string> _dllNames = new List<string>();
        /// <summary>
        /// Setting the directory name will expand all the DLLs in the directory 
        /// </summary>
        public string DirectoryName
        {
            get { return _directoryName; }
            //do a regex based check
            set 
            {
                _directoryName = value;
                //remove the last backweard slash, if present
                if (_directoryName.LastIndexOf(@"\") == _directoryName.Length - 1)
                {
                    _directoryName = _directoryName.Substring(0, _directoryName.Length - 1);
                }
                //add filter for system directories in future
                LoadDllNames();
            }
        }
        /// <summary>
        /// LoadDllNames() will reset the existing dll collection
        /// </summary>
        public void LoadDllNames()
        {
            if (string.IsNullOrEmpty(_directoryName))
            {
                return; 
            }
            if (!Directory.Exists(_directoryName))
            {
                return; 
            }
            string[] files = Directory.GetFiles(_directoryName);
            _dllNames = new List<string>();
            foreach (var fileName in files)
            {
                FileInfo fi = new FileInfo(fileName);
                if (!fi.Extension.Equals(".dll"))
                {
                    continue;
                }
                _dllNames.Add(fi.Name);
            }

        }

        public List<string> DllNames
        {
            get { return _dllNames; }
            set { _dllNames = value; }
        }
    }
}
