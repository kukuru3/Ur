using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Ur.Filesystem {
    public class FileFinder {

        public string Path { get; }
        public FileFinder(string path) {
            Path = path;
        }

        public IEnumerable<string> FindFiles(string pattern, bool includeSubfolders = true) {
            var di = new DirectoryInfo(Path);
            return di.EnumerateFiles(pattern, includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Select(fi => fi.FullName);
        }

    }
}
