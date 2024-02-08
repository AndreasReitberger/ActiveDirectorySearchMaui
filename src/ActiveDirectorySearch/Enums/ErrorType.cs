namespace AndreasReitberger.ActiveDirectorySearch.Enums
{
    public enum ErrorType
    {
        UnhandledException,
        RestApiCommunicationError,
        WebSocketError,
        AccessViolation,

        Misc = 99,
    }
}
