using UserManagementService.Helper;

namespace UserManagementService.ErrorResponse
{
    public class ErrorResponseModel
    {
        public ResponseCode responseCode { get; set; }
        public string message { get; set; }
        public ErrorResponseModel(ResponseCode responsecode, string message) { 
           responseCode = responsecode;
            message = message;
        }


    }
}
