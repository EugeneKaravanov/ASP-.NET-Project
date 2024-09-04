namespace GatewayService.Models
{
    public class OperationStatusDto
    {
        public StatusDto Status { get; set; }

        public string Message { get; set; }
    }

    public enum StatusDto
    {
        SUCCESS,
        FAILURE
    }
}
