namespace MAQTA.Enums
{
    public enum StatusCode
    {
        Succeeded = 1,
        Created = 2,
        Modified = 3,

        Deleted = 6,

        Error = 0,
        Invalid = -2,
        Duplicate = -3,
        NotFound = -404,
        Acivated =88,
        Deactivated=90
    }
}