namespace NotificationCommonLibrary.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Аутентифицирует пользователя.
        /// Если аутентификация успешна - возвращает токен. Если нет - возвращает ошибку.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Токен авторизации.</returns>
        public Task<string> AuthenticateAsync(string login, string password);
    }
}
