namespace Dotspec
{
    public class PreconditionSpec<TSubject> : SpecBase<TSubject>
        where TSubject : class
    {
        public PreconditionSpec(string scenario) : base(scenario) { }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public BehaviourSpec<TSubject, TVal> Given<TVal>(TVal val)
        {
            return new BehaviourSpec<TSubject, TVal>(Scenario, val);
        }
    }
}
