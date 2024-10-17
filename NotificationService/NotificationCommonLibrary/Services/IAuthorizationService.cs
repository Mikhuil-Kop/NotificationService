namespace NotificationCommonLibrary.Services
{
    /// <summary>
    /// Интерфейс сервиса авторизации - это получения права доступа к чему-либо.
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Определяет, какой пользователь авторизован под токеном.
        /// Если токен не активен или не существует, возвращает ошибку.
        /// </summary>
        public Task<int> GetAuthorisedUserIDAsync(string token);
    }
}
