using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Urb.Filesystem {
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

        public int NumOfProcessedItems  { get { throw new NotImplementedException(); } }
        public int NumOfPendingItems    { get { throw new NotImplementedException(); } }

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
            var assetPath = Folders.GetDirectory("Assets");
            var dir = new DirectoryInfo(  assetPath );
            var files = dir.EnumerateFiles("*.*", searchSubfoldersAlso ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var file in files) {
                EnqueueFile(file.FullName);
            }
        }

        public void Execute() {
            System.Threading.ThreadPool.QueueUserWorkItem(ThreadingWrapper);
        }

        void ThreadingWrapper(object obj) {
            var a = obj;
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
