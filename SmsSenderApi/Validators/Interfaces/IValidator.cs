namespace SmsSenderApi.Validators.Interfaces
{
    public interface IValidator<T>
    {
        bool Validate(T obj);
    }
}
