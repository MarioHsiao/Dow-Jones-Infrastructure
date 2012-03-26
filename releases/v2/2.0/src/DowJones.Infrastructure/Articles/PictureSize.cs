using DowJones.Attributes;

namespace DowJones.Articles
{
    public enum PictureSize
    {
        [AssignedToken("pictureSizeLarge")] 
        Large,
        [AssignedToken("pictureSizeSmall")] 
        Small
    }
}