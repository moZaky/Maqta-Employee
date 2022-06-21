using MAQTA.Enums;

namespace MAQTA.Models
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            this.Status = StatusCode.NotFound;
        }
        public StatusCode Status { get; set; }
        public bool IsSucceeded
        {
            get
            {
                return this.Status == StatusCode.Succeeded;
            }
        }
        public object Data { get; set; }

        public string Message { get; set; }
    }
}
