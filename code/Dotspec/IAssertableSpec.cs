namespace Dotspec
{
    /// <summary>
    /// Defines the method that can be used to assert test specifications for 
    /// the intended subject <paramref name="TSubject"/>
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    public interface IAssertableSpec<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Asserts all the test specifications for the intended subject 
        /// <paramref name="TSubject"/>.
        /// </summary>
        /// <param name="subject"></param>
        void Assert(TSubject subject);
    }
}
