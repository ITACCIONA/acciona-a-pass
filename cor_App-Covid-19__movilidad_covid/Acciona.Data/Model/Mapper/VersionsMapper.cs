using Acciona.Data.Model.Versions;
using Acciona.Domain.Model.Versions;
using System;

namespace Acciona.Data.Model.Mapper
{
    public class VersionsMapper : BaseMapper
    {
        public static VersionsInfo MapVersionsInfo(VersionsInfoData data)
        {
            return new VersionsInfo()
            {
                AndroidMinVersion=data.androidMinVersion,
                AndroidRecomendedVersion=data.androidRecomendedVersion,
                IosMinVersion=data.iOSMinVersion,
                IosRecomendedVersion=data.iOSRecomendedVersion,
                SeguridadMinVersion=data.seguridadMinVersion,
                SeguridadRecomendedVersion=data.seguridadRecomendedVersion
            };                        
        }
    }
}
