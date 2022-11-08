namespace SmsSenderApi.Validators.Interfaces
{
    public interface IAllValidator<T>
    {
        bool ValidateAll(T obj);
    }
}
