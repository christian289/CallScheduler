namespace CallSchedulerCore.Helper
{
    public static class StringHelper
    {
        /// <summary>
        /// 문자열 왼쪽편처음부터 지정된 문자열값 리턴(VBScript Left기능)
        /// </summary>
        /// <param name="target">얻을 문자열</param>
        /// <param name="length">얻을 문자열길이</param>
        /// <returns>얻은 문자열 값</returns>
        public static string Left(this string target, int length)
        {
            if (length <= target.Length)
            {
                return target.Substring(0, length);
            }
            return target;
        }
        /// <summary>
        /// 지정된 위치이후 모든 문자열 리턴 (VBScript Mid기능)
        /// </summary>
        /// <param name="target">얻을 문자열</param>
        /// <param name="start">얻을 시작위치</param>
        /// <returns>지정된 위치 이후 모든 문자열리턴</returns>
        public static string Mid(this string target, int start)
        {
            if (start <= target.Length)
            {
                return target.Substring(start - 1);
            }
            return string.Empty;
        }
        /// <summary>
        /// 문자열이 지정된 위치에서 지정된 길이만큼까지의 문자열 리턴 (VBScript Mid기능)
        /// </summary>
        /// <param name="target">얻을 문자열</param>
        /// <param name="start">얻을 시작위치</param>
        /// <param name="length">얻을 문자열길이</param>
        /// <returns>지정된 길이만큼의 문자열 리턴</returns>
        public static string Mid(this string target, int start, int length)
        {
            if (start <= target.Length)
            {
                if (start + length - 1 <= target.Length)
                {
                    return target.Substring(start - 1, length);
                }
                return target.Substring(start - 1);
            }
            return string.Empty;
        }
        /// <summary>
        /// 문자열 오른쪽편처음부터 지정된 문자열값 리턴(VBScript Right기능)
        /// </summary>
        /// <param name="target">얻을 문자열</param>
        /// <param name="length">얻을 문자열길이</param>
        /// <returns>얻은 문자열 값</returns>
        public static string Right(this string target, int length)
        {
            if (length <= target.Length)
            {
                return target.Substring(target.Length - length);
            }
            return target;
        }
    }
}
