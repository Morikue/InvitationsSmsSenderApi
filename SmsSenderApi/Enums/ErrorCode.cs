namespace SmsSenderApi.Enums
{
    /// <summary>
    /// Enum для ValidationException
    /// </summary>
    public enum ErrorCode
    {
        NotInternationalFormatPhoneNumber,
        EmptyPhoneNumbers,
        TooMuchPhoneNumbers,
        TooMuchMessagesToday,
        DuplicatedPhoneNumbers,
        EmptyMessage,
        NotGsmAndTransliteratedCyrillic,
        TooLongMessage,
        IternalErrorStatusCode
    }
}
