using System;
using System.Collections.Generic;
using System.IO;

namespace Ur.Filesystem {
    static public class Folders {

        static private HashSet<string> folderCorrections = new HashSet<string>(new[] {
            "Release", "Debug", "Debug32", "Release32", "Debug64", "Release64", "Build", "x86", "x64", "bin", "netcoreapp3.0", "netcoreapp3.1",
        });

        static private void DoPathsCorrection(string[] additions = null) {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (folderCorrections.Contains(dir.Name)) {
                dir = dir.Parent;
            }
            Directory.SetCurrentDirectory(dir.FullName);
        }


        /// <param name="relativeOrAbsolutePath"></param>
        static public string GetDirectory(string relativeOrAbsolutePath) {
            string path;
            if (Path.IsPathRooted(relativeOrAbsolutePath)) {
                path = relativeOrAbsolutePath;
            } else {
                DoPathsCorrection();
                path = Path.Combine(Directory.GetCurrentDirectory(), relativeOrAbsolutePath);
            }

            var di = new DirectoryInfo(path);
            if (!di.Exists) throw new DirectoryNotFoundException(relativeOrAbsolutePath);
            return path;
        }

        static public string GetAppDataDirectory() {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        }

    }
}
