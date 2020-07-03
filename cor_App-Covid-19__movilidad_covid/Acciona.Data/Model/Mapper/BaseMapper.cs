using System;
using System.Collections.Generic;
using System.Text;
using Acciona.Domain.Model.Base;

namespace Acciona.Data.Model.Mapper
{
    public class BaseMapper
    {
        //Date Format
        public const string DATE_FORTMAT = "yyyy/MM/dd HH:mm";

        public static DateTime MapDate(long date)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(date).ToLocalTime();
            return dtDateTime;
        }

        public static long ReverseMapDate(DateTime date)
        {
            return (long)(date.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public FileResponse MapFileResponse(FileResponseData data)
        {
            return new FileResponse()
            {
                Data = data.Data,
                ContentType = data.ContentType
            };
        }
    }
}
