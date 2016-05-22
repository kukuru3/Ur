using System;
using System.Collections.Generic;

namespace Ur.Filesystem {
    
    public abstract class Loader {

        #region Fields
        
        public string       Path  { get; }
        public LoadStates   State { get; private set; }
        public int          Bulk  { get; protected set; }
        
        public float        Progress { get; private set; }
        #endregion
        
        protected void SetProgress(float newValue) {
            Progress = newValue;
            InvokeUpdate("progress : " + (100 * newValue).ToString() + "%");
        }

        protected void UpdateState(LoadStates newState) {
            State = newState;
            InvokeUpdate("state : " + newState);
        }

        protected void InvokeUpdate(string message = "") {
            Updated?.Invoke(new LoaderEventArgs(this, State, Progress, message));
        }

        public event LoaderEvent Updated;        

        public bool IsCompleted { get { return State >= LoadStates.Completed; } }

        internal void StartLoad() {
            if (State != LoadStates.Pending) throw new LoaderException(this, "StartLoad() can only be called once.");
            Progress = 0f;
            UpdateState(LoadStates.Started);            
            State = LoadStates.Loading;
            Load();            
        }

        public Loader(string path) {
            Path = path;
            Bulk = 100;
            State = LoadStates.Pending;
        }

        protected abstract void Load();

    }
}
