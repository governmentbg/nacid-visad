namespace VisaD.Application.DomainValidation.Enums
{
    public enum UserErrorCode
    {
        User_EmailTaken = 201,
        User_InvalidCredentials = 202,
        User_UserAlreadyUnlocked = 203,
        User_ActivationTokenAlreadyUsed = 204,
        User_ActivationTokenExpired = 205,
        User_ChangePasswordNewPasswordMismatch = 206,
        User_ChangePasswordOldPasswordMismatch = 207,
        User_CannotRestoreUserPassword = 208,
        User_PasswordRecoveryTokenExpired = 209,
        User_NotFound = 210,
        User_TokenExpired = 211,
        User_UserDeactivatedOrLocked = 212
    }
}
