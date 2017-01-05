
using System;
using System.Collections.Generic;
using System.IO;

namespace Ur.Filesystem {

    /// <summary> Serves to process loading of various assets. Can have individual files added or entire folders scanned.
    /// Then it creates corresponding Loader objects, each of which "loads" a single file sequentially. You can supply external
    /// loader types. </summary>
    public class LoadManager {

        #region Fields
        Dictionary<string, Type> registeredLoaderClasses;
        Queue<Loader> itemsPending;
        List<Loader> itemsCompleted;
        List<Loader> itemsFailed;
        #endregion

        public LoadManager() {
            itemsPending    = new Queue<Loader>();
            itemsCompleted  = new List<Loader>();
            itemsFailed     = new List<Loader>();
            registeredLoaderClasses = new Dictionary<string, Type>();
        }

        public int NumOfProcessedItems  => itemsCompleted.Count;
        public int NumOfPendingItems    => itemsPending.Count;

        public void RegisterLoader(Type classType, params string[] extensions) {
            foreach (var ext in extensions) {
                registeredLoaderClasses[ext] = classType;
            }
        }

        public void EnqueueFile(string path) {
            var fi = new FileInfo(path);
            if (!fi.Exists) fi = new FileInfo(Directory.GetCurrentDirectory() + "\\" + path); // not sure if supported by default???
            if (!fi.Exists) throw new LoaderException(null, "Cannot load nonexistent file: " + path);
            var loader = GetLoaderInstance(fi.FullName);
            if (loader != null) {
                itemsPending.Enqueue(loader);
            }

        }

        public void EnqueueDirectory(string path, bool searchSubfoldersAlso = true) {
            
            var assetPath = Folders.GetDirectory(path);
            var dir = new DirectoryInfo(assetPath);
            
            #if DOTNET_35
            throw new NotImplementedException();
            #else
            var files = dir.EnumerateFiles("*.*", searchSubfoldersAlso ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var file in files) {
                EnqueueFile(file.FullName);
            }
            #endif
        }

        public void Execute() {
            System.Threading.ThreadPool.QueueUserWorkItem(ThreadingWrapper);
        }

        void ThreadingWrapper(object obj) {
            
            while(itemsPending.Count > 0) {
                LoadNext();
                System.Threading.Thread.Sleep(1);
            }
        }

        void LoadNext() {
            lock(itemsPending) { 
                var item = itemsPending.Dequeue();
                item.Updated += FileUpdated;
                item.StartLoad();
            }
        }

        public event LoaderEvent LoadingUpdate;
        public event Action       AllTasksComplete;
        
        private void FileUpdated(LoaderEventArgs e) {            

            switch (e.State) {
                case LoadStates.Completed: itemsCompleted.Add(e.Sender); CheckForCompletion(); break;
                case LoadStates.Failure:   itemsFailed.Add(e.Sender);    CheckForCompletion();break;
            }

            LoadingUpdate?.Invoke(e);
        }

        void CheckForCompletion() {
            lock(itemsPending) { 
                if (itemsPending.Count == 0) AllTasksComplete?.Invoke();
            }
        }
        
        Loader GetLoaderInstance(string path) {
            Type t;
            var ext = Path.GetExtension(path).Substring(1);
            if (registeredLoaderClasses.TryGetValue(ext, out t)) {
                var l = Activator.CreateInstance(t, path) as Loader;
                return l;
            } else {
                return null;
            }
        }

    }
}
