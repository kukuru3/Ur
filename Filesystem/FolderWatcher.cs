using System;
using System.Collections.Generic;
using System.IO;

namespace Urb.Filesystem {
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class FolderWatcher
    {
        // interface with the class through these properties : 
        public FolderWatcher(string folderPath, string fileMask, bool watchSubfolders, Action<string> callback)
        {
            var di = new DirectoryInfo(folderPath);
            if (!di.Exists) throw new ArgumentException("Folder path " + folderPath + " you wish to watch is not a valid folder. Whoops. " );
            
            var fsw = new FileSystemWatcher(folderPath);
            fsw.Filter = fileMask;
            fsw.Path = folderPath;
            fsw.IncludeSubdirectories = true;
            fsw.NotifyFilter = NotifyFilters.LastWrite;
            fsw.Changed += delegate(object sender, FileSystemEventArgs args) {
                if (args.ChangeType == WatcherChangeTypes.Changed) {
                    callback(folderPath);
                }
            };
            fsw.EnableRaisingEvents = true; // this will keep the watcher from being collected.
        }
        
    }
}
