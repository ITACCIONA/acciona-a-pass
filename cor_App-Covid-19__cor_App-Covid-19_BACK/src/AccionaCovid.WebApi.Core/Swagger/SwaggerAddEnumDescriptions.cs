using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccionaCovid.WebApi.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerAddEnumDescriptions : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<ApiParameterDescription> paramDescriptors;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            paramDescriptors = context.ApiDescriptions.SelectMany(ad => ad.ParameterDescriptions);
            foreach (KeyValuePair<string, OpenApiSchema> schemaDictionaryItem in swaggerDoc.Components.Schemas)
            {
                OpenApiSchema schema = schemaDictionaryItem.Value;

                IList<IOpenApiAny> schemEnums = schema.Enum;
                if (schemEnums != null && schemEnums.Count > 0)
                {
                    if (schemaDictionaryItem.Key.EndsWith("Nullable"))
                    {
                        Type selectedType = paramDescriptors.SelectMany(pd => pd.Type.GenericTypeArguments)
                            .FirstOrDefault(t => t.Name == schemaDictionaryItem.Key.Replace("Nullable", ""));
                        schema.Description += $" >>> {DescribeEnum(selectedType)}";
                    }
                    else
                    {
                        Type selectedType = paramDescriptors.FirstOrDefault(pd => pd.Type.Name == schemaDictionaryItem.Key)?.Type;
                        schema.Description += $" >>> {DescribeEnum(selectedType)}";
                    }
                }
            }

            // add enum descriptions to input parameters
            if (swaggerDoc.Paths.Count > 0)
            {
                foreach (OpenApiPathItem pathItem in swaggerDoc.Paths.Values)
                {
                    DescribeEnumParameters(pathItem.Parameters);

                    // head, patch, options, delete left out
                    List<OpenApiOperation> possibleParameterisedOperations = pathItem.Operations.Values.ToList();
                    possibleParameterisedOperations.FindAll(x => x != null).ForEach(x => DescribeEnumParameters(x.Parameters));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        private void DescribeEnumParameters(IList<OpenApiParameter> parameters)
        {
            if (parameters != null)
            {
                foreach (OpenApiParameter param in parameters)
                {
                    Type paramEnums = paramDescriptors.FirstOrDefault(pd => pd.Name == param.Name)?.Type;
                    if (paramEnums.IsGenericType && paramEnums.IsNullable()) paramEnums = paramEnums.GetGenericArguments()[0];
                    if (paramEnums != null && paramEnums.IsEnum)
                    {
                        param.Description += $" >>> {DescribeEnum(paramEnums)}";
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramEnums"></param>
        /// <returns></returns>
        private string DescribeEnum(Type paramEnums)
        {
            if (paramEnums == null || !paramEnums.IsEnum) return "";

            List<string> enumDescriptions = new List<string>();
            foreach (object enumValue in paramEnums.GetEnumValues())
            {
                enumDescriptions.Add(string.Format("{0} = {1}", (int)enumValue, paramEnums.GetEnumName(enumValue)));
            }

            string des = string.Join(", ", enumDescriptions.ToArray());

            return des;
        }
    }
}
