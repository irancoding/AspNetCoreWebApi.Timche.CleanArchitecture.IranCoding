namespace Application.Wrappers
{
    public class CustomActionResult<T>
    {
        public bool Success { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
    public class CustomActionResult : CustomActionResult<byte> { }
}
