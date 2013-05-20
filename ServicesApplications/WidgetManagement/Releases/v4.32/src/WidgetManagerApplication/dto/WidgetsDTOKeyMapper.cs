using System;
using System.Data;
using factiva.nextgen.dto;
using EMG.widgets.ui.dto.request;

namespace EMG.widgets.ui.dto
{
    /// <summary>
    /// 
    /// </summary>
    public class WidgetsDTOKeyMapper : DTOKeyMapper
    {
        static WidgetsDTOKeyMapper()
        {
            InsertDTOMapping(typeof (WidgetManagementDTO));
            InsertDTOMapping(typeof (EncryptedRenderWidgetDTO));
            InsertDTOMapping(typeof (ReadspeakerContentDTO));
            InsertDTOMapping(typeof (HeadlinePluginWidgetRequestDTO));
        }

        /// <summary>
        /// Gets the dto id.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        new public static string GetDtoId(Type type)
        {
            object obj = dtoToKeyMapper[type];
            if (obj != null)
            {
                return obj.ToString();
            }
            throw new NoNullAllowedException("Missing DTO id for: " + type.FullName);
        }
    }
}