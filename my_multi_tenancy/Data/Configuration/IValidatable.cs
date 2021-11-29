namespace my_multi_tenancy.Data.Configuration
{
    public interface IValidatable
    {
        /// <summary>
        /// The classes should provide validation logic
        /// </summary>
        void Validate();
    }
}
