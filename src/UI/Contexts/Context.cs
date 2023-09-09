using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UniverseLib.UI.Contexts
{
    public abstract class Context<T> : IDisposable
        where T : Context<T>
    {
        public class Stack : IStack
        {
            private readonly Stack<WeakReference> _stack;

            public Stack()
            {
                _stack = new Stack<WeakReference>();
            }

            public Stack(int capacity)
            {
                _stack = new Stack<WeakReference>(capacity);
            }

            public int Count => _stack.Count;
            public T Peek() => _stack.Peek() as T;

            /// <summary>
            /// Returns null if the stack is empty instead of throwing an exception.
            /// </summary>
            public T SafePeek() => Count > 0 ? _stack.Peek().Target as T : null;

            WeakReference IStack.Peek() => _stack.Peek();
            WeakReference IStack.Pop() => _stack.Pop();
            void IStack.Push(WeakReference obj) => _stack.Push(obj);
        }

        private interface IStack
        {
            public int Count { get; }

            WeakReference Peek();
            WeakReference Pop();
            void Push(WeakReference obj);
        }

        private bool disposedValue;
        private readonly StackTrace creationStackTrace;
        private readonly IStack contextStack;


        public Context(Stack contextStack)
        {
            creationStackTrace = new StackTrace(2, true);
            this.contextStack = contextStack;
            this.contextStack.Push(new WeakReference(this));
        }

        protected virtual void OnDispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (!disposing)
                {
                    Universe.Logger.LogError(
                        $"The {GetType().Name} was never disposed. " +
                        $"Please use 'using (context) {{ ... }}' to ensure it is properly disposed.\n" +
                        $"{creationStackTrace}"
                    );
                }

                var topContextRef = contextStack.Pop();
                if (!object.ReferenceEquals(topContextRef.Target, this))
                {
                    Universe.Logger.LogError(
                        $"The {GetType().Name} was disposed out-of-order. " +
                        $"Please use 'using (context) {{ ... }}' to ensure it is properly disposed.\n" +
                        $"{creationStackTrace}"
                    );

                    IStack tempStack = new Stack(contextStack.Count + 1);
                    tempStack.Push(topContextRef);
                    while (contextStack.Count > 0)
                    {
                        var context = contextStack.Pop();
                        if (object.ReferenceEquals(context.Target, this))
                            break;
                        tempStack.Push(context);
                    }
                    while (tempStack.Count > 0)
                    {
                        contextStack.Push(tempStack.Pop());
                    }
                }

                OnDispose(disposing);

                disposedValue = true;
            }
        }

        ~Context()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
