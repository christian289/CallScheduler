using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace CallSchedulerCore.Helper
{
    /// <summary>
    /// 형변환 추가 기능
    /// </summary>
    public static class ConversionHelper
    {
        #region Enum Valiables
        public enum DateTimeFormat
        {
            yyyy,
            yyyyMM,
            yyyyMMdd,
            yyyyMMddHH,
            yyyyMMddHHmm,
            yyyyMMddHHmmss,
            yyyyMMddHHmmssfff,
            MMddyyyy,
            MMddyyyyHHmmss,
            MMddyyHHmmtt,
            yyMMdd,
            MMddyy
        }
        #endregion

        #region ToStringEx
        /// <summary>
        /// 문자열 변환 flaot -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="obj">string</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this float value)
        {
            try
            {
                if (ReferenceEquals(value, null) || string.IsNullOrEmpty(value.ToString()))
                {
                    return "0";
                }

                return value.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 문자열 변환 object -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this object value, string defaultValue = "")
        {
            try
            {
                if (ReferenceEquals(value, null) || ReferenceEquals(value, DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultValue;
                }

                return value.ToString().Trim();
            }
            catch { return string.Empty; }

        }

        /// <summary>
        /// 문자열 변환 decimal -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="value">decimal</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this decimal value)
        {
            try
            {
                if (ReferenceEquals(value, null) || string.IsNullOrEmpty(value.ToString()))
                {
                    return "0";
                }

                return value.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary> 
        /// 문자열 변환 DateTime -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this DateTime value, string defaultValue = "")
        {
            try
            {
                if (ReferenceEquals(value, null) || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultValue == "" ? DateTime.Now.ToString() : defaultValue;
                }

                return value.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 문자열 변환 string -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this string value, string defaultValue = "")
        {
            try
            {
                if (ReferenceEquals(value, null) || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultValue;
                }

                return value.Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 문자열 변환 bool -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="value">string</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this bool value, bool defaultValue = true)
        {
            try
            {
                if (ReferenceEquals(value, null) || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultValue.ToString();
                }

                return value.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 문자열 변환 byte -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="value">byte</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this byte value)
        {
            try
            {
                if (ReferenceEquals(value, null))
                {
                    return "";
                }
                //Text.Encoding.Default.GetString(new byte[] { value });
                return value.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 문자열 변환 long -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="value">long</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this long value, long defaultValue = 0)
        {
            try
            {
                if (ReferenceEquals(value, null) || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultValue.ToString();
                }

                return value.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 문자열 변환 int -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="value">int</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this int value, int defaultValue = 0)
        {
            try
            {
                if (ReferenceEquals(value, null) || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultValue.ToString();
                }

                return value.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 문자열 변환 short -> string, Trim 적용해서 리턴
        /// </summary>
        /// <param name="value">short</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>문자열 리턴</returns>
        public static string ToStringEx(this short value, short defaultValue = 0)
        {
            try
            {
                if (ReferenceEquals(value, null) || string.IsNullOrEmpty(value.ToString()))
                {
                    return defaultValue.ToString();
                }

                return value.ToString().Trim();
            }
            catch { return string.Empty; }
        }

        #endregion

        #region ToBooleanEx
        /// <summary>
        /// bool 타입 변환 object(string) -> bool, Null 또는 다른 타입은 false
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBooleanEx(this object value)
        {
            try
            {
                if (value is bool)
                {
                    if (ReferenceEquals(value, null) || ReferenceEquals(value, DBNull.Value))
                    {
                        return false;
                    }
                    else
                    {
                        return (bool)value;
                    }
                }
                else if (value is string)
                {
                    return value.ToStringEx().Equals("Y") ? true : false;
                }
                else
                {
                    return Convert.ToBoolean(value);
                }
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환 object(string) -> bool, Null 또는 다른 타입은 false
        /// </summary>
        /// <param name="value">string</param>
        /// <returns></returns>
        public static bool ToBooleanEx(this string value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return false;
                }


                return value.Equals("Y") ? true : false;
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환, Null 값을 false로 변환
        /// </summary>
        /// <param name="value">bool 값</param>
        /// <returns>Boolean 값</returns>
        public static bool ToBooleanEx(this bool value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value))
                {
                    return false;
                }
                else
                {
                    return value;
                }
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환, Null 값을 false로 변환
        /// </summary>
        /// <param name="value">DateTime 값</param>
        /// <returns>Boolean 값</returns>
        public static bool ToBooleanEx(this DateTime value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환, Null 값을 false로 변환
        /// </summary>
        /// <param name="value">decimal 값</param>
        /// <returns>Boolean 값</returns>
        public static bool ToBooleanEx(this decimal value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환, Null 값을 false로 변환
        /// </summary>
        /// <param name="value">double 값</param>
        /// <returns>Boolean 값</returns>
        public static bool ToBooleanEx(this double value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환, Null 값을 false로 변환
        /// </summary>
        /// <param name="value">float 값</param>
        /// <returns>Boolean 값</returns>
        public static bool ToBooleanEx(this float value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환, Null 값을 false로 변환
        /// </summary>
        /// <param name="value">int 값</param>
        /// <returns>Boolean 값</returns>
        public static bool ToBooleanEx(this int value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환, Null 값을 false로 변환
        /// </summary>
        /// <param name="value">short 값</param>
        /// <returns>Boolean 값</returns>
        public static bool ToBooleanEx(this short value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환, Null 값을 false로 변환
        /// </summary>
        /// <param name="value">char 값</param>
        /// <returns>Boolean 값</returns>
        public static bool ToBooleanEx(this char value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return false; }
        }

        /// <summary>
        /// bool 타입 변환, Null 값을 false로 변환
        /// </summary>
        /// <param name="value">byte 값</param>
        /// <returns>Boolean 값</returns>
        public static bool ToBooleanEx(this byte value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch { return false; }
        }

        #endregion

        #region ToBooleanStringEx
        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>Y or N</returns>
        public static string ToBooleanStringEx(this object value)
        {
            try
            {
                if (value is bool)
                {
                    if (value is null || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                    {
                        return "N";
                    }
                    else
                    {
                        return ((bool)value == true ? "Y" : "N");
                    }
                }
                else if (value is string)
                {
                    if (value is null || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                    {
                        return "N";
                    }
                    else
                    {
                        return value.ToString().Equals("Y") ? "Y" : "N";
                    }
                }
                else
                {
                    return Convert.ToBoolean(value) ? "Y" : "N";
                }
            }
            catch { return "N"; }
        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this string value)
        {
            try
            {
                if (value is null || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return "N";
                }
                else
                {
                    return value.ToString().Equals("Y") ? "Y" : "N";
                }
            }
            catch { return "N"; }
        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">bool</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this bool value)
        {
            try
            {
                return value ? "Y" : "N";
            }
            catch { return "N"; }

        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this DateTime value)
        {
            try
            {
                return Convert.ToBoolean(value) ? "Y" : "N";
            }
            catch { return "N"; }
        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">decimal</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this decimal value)
        {
            try
            {
                return Convert.ToBoolean(value) ? "Y" : "N";
            }
            catch { return "N"; }
        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">double</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this double value)
        {
            try
            {
                return Convert.ToBoolean(value) ? "Y" : "N";
            }
            catch { return "N"; }
        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">float</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this float value)
        {
            try
            {
                return Convert.ToBoolean(value) ? "Y" : "N";
            }
            catch { return "N"; }
        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">int</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this int value)
        {
            try
            {
                return Convert.ToBoolean(value) ? "Y" : "N";
            }
            catch { return "N"; }
        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">short</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this short value)
        {
            try
            {
                return Convert.ToBoolean(value) ? "Y" : "N";
            }
            catch { return "N"; }
        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">char</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this char value)
        {
            try
            {
                return Convert.ToBoolean(value) ? "Y" : "N";
            }
            catch { return "N"; }
        }

        /// <summary>
        /// "Y","N" 형식으로 변환 
        /// </summary>
        /// <param name="value">byte</param>
        /// <returns>Y or N</returns>
        public static string ToStringBooleanEx(this byte value)
        {
            try
            {
                return Convert.ToBoolean(value) ? "Y" : "N";
            }
            catch { return "N"; }
        }
        #endregion

        #region ToInt16Ex
        /// <summary>
        /// int 값을 Int16로 변환
        /// </summary>
        /// <param name="value">int값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this int value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// long 값을 Int16로 변환
        /// </summary>
        /// <param name="value">long값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this long value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// DateTime값 값을 Int16로 변환
        /// </summary>
        /// <param name="value">DateTime값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this DateTime value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// bool값 값을 Int16로 변환
        /// </summary>
        /// <param name="value">bool값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this bool value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// char값 값을 Int16로 변환
        /// </summary>
        /// <param name="value">char값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this char value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// byte값 값을 Int16로 변환
        /// </summary>
        /// <param name="value">byte값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this byte value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// short값 값을 Int16로 변환
        /// </summary>
        /// <param name="value">short값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this short value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Object 형식의 값을 Int16 형식으로 변환
        /// </summary>
        /// <param name="value">Object 값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt16(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// string값 값을 Int16로 변환
        /// </summary>
        /// <param name="value">string값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this string value)
        {
            try
            {
                if (value is null || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return short.Parse(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// Decimal 값을 Int16로 변환
        /// </summary>
        /// <param name="value">Decimal값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this decimal value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Double 형식의 값을 Int16롤 변환
        /// </summary>
        /// <param name="value">Double값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this double value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// float 값을 Int16로 변환
        /// </summary>
        /// <param name="value">float값</param>
        /// <returns>Int16값</returns>
        public static short ToInt16Ex(this float value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch { return 0; }
        }
        #endregion

        #region ToInt32Ex
        /// <summary>
        /// int 값을 Int32로 변환
        /// </summary>
        /// <param name="value">int값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this int value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// long 값을 Int32로 변환
        /// </summary>
        /// <param name="value">long값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this long value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// DateTime값 값을 Int32로 변환
        /// </summary>
        /// <param name="value">DateTime값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this DateTime value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// bool값 값을 Int32로 변환
        /// </summary>
        /// <param name="value">bool값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this bool value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// char값 값을 Int32로 변환
        /// </summary>
        /// <param name="value">char값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this char value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// byte값 값을 Int32로 변환
        /// </summary>
        /// <param name="value">byte값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this byte value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// short값 값을 Int32로 변환
        /// </summary>
        /// <param name="value">short값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this short value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Object 형식의 값을 Int32 형식으로 변환
        /// </summary>
        /// <param name="value">Object 값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// string값 값을 Int32로 변환
        /// </summary>
        /// <param name="value">string값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this string value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Int32.Parse(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// Decimal 값을 Int32로 변환
        /// </summary>
        /// <param name="value">Decimal값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this decimal value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Double 형식의 값을 Int32롤 변환
        /// </summary>
        /// <param name="value">Double값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this double value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// float 값을 Int32로 변환
        /// </summary>
        /// <param name="value">float값</param>
        /// <returns>Int32값</returns>
        public static int ToInt32Ex(this float value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }
        #endregion

        #region ToInt64Ex
        /// <summary>
        /// int 값을 Int64로 변환
        /// </summary>
        /// <param name="value">int값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this int value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// long 값을 Int64로 변환
        /// </summary>
        /// <param name="value">long값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this long value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// DateTime값 값을 Int64로 변환
        /// </summary>
        /// <param name="value">DateTime값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this DateTime value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// bool값 값을 Int64로 변환
        /// </summary>
        /// <param name="value">bool값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this bool value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// char값 값을 Int64로 변환
        /// </summary>
        /// <param name="value">char값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this char value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// byte값 값을 Int64로 변환
        /// </summary>
        /// <param name="value">byte값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this byte value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// short값 값을 Int64로 변환
        /// </summary>
        /// <param name="value">short값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this short value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Object 형식의 값을 Int64 형식으로 변환
        /// </summary>
        /// <param name="value">Object 값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt64(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// string값 값을 Int64로 변환
        /// </summary>
        /// <param name="value">string값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this string value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Int64.Parse(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// Decimal 값을 Int64로 변환
        /// </summary>
        /// <param name="value">Decimal값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this decimal value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Double 형식의 값을 Int64롤 변환
        /// </summary>
        /// <param name="value">Double값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this double value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// float 값을 Int64로 변환
        /// </summary>
        /// <param name="value">float값</param>
        /// <returns>Int64값</returns>
        public static long ToInt64Ex(this float value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch { return 0; }
        }
        #endregion

        #region ToDecimalEx
        /// <summary>
        /// int 값을 decimal로 변환
        /// </summary>
        /// <param name="value">int값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this int value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// long 값을 decimal로 변환
        /// </summary>
        /// <param name="value">long값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this long value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// DateTime값 값을 decimal로 변환
        /// </summary>
        /// <param name="value">DateTime값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this DateTime value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// bool값 값을 decimal로 변환
        /// </summary>
        /// <param name="value">bool값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this bool value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// char값 값을 decimal로 변환
        /// </summary>
        /// <param name="value">char값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this char value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// byte값 값을 decimal로 변환
        /// </summary>
        /// <param name="value">byte값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this byte value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// short값 값을 decimal로 변환
        /// </summary>
        /// <param name="value">short값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this short value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Object 형식의 값을 decimal형식으로 변환
        /// </summary>
        /// <param name="value">Object 값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// string값 값을 decimal로 변환
        /// </summary>
        /// <param name="value">string값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this string value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Decimal.Parse(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// Decimal 값을 decimal로 변환
        /// </summary>
        /// <param name="value">Decimal값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this decimal value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Double 형식의 값을 decimal롤 변환
        /// </summary>
        /// <param name="value">Double값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this double value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// float 값을 decimal로 변환
        /// </summary>
        /// <param name="value">float값</param>
        /// <returns>decimal값</returns>
        public static decimal ToDecimalEx(this float value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }
        #endregion

        #region ToDoubleEx
        /// <summary>
        /// int 값을 double로 변환
        /// </summary>
        /// <param name="value">int값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this int value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// long 값을 double로 변환
        /// </summary>
        /// <param name="value">long값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this long value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// DateTime값 값을 double로 변환
        /// </summary>
        /// <param name="value">DateTime값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this DateTime value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// bool값 값을 double로 변환
        /// </summary>
        /// <param name="value">bool값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this bool value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// char값 값을 double로 변환
        /// </summary>
        /// <param name="value">char값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this char value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// byte값 값을 double로 변환
        /// </summary>
        /// <param name="value">byte값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this byte value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// short값 값을 double로 변환
        /// </summary>
        /// <param name="value">short값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this short value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Object 형식의 값을 double형식으로 변환
        /// </summary>
        /// <param name="value">Object 값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDouble(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// string값 값을 double로 변환
        /// </summary>
        /// <param name="value">string값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this string value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Double.Parse(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// Decimal 값을 double로 변환
        /// </summary>
        /// <param name="value">Decimal값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this decimal value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Double 형식의 값을 double롤 변환
        /// </summary>
        /// <param name="value">Double값</param>
        /// <returns>double값</returns>
        public static decimal ToDoubleEx(this double value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// float 값을 double로 변환
        /// </summary>
        /// <param name="value">float값</param>
        /// <returns>double값</returns>
        public static double ToDoubleEx(this float value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }
        #endregion

        #region ToSingleEx
        /// <summary>
        /// int 값을 float로 변환
        /// </summary>
        /// <param name="value">int값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this int value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// long 값을 float로 변환
        /// </summary>
        /// <param name="value">long값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this long value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// DateTime값 값을 float로 변환
        /// </summary>
        /// <param name="value">DateTime값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this DateTime value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// bool값 값을 float로 변환
        /// </summary>
        /// <param name="value">bool값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this bool value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// char값 값을 float로 변환
        /// </summary>
        /// <param name="value">char값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this char value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// byte값 값을 float로 변환
        /// </summary>
        /// <param name="value">byte값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this byte value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// short값 값을 float로 변환
        /// </summary>
        /// <param name="value">short값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this short value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Object 형식의 값을 float형식으로 변환
        /// </summary>
        /// <param name="value">Object 값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToSingle(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// string값 값을 float로 변환
        /// </summary>
        /// <param name="value">string값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this string value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return float.Parse(value);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// Decimal 값을 float로 변환
        /// </summary>
        /// <param name="value">Decimal값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this decimal value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// Double 형식의 값을 float롤 변환
        /// </summary>
        /// <param name="value">Double값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this double value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// float 값을 float로 변환
        /// </summary>
        /// <param name="value">float값</param>
        /// <returns>float값</returns>
        public static float ToSingleEx(this float value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch { return 0; }
        }
        #endregion

        #region ToDateTimeEx
        /// <summary>
        /// object 형식을 DateTime으로 변환
        /// </summary>
        /// <param name="value">Object값</param>
        /// <returns>DateTime 값</returns>
        public static DateTime ToDateTimeEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null))
                    return Convert.ToDateTime("1900-01-01");

                if (string.IsNullOrEmpty(value.ToString()))
                    return Convert.ToDateTime("1900-01-01");

                return Convert.ToDateTime(value);
            }
            catch { return Convert.ToDateTime("1900-01-01"); }
        }

        /// <summary>
        /// object 형식을 DateTime으로 변환
        /// </summary>
        /// <param name="value">Object값</param>
        /// <param name="defaultdate">기본일자</param>
        /// <returns>DateTime값</returns>
        public static DateTime ToDateTimeEx(this object value, DateTime defaultdate)
        {
            try
            {
                if (ReferenceEquals(value, null) || (value is string && string.IsNullOrEmpty(value.ToString())))
                {
                    return defaultdate;
                }

                return Convert.ToDateTime(value);
            }
            catch { return defaultdate; }
        }

        /// <summary>
        /// YYYYMMDD 포멧의 문자열을 DateTime 으로 Convert
        /// </summary>
        /// <param name="str">YYYYMMDD 포멧의 날짜</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 8)
                return Convert.ToDateTime("1900-01-01");

            try
            {
                return Convert.ToDateTime(string.Format("{0}-{1}-{2}", str.Substring(0, 4), str.Substring(4, 2), str.Substring(6, 2)));
            }
            catch
            {
                return Convert.ToDateTime("1900-01-01");
            }
        }
        #endregion

        #region ToTimeSpanEx
        /// <summary>
        /// Dattime을 TimeSpan 형식으로 변경
        /// </summary>
        /// <param name="time">DateTime 값</param>
        /// <returns>TimeSpan 값</returns>
        public static TimeSpan ToTimeSpanEx(this DateTime time)
        {
            try
            {
                return new TimeSpan(time.Day, time.Hour, time.Minute, time.Second, time.Millisecond);
            }
            catch { return new TimeSpan(); }
        }
        #endregion

        #region ToByteEX
        /// <summary>
        /// Object를 byte형으로 변환
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>byte</returns>
        public static byte ToByteEx(this object value)
        {
            try
            {
                return Convert.ToByte(value);
            }
            catch { return Convert.ToByte(System.Byte.MinValue); }
        }

        /// <summary>
        /// Object를 byte[]형으로 변환(Compress)
        /// </summary>
        /// <param name="value">Object</param>
        /// <returns>byte[]</returns>
        public static byte[] ToBytesEx(this object value)
        {
            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //Serialize
                    binaryFormatter.Serialize(memoryStream, value);

                    byte[] inbyt = memoryStream.ToArray();

                    using (MemoryStream objStream = new MemoryStream())
                    {
                        using (DeflateStream objZS = new DeflateStream(objStream, CompressionMode.Compress))
                        {
                            objZS.Write(inbyt, 0, inbyt.Length);
                            objZS.Flush();
                            objZS.Close();
                        }
                        return objStream.ToArray();
                    }
                }
            }
            catch { return null; }
        }
        #endregion

        #region ToObjectEx
        /// <summary>
        /// byte[] 배열을 Object 타입으로 리턴 (DeCompress)
        /// </summary>
        /// <param name="value">byte[] 타입</param>
        /// <returns>object</returns>
        public static object ToObjectEx(byte[] value)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(value))
                {
                    memoryStream.Seek(0, 0);
                    using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress, true))
                    {
                        byte[] buffer = ReadFullStream(deflateStream);
                        deflateStream.Flush();
                        deflateStream.Close();

                        using (MemoryStream outStream = new MemoryStream(buffer))
                        {
                            outStream.Seek(0, 0);
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            return binaryFormatter.Deserialize(outStream);
                        }
                    }
                }
            }
            catch { throw; }
        }

        private static byte[] ReadFullStream(Stream stream)
        {
            try
            {
                byte[] buffer = new byte[2048];

                using (MemoryStream ms = new MemoryStream())
                {
                    while (true)
                    {
                        int read = stream.Read(buffer, 0, buffer.Length);

                        if (read <= 0)
                            return ms.ToArray();

                        ms.Write(buffer, 0, read);
                    }
                }
            }
            catch { throw; }
        }
        #endregion

        #region ToRegexEx
        /// <summary>
        /// 숫자만 추출해서 int16형으로 변환
        /// </summary>
        /// <param name="value">Object값</param>
        /// <returns>short</returns>
        public static short ToInt16RegexEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }

                string num = Regex.Replace(value.ToString(), @"\D", "");

                return Int16.Parse(num);

            }
            catch { return 0; }
        }

        /// <summary>
        /// 숫자만 추출해서 Int32형으로 변환
        /// </summary>
        /// <param name="value">Object값</param>
        /// <returns>int</returns>
        public static int ToInt32RegexEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }

                string num = Regex.Replace(value.ToString(), @"\D", "");

                return Int32.Parse(num);

            }
            catch { return 0; }
        }

        /// <summary>
        /// 숫자만 추출해서 Int64형으로 변환
        /// </summary>
        /// <param name="value">Object값</param>
        /// <returns>long</returns>
        public static long ToInt64RegexEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }

                string num = Regex.Replace(value.ToString(), @"\D", "");

                return Int32.Parse(num);

            }
            catch { return 0; }
        }

        /// <summary>
        /// 숫자만 추출해서 float형으로 변환
        /// </summary>
        /// <param name="value">Object값</param>
        /// <returns>float</returns>
        public static float ToSingleRegexEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }

                string num = Regex.Replace(value.ToString(), @"\D", "");

                return float.Parse(num);

            }
            catch { return 0; }
        }

        /// <summary>
        /// 숫자만 추출해서 Decimal형으로 변환
        /// </summary>
        /// <param name="value">Object값</param>
        /// <returns>decimal</returns>
        public static decimal ToDecimalRegexEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }

                string num = Regex.Replace(value.ToString(), @"\D", "");

                return Decimal.Parse(num);

            }
            catch { return 0; }
        }

        /// <summary>
        /// 숫자만 추출해서 Double형으로 변환
        /// </summary>
        /// <param name="value">Object값</param>
        /// <returns>double</returns>
        public static double ToDoubleRegexEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }

                string num = Regex.Replace(value.ToString(), @"\D", "");

                return Double.Parse(num);

            }
            catch { return 0; }
        }

        /// <summary>
        /// 숫자만 추출해서 String형으로 변환
        /// </summary>
        /// <param name="value">Object값</param>
        /// <returns>double</returns>
        public static string ToStringRegexEx(this object value)
        {
            try
            {
                if (ReferenceEquals(value, null) || value.Equals(DBNull.Value) || string.IsNullOrEmpty(value.ToString()))
                {
                    return string.Empty;
                }

                return Regex.Replace(value.ToString(), @"\D", "");
            }
            catch { return string.Empty; }
        }
        #endregion

        #region ToXmlEx
        public static string ToStringXmlEx(this object value, Type type)
        {
            try
            {
                //넘어온값이 없다면 Null 리턴
                if (ReferenceEquals(value, null))
                    return string.Empty;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //XmlSerializer를 선언
                    XmlSerializer xmlSerializer = new XmlSerializer(type);

                    //MemoryStream에 객체를 Xml로 쓴다
                    xmlSerializer.Serialize(memoryStream, value);

                    //String으로 처리하기 위해 처음으로 위치
                    memoryStream.Position = 0;

                    //StreamReader 객체를 이용하여 MemoryStream에서 String으로 변환후 리턴
                    using (StreamReader streamReader = new StreamReader(memoryStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch { return string.Empty; }
        }

        public static object ToXmlStringEx(this string value, Type type)
        {
            try
            {
                //넘어온값이 없다면 Null 리턴
                if (string.IsNullOrEmpty(value))
                    return null;

                //TextReader 객체 생성
                using (TextReader textReader = new StringReader(value))
                {
                    //XmlSerializer를 선언
                    XmlSerializer xmlSerializer = new XmlSerializer(type);

                    //TextReader 형을 XmlReader형 객체로 변환
                    XmlReader xmlReader = XmlReader.Create(textReader);

                    //객체로 Deserialize
                    return xmlSerializer.Deserialize(xmlReader);
                }
            }
            catch { return null; }
        }
        #endregion

        #region ToFormatStringEx

        public static string ToFormatStringEx(this DateTime date, DateTimeFormat format = DateTimeFormat.yyyyMMdd, bool dash = true, string dashString = "-")
        {
            try
            {
                //리턴객체
                string convertString = string.Empty;

                //날짜가 없으면 0001-01-01 로 오기 때문에 강제로 처리
                if (date.Year < 1900) return convertString;

                switch (format)
                {
                    case DateTimeFormat.yyyy:
                        convertString = "yyyy"; break;
                    case DateTimeFormat.yyyyMM:
                        convertString = String.Format("yyyy{0}MM", dashString); break;
                    case DateTimeFormat.yyyyMMdd:
                        convertString = String.Format("yyyy{0}MM{0}dd", dashString); break;
                    case DateTimeFormat.yyyyMMddHH:
                        convertString = String.Format("yyyy{0}MM{0}dd HH", dashString); break;
                    case DateTimeFormat.yyyyMMddHHmm:
                        convertString = String.Format("yyyy{0}MM{0}dd HH:mm", dashString); break;
                    case DateTimeFormat.yyyyMMddHHmmss:
                        convertString = String.Format("yyyy{0}MM{0}dd HH:mm:ss", dashString); break;
                    case DateTimeFormat.yyyyMMddHHmmssfff:
                        convertString = String.Format("yyyy{0}MM{0}dd HH:mm:ss,fff", dashString); break;
                    case DateTimeFormat.MMddyyyy:
                        convertString = String.Format("MM{0}dd{0}yyyy", dashString); break;
                    case DateTimeFormat.MMddyyyyHHmmss:
                        convertString = String.Format("MM{0}dd{0}yyyy HH:mm:ss", dashString); break;
                    case DateTimeFormat.MMddyyHHmmtt:
                        convertString = String.Format("MM{0}dd{0}yy HH:mm tt", dashString); break;
                    case DateTimeFormat.yyMMdd:
                        convertString = String.Format("yy{0}MM{0}dd", dashString); break;
                    default:
                        convertString = string.Empty; break;
                }

                convertString = date.ToString(convertString);

                //dash를 사용하지 않으면 지운다
                if (!dash)
                {
                    convertString = convertString.Replace(dashString, "");
                    convertString = convertString.Replace(":", "");
                }

                //값 리턴
                return convertString;
            }
            catch { throw; }
        }

        #endregion

        #region ToRoundAmountStringEx
        public static string ToRoundAmountStringEx(this object value, int point = 0)
        {
            try
            {
                //POINT값이 0보다 작으면 빈값 리턴
                if (point < 0) return string.Empty;

                //넘어온값을 Decimal로 변경한다
                decimal amount = value.ToDecimalEx();

                //Round 처리한다
                amount = Math.Round(amount, point, MidpointRounding.AwayFromZero);

                //Format처리
                string strFormat = "###,###,###,##0";

                //값이 Null, 0, "" 이 아니면 소수점을 처리
                if (point > 0)
                {
                    strFormat += ".";
                    for (int i = 0; i < point; i++) strFormat += "0";
                }

                //Format을 씌워서 리턴
                return string.Format("{0:" + strFormat + "}", amount);
            }
            catch { throw; }
        }
        #endregion

        public static bool isNumeric(this string sourceString)
        {
            int temp;
            return int.TryParse(sourceString, out temp);
        }

        public static object CopyObject(this object obj)
        {
            //메모리 스트림 개체 생성
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    //넘어온개체가 Null이면 나간다
                    if (obj == null) return null;

                    //바이너리 포맷터 선언
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    // This line is explained in the "Streaming Contexts" section
                    binaryFormatter.Context = new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone);

                    //바이너리 포메터 변환
                    binaryFormatter.Serialize(memoryStream, obj);

                    //Deserialize을 하기 위해 위치를 0으로 초기화
                    memoryStream.Position = 0;

                    //개체에 다시 넣음
                    return binaryFormatter.Deserialize(memoryStream);
                }
                catch { throw; }
                finally { memoryStream.Close(); }
            }
        }

        /// <summary>
        /// XXXXXX 마스크 적용 함수
        /// </summary>
        /// <param name="value">마스크 적용할 값</param>
        /// <param name="startlength">앞쪽 제외 문자 길이</param>
        /// <param name="endlength">뒤쪽 제외 문자 길이</param>
        /// <param name="masktype">마스크 표시 문자</param>
        /// <returns>마스크 적용된 값</returns>
        public static string MaskStringEx(this string value, int startlength = 0, int endlength = 0, string masktype = "X")
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return value;

                //넘겨받은 문자의 길이
                int valueLength = value.Length;

                //마스크 제외한 부분보다  value의 길이가 작으면 value를 보낸다.
                if (valueLength < startlength + endlength)
                    return value;

                //마스크 씌울 문자
                string maskText = string.Empty;

                for (int i = 0; i < valueLength; i++)
                {
                    maskText += masktype;
                }

                //전체 길이 중 제외 길이를 뺀 길이 만큼 마스크 글자를 구한다.
                string maskString = maskText.Substring(0, valueLength - startlength - endlength);
                //앞자리 + 마스크 + 뒷자리
                return value.Substring(0, startlength) + maskString + value.Substring(startlength + maskString.Length, endlength);
            }
            catch { return value; }
        }
        /*
        public static IList ToListEx(this DataTable dt, Type _type)
        {
            Type type = _type.GetType();
            Type listType = typeof(List<>).MakeGenericType(new[] { type });
            IList list = Activator.CreateInstance(listType) as IList;

            var Entity = Activator.CreateInstance(type);


            // 데이터 테이블 컬럼 가져와서, Entity와 1:1 매칭
            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();


            foreach (DataRow dr in dt.Rows)
            {
            }
        }
        */

        public static T ToObjectEx<T>(this DataRow row) where T : class, new()
        {
            T obj = new T();

            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.Name.Contains("Nullable"))
                    {
                        if (!string.IsNullOrEmpty(row[prop.Name].ToString()))
                        {
                            prop.SetValue(obj, Convert.ChangeType(row[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType), null));
                        }
                    }
                    else
                    {
                        prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType), null);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return obj;
        }

        public static List<T> ToListEx<T>(this DataTable dt) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in dt.AsEnumerable())
                {
                    var obj = row.ToObjectEx<T>();

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
