using System.Threading.Tasks;
using TreeSharp;

namespace EatMe.Logic
{
    /// <summary>
    /// Base Implementation of the ILogic Interface
    /// </summary>
    public abstract class BaseLogic : ILogic
    {
        private Composite _execute;
        /// <summary>
        /// Composite that gets hooked onto the main TreeRoot
        /// </summary>
        public Composite Execute
        {
            get
            {
                return _execute ?? (_execute = new Decorator(ctx => NeedToStart, new ActionRunCoroutine(x => Start())));
            }
        }
        /// <summary>
        /// Boolean that tells the main TreeRoot if it needs to run down this branch of Logic
        /// </summary>
        protected abstract bool NeedToStart { get; }
        /// <summary>
        /// The logic that gets run when NeedToStart is true
        /// </summary>
        protected abstract Task<bool> Start();
    }
}