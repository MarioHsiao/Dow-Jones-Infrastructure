namespace EMG.widgets.ui.dto.lob
{
    /// <summary>
    /// 
    /// </summary>
    public class AbstractWidgetRequestDTO : factiva.nextgen.dto.AbstractRequestDTO
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public override string Key
        {
            get { return (WidgetsDTOKeyMapper.GetDtoId(GetType())); }
        }
    }
}