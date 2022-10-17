namespace VerificationsService.Models.ResponseModel
{
    public class CommonMessage
    {
        public static string PassValidData = "Some data are missed in the request. Pass valid data.";
        public static string TokenVerified = "Token verify successfully";
        public static string TokenCreated = "Token created successfully";
        public static string ChallengedIdCreated = "ChallengedId created successfully";
        public static string IdentityNotFound = "Identity not found.";
        public static string CodeAlreadyUsed = "Code already used!";
        public static string CodeInvalid = "Code is Invalid.";
        public static string MissingPhoneNumber = "Missing Phone Number.";
        public static string LimitExceedException = "Maximum number of atempts reached!!";
        public static string MissingPassword = "Missing password.";
        public static string IncorrectOTP = "Incorrect OTP!!.";
        public static string IncorrectPassword = "Incorrect password.";
        public static string LoginSuccess = "Login successfully.";
        public static string ExceptionMessage = "Something went wrong. Error Message - ";
        public static string ApplicationNotFound = "Applications not found.";
        public static string ApplicationDelete = "Applications deleted successfully.";
        public static string ApplicationRetrived = "Applications retrived successfully.";
        public static string ApplicationInsert = "Applications inserted successfully.";
        public static string ApplicationUpdate = "Applications updated successfully.";
        public static string ApplicationExists = " Applications already exists.";
        public static string PrivilegeAssociatedWithRole = "Privileges associated with role.";
        public static string PrivilegeAssociatedWithUserRole = "Privileges associated with user role.";
        public static string ApplicationAssociatedWithRole = "Application associated with role.";
        public static string ApplicationAssociatedWithUserRole = "Application associated with user role.";
        public static string UnknownApplication = "No Application is specified in request";
        public static string TokenDataNull = "Token data is null.";
        public static string Unauthorized = "You are unauthorized to perform this operation.";
        public static string Forbidden = "Forbidden";
        public static string InvalidData = "Pass valid data.";
        public static string CodeExpired = "Code has expired.";
        public static string TokenExpired = "Token is expired.";
        public static string TokenAlreadyRevoked = "This token is already revoked.";
        public static string InvalidIdentityToken = "This token does not belong to a valid identity.";
        public static string OfficerNotFound = "Officer not found.";
        public static string RefreshTokenRevoked = "Refresh token revoked successfully.";
        public static string AccessTokenRevoked = "Access token revoked successfully.";
        public static string ConnectionFailure = "Request cannot be executed: unable to establish connection with the targeted machine.";
    }
}