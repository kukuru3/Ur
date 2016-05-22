using System;
using System.Collections.Generic;

namespace Ur.Filesystem {
    public enum LoadStates {
        Pending,
        Started,
        Loading,
        Completed,
        Failure
    }

    public class UrLoaderAttribute : Attribute {
        public string[] Extensions { get; }
        public UrLoaderAttribute(string extensions) {
            Extensions = extensions.Split('|');
        }       
    }

    public class LoaderEventArgs {
        public Loader      Sender   { get; }
        public LoadStates  State    { get; }
        public float       Progress { get; }
        public string      Message  { get; }

        public LoaderEventArgs(Loader sender, LoadStates state, float progress = 0f, string message ="") {
            Sender      = sender;
            State       = state;
            Progress    = progress;
            Message     = message;
        }

    }

    public delegate void LoaderEvent(LoaderEventArgs e);

    public class LoaderException : Exception {
        public Loader Loader { get; }
        public LoaderException(Loader loader, string msg) : base(msg) {
            Loader = loader;
        }
    }
}
