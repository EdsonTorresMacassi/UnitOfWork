namespace Repository.Interfaces.Actions
{
    public interface IRemoveRepository<T>
    {
        void Delete(T id);
    }
}
