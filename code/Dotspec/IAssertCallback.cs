namespace Dotspec
{
    /// <summary>
    /// Provides callback method for the OnAssert event.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public interface IAssertCallback<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Callback method to be invoked when the OnAssert event is raised.
        /// </summary>
        /// <param name="subject"></param>
        void OnAssertCallback(TSubject subject);
    }
}
