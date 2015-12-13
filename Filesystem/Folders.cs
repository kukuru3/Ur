using System;
using System.Collections.Generic;
using System.IO;

namespace Urb.Filesystem {
    static public class Folders {

        static private HashSet<string> folderCorrections = new HashSet<string>(new[] {
            "Release", "Debug", "Build", "x86", "x64", "bin"
        });

        static private void DoPathsCorrection() {
            var dir =  new DirectoryInfo( Directory.GetCurrentDirectory());
            while( folderCorrections.Contains(dir.Name) ) {
                dir = dir.Parent;
            }                
            Directory.SetCurrentDirectory(dir.FullName);
        }

        static public string GetDirectory(string offsetFromRoot) {
            DoPathsCorrection();
            var path = Path.Combine( Directory.GetCurrentDirectory(), offsetFromRoot);
            var di = new DirectoryInfo(path);
            if (!di.Exists) throw new DirectoryNotFoundException(offsetFromRoot);
            return path;
        }

    }
}
